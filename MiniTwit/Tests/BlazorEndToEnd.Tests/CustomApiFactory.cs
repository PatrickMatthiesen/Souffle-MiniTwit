using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

namespace BlazorEndToEnd.Tests;
public sealed class CustomApiFactory : WebApplicationFactory<Program>, IAsyncLifetime {

    private readonly MsSqlContainer _container = new MsSqlBuilder()
        .WithNetwork("TestNetwork")
        .WithPortBinding(Random.Shared.Next(5000, 50000), 5000)
        //.WithEnvironment("ASPNETCORE_URLS", "http://localhost")
        .Build();

    public Task InitializeAsync() => _container.StartAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

            services.RemoveAll(typeof(DbConnection));

            // Create open SqliteConnection so EF won't automatically close it.
            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new SqlConnection(_container.GetConnectionString());
                connection.Open();

                return connection;
            });

            services.AddDbContext<ApplicationDbContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlServer(connection);
            });
        });

        builder.UseUrls(new string[] { "https://localhost:1234/" })
            .UseSetting("https_port", "1234");

        builder.UseEnvironment("Development");
    }

    Task IAsyncLifetime.DisposeAsync() => _container.StopAsync();
}
