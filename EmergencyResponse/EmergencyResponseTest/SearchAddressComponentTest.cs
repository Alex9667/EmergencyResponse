using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using Bunit;
using EmergencyResponse.DTO;
using EmergencyResponse.ExternalServices.Interfaces;
using EmergencyResponse.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;


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

        [Theory]
        [MemberData(nameof(GetAddresses))]
        public void SelectAddressFromList_ShouldSetSelectedAddress(List<AddressDTO> addresses)
        {
            using var ctx = new TestContext();

            var mockDataFordelerService = new Mock<IDatafordelerenService>();
            var mockDataForsyningService = new Mock<IDataforsyningService>();

            ctx.Services.AddSingleton<IDatafordelerenService>(mockDataFordelerService.Object);
            ctx.Services.AddSingleton<IDataforsyningService>(mockDataForsyningService.Object);

            var component = ctx.RenderComponent<SearchAddressComponent>();

            component.Instance.possibleAddresses.AddRange(addresses);
            component.Render();
            var address = addresses[2];

            var row = component.Find($"tr:has(td:nth-child(2):contains('{address.StreetName}'))" +
                                    $":has(td:nth-child(3):contains('{address.HouseNumber}'))" +
                                    $":has(td:nth-child(4):contains('{address.Floor}'))" +
                                    $":has(td:nth-child(5):contains('{address.Door}'))" +
                                    $":has(td:nth-child(6):contains('{address.PostalCode}'))" +
                                    $":has(td:nth-child(7):contains('{address.PostalCodeName}'))");

            row.Click();
            var selectedAddress = component.Instance.selectedAddress;
            selectedAddress.Should().Be(addresses[2]);
        }

        public static IEnumerable<Object[]> GetAddresses()
        {
            return new List<object[]> {
                new object[] {
                    new List<AddressDTO> {
                        new AddressDTO("Æblevej", "15", "1", "a", "5000", "odense C", "nnkdnvkdn"),
                        new AddressDTO("Æblevej", "15", "1", "c", "5000", "odense C", "joidsdlksjls"),
                        new AddressDTO("Æblevej", "15", "2", "a", "5000", "odense C", "eksknfkskf"),
                        new AddressDTO("Æblevej", "15", "2", "b", "5000", "odense C", "skjekskjf"),
                        new AddressDTO("Æblevej", "15", "2", "c", "5000", "odense C", "ldgjrigdl")
                    },
                }
            };
        }
    }
}
