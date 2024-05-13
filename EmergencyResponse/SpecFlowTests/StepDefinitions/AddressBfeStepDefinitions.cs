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

        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        [Given("the street is (.*)")]
        public void GivenStreetIs(string street)
        {
            //TODO: implement arrange (precondition) logic
            // For storing and retrieving scenario-specific data see https://go.specflow.org/doc-sharingdata
            // To use the multiline text or the table argument of the scenario,
            // additional string/Table parameters can be defined on the step definition
            // method. 

            _address = street;
        }

        [Given("the house number is (.*)")]
        public void GivenTheHouseNumberIs(string houseNumber)
        {
            //TODO: implement arrange (precondition) logic

            _address += " " + houseNumber;
        }

        [Given("the postal code is (.*)")]
        public void GivenThePostalCodeIs(int postalCode)
        {
            //TODO: implement arrange (precondition) logic

            _address += " " + postalCode;
        }

        [When("the address is looked up")]
        public async void WhenTheAddressIsLookedUp()
        {
            //TODO: implement act (action) logic

            _bfe = Convert.ToInt32(_addressController.GetAddressBFE(_address));

            await Task.Delay(3000);
        }

        [Then("the result should be (.*)")]
        public void ThenTheResultShouldBe(int result)
        {
            //TODO: implement assert (verification) logic

            _bfe.Should().Be(result);

            Console.WriteLine();
        }
    }
}
