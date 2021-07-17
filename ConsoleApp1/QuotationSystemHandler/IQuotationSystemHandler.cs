using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    // due to static methods and not needing to inject singleton class's, this interface can be used for unit testing
    public interface IQuotationSystemHandler
    {
        List<QuotationSystemResponse> GetResponses(RiskData request);
        QuotationSystemResponse GetPrice(RiskData request, string url, string port);
    }
}
