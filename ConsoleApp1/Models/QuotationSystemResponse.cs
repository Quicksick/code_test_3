using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    // could be applied along side a generic<T> class if responses from each system differ
    public class QuotationSystemResponse
    {
        public decimal Price { get; set; }
        public bool IsSuccess { get; set; }
        public string Name { get; set; }
        public decimal Tax => Price * 0.12M; // readonly value
    }
}
