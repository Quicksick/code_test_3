using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class QuotationSystemHandler : IQuotationSystemHandler
    {
        // assume this will not be an injectable singleton class for this example static methods should be fine
        public QuotationSystemHandler() { }

        public static List<QuotationSystemResponse> GetResponses(RiskData request)
        {
            var responses = new List<QuotationSystemResponse>();

            // applied single responsibility principle from the price engine by creating a quotationSystemHandler
            // simplified quotation service calls
            // todo: units tests for get responses logic

            //system 1 requires DOB to be specified
            if (request.DOB != null)
                responses.Add(GetPrice(request, "http://quote-system-1.com", "1234"));

            //system 2 only quotes for some makes
            if (request.Make == "examplemake1" || request.Make == "examplemake2" || request.Make == "examplemake3")
                responses.Add(GetPrice(request, "http://quote-system-2.com", "1235"));

            //system 3 is always called
            responses.Add(GetPrice(request, "http://quote-system-3.com", "100"));

            //system 4, 5, 6...

            // argumented exception example
            if (responses.Where(r => r.IsSuccess).Count() == 0)
                throw (new ArgumentException("No successful responeses from the QuotationSystem"));

            return responses;
        }

        public static QuotationSystemResponse GetPrice(RiskData request, string url, string port)
        {
            //makes a call to an external service - SNIP
            //var response = _someExternalService.PostHttpRequest(requestData);

            // if RiskData contains different params for each service we could make use of generics to pass in for each service call
            // but for now this example indicates the request model is the same for each QuotationSystem service

            // could be async to prevent thread blocking calls to our external service

            // crude fake back end response depending on url request
            // stuck with original example values
            string[] urlArray = new string[] { "http://quote-system-1.com", "http://quote-system-2.com", "http://quote-system-3.com" };

            var response = new QuotationSystemResponse()
            {
                Price = url == urlArray[0] ? 123.45M : url == urlArray[1] ? 234.56M : url == urlArray[2] ? 92.67M : 0,
                IsSuccess = Array.Exists(urlArray, uri => uri == url) ? true : false,
                Name = url == urlArray[0] ? "Test Name" : url == urlArray[1] ? "qewtrywrh" : url == urlArray[2] ? "zxcvbnm" : null,
            };

            return response;
        }


        // interface static calls to be used when injecting and mocking outputs for testing
        List<QuotationSystemResponse> IQuotationSystemHandler.GetResponses(RiskData request)
        {
            return GetResponses(request);
        }

        QuotationSystemResponse IQuotationSystemHandler.GetPrice(RiskData request, string url, string port)
        {
            return GetPrice(request, url, port);
        }
    }
}
