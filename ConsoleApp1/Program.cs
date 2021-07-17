using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //SNIP - collect input (risk data from the user)
            try
            {
                // could be injected at a higher level possibly ninject or similar
                IQuotationSystemHandler quotationSystemHandler = new QuotationSystemHandler();

                var request = new PriceRequest()
                {
                    RiskData = new RiskData() //hardcoded here, but would normally be from user input above
                    {
                        DOB = DateTime.Parse("1980-01-01"),
                        FirstName = "John",
                        LastName = "Smith",
                        Make = "examplemake1",
                        Value = 500
                    }
                };

                decimal tax = 0;
                string insurer = "";
                // string error = "";

                var priceEngine = new PriceEngine(quotationSystemHandler);
                var price = priceEngine.GetPrice(request, out tax, out insurer);

                Console.WriteLine(String.Format("You price is {0}, from insurer: {1}. This includes tax of {2}", price, insurer, tax));
            }
            catch (ArgumentException ex)
            {
                // argumented exceptions for deeper error message handling
                Console.WriteLine(String.Format("There was an error - {0}", ex.Message));
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

    }
}
