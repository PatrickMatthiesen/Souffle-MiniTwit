using Microsoft.Playwright;


namespace BlazorEndToEnd.Tests;

public class RegisterAndLoginTests : IClassFixture<TestFixture> {

    //private readonly HttpClient _httpClient;
    //private readonly CustomApiFactory _customApiFactory;
    //private readonly string _address; 

    //public RegisterAndLoginTests(CustomApiFactory customApiFactory) {
    //    _customApiFactory = customApiFactory;
    //    _address = _customApiFactory.ClientOptions.BaseAddress.OriginalString;
    //    //_customApiFactory.InitializeAsync().Wait();
    //    _httpClient = _customApiFactory.CreateClient();
    //}

    private readonly IPage? Page;

    public RegisterAndLoginTests(TestFixture fixture) {
        Page = fixture.Page;
    }

    [Fact]
    public async Task Register() {
        //using var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        //await using var browser = await playwright.Chromium.LaunchAsync();
        //var Page = await browser.NewPageAsync();

        Xunit.Assert.NotNull(Page);

        var responce = await Page.GotoAsync($"https://localhost:7048"); // /Identity/Account/Register
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        Assert.NotNull(responce);
        Assert.Contains(await Page.ContentAsync(), "<!doctype html>");

        await Page.GetByPlaceholder("name@example.com").FillAsync("test@test.dk");
        await Page.FillAsync("input[name='Input.Password']", "Test1234!");
        await Page.FillAsync("input[name='Input.ConfirmPassword']", "Test1234!");
        await Page.ClickAsync("button[type='submit']");

        //await Expect(Page).ToHaveURLAsync(new Regex(".*intro"));
    }
}