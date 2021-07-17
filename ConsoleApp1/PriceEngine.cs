using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class PriceEngine
    {
        private readonly IQuotationSystemHandler _quotationSystemHandler;

        public PriceEngine(IQuotationSystemHandler quotationSystemHandler)
        {
            // constructor injecting QuotationSystemHandler interface, primarily for unit testing
            // if this was a real world solution we could inject our services at a higer level
            _quotationSystemHandler = quotationSystemHandler;
        }

        // pass request with risk data with details of a gadget, return the best price retrieved from 3 external quotation engines
        public decimal GetPrice(PriceRequest request, out decimal tax, out string insurerName/* argumented errors , out string errorMessage*/)
        {
            // validation - todo: possible validationhandler for reusable implementation, but I'm happy for this to stay as-is for now
            if (request.RiskData == null)
                throw (new ArgumentException("Risk Data is missing"));

            if (String.IsNullOrEmpty(request.RiskData.FirstName))
                throw (new ArgumentException("First name is required"));

            if (String.IsNullOrEmpty(request.RiskData.LastName))
                throw (new ArgumentException("Surname is require"));

            if (request.RiskData.Value == 0)
                throw (new ArgumentException("Value is required"));

            // at a glance, the main concern is the duplicate code used for what appears to be 
            //						multiple services expecting similar params, and similar results
            // Big 0: duplicating objects with what appears to be identical values for requests and responses

            // use of LINQ as responses aren't overly data heavy
            var result = _quotationSystemHandler.GetResponses(request.RiskData)
                .Where(r => r.IsSuccess)                    // only resolve successfull responses 
                .OrderBy(p => p.Price).FirstOrDefault();    // select lowest price

            // probably overkill
            if (result == null)
                throw (new ArgumentException("No results from the QuotationSystem"));

            // public QuotationSystemResponse GetPrice(...params) could be an alternative if further information is required from the service
            // but for now I have stuck with origianl priceEngine outs rather than return a complete object
            tax = result.Tax;           // trusting the responses never return null values on IsSuccess calls 
            insurerName = result.Name;  // trusting the responses never return null values on IsSuccess calls
                                        // todo: handle potential null value returns on IsSuccess true

            return result.Price;
        }

    }
}
