using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace BlazorEndToEnd.Tests;

public class RegisterAndLoginTests : PageTest, IClassFixture<CustomApiFactory> {

    private readonly HttpClient _httpClient;
    private readonly CustomApiFactory _customApiFactory;
    private readonly string _address; 

    public RegisterAndLoginTests(CustomApiFactory customApiFactory) {
        _customApiFactory = customApiFactory;
        _address = _customApiFactory.ClientOptions.BaseAddress.ToString();
        //_customApiFactory.InitializeAsync().Wait();
        _httpClient = _customApiFactory.CreateClient();
    }

    [Fact]
    public async Task Register() {
        using var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var Page = await browser.NewPageAsync();

        await Page.GotoAsync($"{_address}Identity/Account/Register");

        await Page.FillAsync("input[name='email']", "");

        await Expect(Page).ToHaveURLAsync(new Regex(".*intro"));
    }
}