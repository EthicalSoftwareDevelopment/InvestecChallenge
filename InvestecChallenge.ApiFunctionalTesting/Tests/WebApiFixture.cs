using FluentAssertions;
using InvestecChallenge.ApiFunctionalTesting.DomainModels;
using Newtonsoft.Json;

namespace InvestecChallenge.ApiFunctionalTesting.Tests
{
    /// <summary>
    /// Web Api tests for the Star Wars API
    /// </summary>
    [TestFixture]
    public class WebApiFixture
    {
        private const string RequestUri = "https://swapi.dev/api/people/";

        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Asserts that the WebApi data for R2-D2 should be correct
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Api_R2D2_ReturnsCorrectColour()
        {
            var client = new HttpClient();
            var response = client.GetAsync(RequestUri).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;

            var starWarsApiResponse = JsonConvert.DeserializeObject<StarWarsResponse>(responseString);
            var characters = starWarsApiResponse.Results;

            R2D2Assertions(characters);
        }

        /// <summary>
        /// Provides assertions for R2D2
        /// </summary>
        /// <param name="characters"></param>
        private static void R2D2Assertions(List<Character> characters)
        {
            characters.Should().NotBeEmpty().And.HaveCountGreaterThan(3);
            characters.Should().ContainSingle(x => x.Name == "R2-D2");

            var r2d2 = characters.First(x => x.Name == "R2-D2");
            r2d2.Skin_Color.Should().Be("white, blue");
        }
    }
}