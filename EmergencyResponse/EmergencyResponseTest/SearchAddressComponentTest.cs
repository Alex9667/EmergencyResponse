using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using EmergencyResponse.Pages;
using EmergencyResponse.ExternalServices.Interfaces;
using Moq;
using EmergencyResponse.DTO;


namespace EmergencyResponseTest
{
    public class SearchAddressComponentTest
    {
        [Fact]
        public async void SearchAddress_ShouldReturnListOfAddresses()
        {
            // Arrange
            using var ctx = new TestContext();
            var cut = ctx.RenderComponent<SearchAddressComponent>();

            // Act - simulate user input and button click
            var input1 = cut.Find("input[id='street']");
            input1.SetAttribute("value", "Fjellerupvej"); // Set first value
            input1.Change("Fjellerupvej"); // Simulate input change

            var input2 = cut.Find("input[id='houseNumber']");
            input2.SetAttribute("value", "7"); // Set second value
            input2.Change("7"); // Simulate input change

            var input3 = cut.Find("input[id='postalCode']");
            input3.SetAttribute("value", "5463"); // Set third value
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
