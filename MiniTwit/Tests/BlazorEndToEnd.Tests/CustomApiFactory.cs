using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MiniTwit.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;
using Xunit.Sdk;

namespace BlazorEndToEnd.Tests;
public sealed class CustomApiFactory : WebApplicationFactory<Program>, IAsyncLifetime {

    //private readonly MsSqlContainer _container = new MsSqlBuilder()
    //    .WithNetwork("TestNetwork")
    //    .WithPortBinding(Random.Shared.Next(5000, 50000), 5000)
    //    //.WithEnvironment("ASPNETCORE_URLS", "http://localhost")
    //    .Build();
    public readonly MsSqlContainer DatabaseContainer;
    private readonly Uri _baseUrl = new("http://127.0.0.1:8081");
    private bool _disposed;
    private IHost? _host;

    public CustomApiFactory() {
        try
        {
            DatabaseContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .Build();
            ClientOptions.AllowAutoRedirect = false;
            ClientOptions.BaseAddress = _baseUrl;
        }
        catch (ArgumentException ae)
        {
            throw new XunitException($"Is docker installed and running? {ae.Message}.");
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        builder.ConfigureTestServices(services =>
        {
            //services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

            //services.RemoveAll(typeof(DbConnection));

            //// Create open SqliteConnection so EF won't automatically close it.
            //services.AddSingleton<DbConnection>(container =>
            //{
            //    var connection = new SqlConnection(DatabaseContainer.GetConnectionString());
            //    connection.Open();

            //    return connection;
            //});

            //services.AddDbContext<ApplicationDbContext>((container, options) =>
            //{
            //    var connection = container.GetRequiredService<DbConnection>();
            //    options.UseSqlServer(connection);
            //});

            //var db = services.BuildServiceProvider().GetService<ApplicationDbContext>();
            //db.Database.Migrate();
            //builder.UseUrls("https://localhost:7048");
            builder.UseUrls(_baseUrl.ToString());
            builder.UseEnvironment("FullIntegrationTest").ConfigureTestServices(services =>
            {
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "FullIntegrationTest");

                // replace the DB with docker variant fill it with data
                //https://github.com/dotnet/efcore/issues/27118
                var factoryDescriptor = services
                    .FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IDbContextFactory<ApplicationDbContext>));
                if (factoryDescriptor != null)
                    services.Remove(factoryDescriptor);

                var contextDescriptor = services
                    .FirstOrDefault(descriptor => descriptor.ServiceType == typeof(ApplicationDbContext));
                if (contextDescriptor != null)
                    services.Remove(contextDescriptor);

                var factorySourceDescriptor = services
                    .FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IDbContextFactorySource<ApplicationDbContext>));
                if (factorySourceDescriptor != null)
                    services.Remove(factorySourceDescriptor);

                var dbContextDescriptor = services
                    .FirstOrDefault(descriptor => descriptor.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (dbContextDescriptor != null)
                    services.Remove(dbContextDescriptor);

                services.AddDbContextFactory<ApplicationDbContext>(opt =>
                     opt.UseSqlServer(DatabaseContainer.GetConnectionString()));

                var db = services.BuildServiceProvider().GetService<ApplicationDbContext>();
                db.Database.Migrate();


                //try
                //{
                //    lock (TestData.LockObject)
                //    {
                //        SeedTestData(db);
                //    }
                //}
                //catch (System.ArgumentException)
                //{
                //    // The DataInitializer does not detect that Data is already in the DB.
                //}
            });

        });

        //builder.UseUrls("https://localhost:1234");

        //builder.UseEnvironment("Development");
    }

    protected override IHost CreateHost(IHostBuilder builder) {
        // need to create a plain host that we can return.
        var testHost = builder.Build();

        // configure and start the actual host.
        builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());

        _host = builder.Build();
        _host.Run();

        var server = _host.Services.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>();
        ClientOptions.BaseAddress = addresses!.Addresses
            .Select(x => new Uri(x))
            .Last();

        testHost.Start();

        return testHost;
    }

    public async Task InitializeAsync() {
        await DatabaseContainer.StartAsync();
        EnsureServer();
    }

    protected override void Dispose(bool disposing) {
        base.Dispose(disposing);
        this.DisposeAsync();

        if (_disposed)
            return;

        if (disposing)
        {
            _host?.Dispose();
        }
        DatabaseContainer.StopAsync();

        _disposed = true;
    }

    private void EnsureServer() {
        if (_host is null)
        {
            // This forces WebApplicationFactory to bootstrap the server
            using var _ = CreateDefaultClient();
        }
    }

    //public Task InitializeAsync() => DatabaseContainer.StartAsync();
    Task IAsyncLifetime.DisposeAsync() => DatabaseContainer.StopAsync();
}
