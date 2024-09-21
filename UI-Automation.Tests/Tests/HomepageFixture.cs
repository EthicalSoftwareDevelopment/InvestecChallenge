using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace InvestecChallenge.WebAutomationTesting.Tests
{
    /// <summary>
    /// Tests the basic functionality and navigation of the Investec homepage.
    /// </summary>
    [TestFixture]
    public class HomepageFixture : PageTest
    {
        [SetUp] 
        public static void GlobalSetup()
        {
        }


        /// <summary>
        /// Automates navigation to the Investec Interest Rates page
        /// </summary>
        [Test]
        public async Task NavigateToInterestRates()
        {
            await using var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
            });
            var context = await browser.NewContextAsync();

            var page = await context.NewPageAsync();
            await page.GotoAsync("https://www.investec.com/en_za.html", new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle
            });
            await page.GetByLabel("Accept all cookies").ClickAsync();

            await SearchDialogForInterestRatesPage(page);

            await ManualOverrideInCaseOfMissingContent(page);

            await page.GetByRole(AriaRole.Button, new() { Name = "Sign up" }).ClickAsync();
            await FillInForm(page);
            await page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();

            await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Thank you" })).ToBeVisibleAsync();
        }

        /// <summary>
        /// If the search bar does not return the expected page, this method will manually navigate to the page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private static async Task ManualOverrideInCaseOfMissingContent(IPage page)
        {
            var pageLink = page.GetByText("understanding-interest-rates");
            if (pageLink.ToString() == String.Empty)
            {
                Console.WriteLine("Item not found on page: Manual resolution of url implemented");
                await page.GotoAsync("https://www.investec.com/en_za/focus/money/understanding-interest-rates.html",
                    new PageGotoOptions
                    {
                        WaitUntil = WaitUntilState.NetworkIdle
                    });
            }
        }

        /// <summary>
        /// Searches the dialog for the interest rates page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private static async Task SearchDialogForInterestRatesPage(IPage page)
        {
            await page.GetByRole(AriaRole.Link, new() { Name = "Search" }).ClickAsync();
            await page.GetByPlaceholder("Search Investec").ClickAsync();
            await page.GetByPlaceholder("Search Investec").FillAsync("cash investment rates information");
            await page.GetByLabel("Submit search").ClickAsync();
        }

        /// <summary>
        /// Fills in the form on the page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private static async Task FillInForm(IPage page)
        {
            await page.Locator("input[name=\"name\"]").ClickAsync();
            await page.Locator("input[name=\"name\"]").FillAsync("testname");
            await page.Locator("input[name=\"name\"]").PressAsync("Tab");
            await page.Locator("input[name=\"surname\"]").FillAsync("testlastname");
            await page.Locator("input[name=\"email\"]").ClickAsync();
            await page.Locator("input[name=\"email\"]").FillAsync("test@investec.co.za");
            await page.Locator(".dropdown__hitarea").ClickAsync();
            await page.Locator("#content li").Filter(new() { HasText = "Savings" }).ClickAsync();
            await page.Locator("input[name=\"service_other\"]").ClickAsync();
            await page.Locator("text-input").Filter(new() { HasText = "What year were you born? *" }).Locator("div")
                .Nth(1).ClickAsync();
            await page.Locator("input[name=\"year_of_birth\"]").FillAsync("1990");
            await page.Locator("div:nth-child(2) > .padding--component").ClickAsync();
        }
    }
}