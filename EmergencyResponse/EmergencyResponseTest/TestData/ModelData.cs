using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmergencyResponse.Model;

namespace EmergencyResponseTest.TestData
{
    public class ModelData : IEnumerable<object[]>
    {
        private IEnumerable<object[]> data => new[]
        {
            new object[] 
            {
                new Address("Nygade", "74", null, null, "7430", "Ikast")
            },
            new object[]
            {
                new Address("Fjellerupvej", "7", null, null, "5463", "Harndrup")
            }
        };

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            return data.GetEnumerator();
        }
    }
}
