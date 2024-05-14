using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using EmergencyResponse.Pages;


namespace EmergencyResponseTest
{
    public class SearchAddressComponentTest
    {
        [Fact]
        public async void AddNumbers_ShouldCalculateCorrectly()
        {
            // Arrange
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<SearchAddressComponent>();

            // Act - simulate user input and button click
            var input1 = cut.Find("input[id='street']");
            input1.SetAttribute("value", "Fjellerupvej"); // Set first number
            input1.Change("Fjellerupvej"); // Simulate input change

            var input2 = cut.Find("input[id='houseNumber']");
            input2.SetAttribute("value", "7"); // Set second number
            input2.Change("7"); // Simulate input change

            var input3 = cut.Find("input[id='postalCode']");
            input3.SetAttribute("value", "5463"); // Set second number
            input3.Change("5463"); // Simulate input change

            var submitButton = cut.Find("button[id='submit']");
            submitButton.Click(); // Simulate button click

            //Awaiting because the submitButton.Click event calls an async method that won't finish before the test continues.
            await Task.Delay(1000);

            // Assert
            var componentInstance = cut.Instance;
            var possibleAddresses = componentInstance.possibleAddresses;

            possibleAddresses.Should().NotBeEmpty(); // Assert the final result
        }
    }
}
