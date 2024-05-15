using EmergencyResponse.Controller;
using Moq;


namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public sealed class AddressBfeStepDifinitions
    {
        private string? _address;
        private int? _bfe;
        private AddressController _addressController;
        public AddressBfeStepDifinitions()
        {
            //var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var httpClient = new HttpClient();

            _addressController = new AddressController(httpClient);
        }

        [Given("the street is (.*)")]
        public void GivenStreetIs(string street)
        {
            //arrange (precondition)

            _address = street;
        }

        [Given("the house number is (.*)")]
        public void GivenTheHouseNumberIs(string houseNumber)
        {
            //arrange (precondition)

            _address += " " + houseNumber;
        }

        [Given("the postal code is (.*)")]
        public void GivenThePostalCodeIs(int postalCode)
        {
            //arrange (precondition)

            _address += " " + postalCode;
        }

        [When("the address is looked up")]
        public async Task WhenTheAddressIsLookedUp()
        {
            //act (action)
            _bfe = Convert.ToInt32(await _addressController.GetAddressBFE(_address));
        }

        [Then("the result should be (.*)")]
        public async Task ThenTheResultShouldBe(int result)
        {
            //assert (verification) 
            await WhenTheAddressIsLookedUp();
            _bfe.Should().Be(result);
        }
    }
}
