using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestecChallenge.ApiFunctionalTesting.DomainModels
{
    public class StarWarsResponse
    {
        public int Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public List<Character> Results { get; set; }
    }
}
