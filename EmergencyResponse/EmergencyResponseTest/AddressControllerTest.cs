﻿using EmergencyResponse.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmergencyResponse.Controller;
using Moq;
using EmergencyResponse.Interfaces;
using Xunit;
using System.Net;
using System.Runtime.InteropServices;
using Moq.Protected;
using System.Text.Json;
using System.Net.Http;
using EmergencyResponse.DTO;

namespace EmergencyResponseTest
{
    public class AddressControllerTest
    {
        private AddressController _addressController;
        public AddressControllerTest()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var httpClient = new HttpClient(/*mockHttpMessageHandler.Object*/);

            _addressController = new AddressController(httpClient);
        }

        [Theory]
        [InlineData("Nygade 74 7430")]
        [InlineData("Fjellerupvej 7 5463")]
        public async void AddressController_GetAddressBFE_ShouldReturnString(string address)
        {
            int bfe = await _addressController.GetAddressBFE(address);
            bfe.ToString().Should().NotBeNullOrEmpty();
        }
    }
}
