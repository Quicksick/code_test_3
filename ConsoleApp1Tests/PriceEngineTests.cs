using ConsoleApp1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace ConsoleApp1Tests
{
				[TestClass]
				public class PriceEngineTests
				{
								[TestMethod]
								public void Validate_RiskData_Null_Throw()
								{
												var quotationSystemHandler = new Mock<IQuotationSystemHandler>();

												var priceEngine = new Mock<PriceEngine>(quotationSystemHandler.Object);

												var request = new PriceRequest()
												{
																RiskData = null
												};

												var exception = Assert.ThrowsException<ArgumentException>(() => priceEngine.Object.GetPrice(request, out decimal tax, out string insurerName));
												Assert.AreEqual("Risk Data is missing", exception.Message);
								}

								[TestMethod]
								public void Validate_RiskData_FirstName_Throw()
								{
												var quotationSystemHandler = new Mock<IQuotationSystemHandler>();

												var priceEngine = new Mock<PriceEngine>(quotationSystemHandler.Object);

												var request = new PriceRequest()
												{
																RiskData = new RiskData()
																{
																				DOB = DateTime.Parse("1980-01-01"),
																				FirstName = "",     // empty firstname
																				LastName = "Smith",
																				Make = "examplemake1",
																				Value = 500
																}
												};

												var exception = Assert.ThrowsException<ArgumentException>(() => priceEngine.Object.GetPrice(request, out decimal tax, out string insurerName));
												Assert.AreEqual("First name is required", exception.Message);
								}

								[TestMethod]
								public void Validate_RiskData_LastName_Throw()
								{
												var quotationSystemHandler = new Mock<IQuotationSystemHandler>();

												var priceEngine = new Mock<PriceEngine>(quotationSystemHandler.Object);

												var request = new PriceRequest()
												{
																RiskData = new RiskData()
																{
																				DOB = DateTime.Parse("1980-01-01"),
																				FirstName = "John",
																				LastName = "",     // empty lastname
																				Make = "examplemake1",
																				Value = 500
																}
												};

												var exception = Assert.ThrowsException<ArgumentException>(() => priceEngine.Object.GetPrice(request, out decimal tax, out string insurerName));
												Assert.AreEqual("Surname is require", exception.Message);
								}

								[TestMethod]
								public void Validate_RiskData_Value_Throw()
								{
												var quotationSystemHandler = new Mock<IQuotationSystemHandler>();

												var priceEngine = new Mock<PriceEngine>(quotationSystemHandler.Object);

												var request = new PriceRequest()
												{
																RiskData = new RiskData()
																{
																				DOB = DateTime.Parse("1980-01-01"),
																				FirstName = "John",
																				LastName = "Smith",
																				Make = "examplemake1"
																				// Value = 500 no value set - default = 0
																}
												};

												var exception = Assert.ThrowsException<ArgumentException>(() => priceEngine.Object.GetPrice(request, out decimal tax, out string insurerName));
												Assert.AreEqual("Value is required", exception.Message);
								}

								[TestMethod]
								public void Get_Lowest_Price_1()
								{
												var quotationSystemHandler = new Mock<IQuotationSystemHandler>();

												var priceEngine = new PriceEngine(quotationSystemHandler.Object);

												var request = new PriceRequest()
												{
																RiskData = new RiskData()
																{
																				DOB = DateTime.Parse("1980-01-01"),
																				FirstName = "John",
																				LastName = "Smith",
																				Make = "examplemake1",
																				Value = 500
																}
												};

												var responses = new List<QuotationSystemResponse>() {
																new QuotationSystemResponse()
																{
																				IsSuccess = true,
																				Name = "System 1",
																				Price = 10.5M // lowest price
																},
																new QuotationSystemResponse()
																{
																				IsSuccess = true,
																				Name = "System 2",
																				Price = 16.5M
																},
																new QuotationSystemResponse()
																{
																				IsSuccess = true,
																				Name = "System 3",
																				Price = 18.5M
																}
												};

												quotationSystemHandler.Setup(r => r.GetResponses(request.RiskData)).Returns(responses);

												decimal tax = 0;
												string insurerName = "";

												var expectedPrice = 10.5M;
												var expectedTax = 1.26M;
												var expectedInsurerName = "System 1";

												var priceEnginePrice = priceEngine.GetPrice(request, out tax, out insurerName);

												Assert.AreEqual(expectedPrice, priceEnginePrice);
												Assert.AreEqual(expectedTax, tax);
												Assert.AreEqual(expectedInsurerName, insurerName);
								}

								[TestMethod]
								public void Get_Lowest_Price_2()
								{
												var quotationSystemHandler = new Mock<IQuotationSystemHandler>();

												var priceEngine = new PriceEngine(quotationSystemHandler.Object);

												var request = new PriceRequest()
												{
																RiskData = new RiskData()
																{
																				DOB = DateTime.Parse("1980-01-01"),
																				FirstName = "John",
																				LastName = "Smith",
																				Make = "examplemake1",
																				Value = 500
																}
												};

												var responses = new List<QuotationSystemResponse>() {
																new QuotationSystemResponse()
																{
																				IsSuccess = true,
																				Name = "System 1",
																				Price = 10.5M
																},
																new QuotationSystemResponse()
																{
																				IsSuccess = true,
																				Name = "System 2",
																				Price = 16.5M
																},
																new QuotationSystemResponse()
																{
																				IsSuccess = true,
																				Name = "System 3",
																				Price = 9.5M // lowest price
																}
												};

												quotationSystemHandler.Setup(r => r.GetResponses(request.RiskData)).Returns(responses);

												decimal tax = 0;
												string insurerName = "";

												var expectedPrice = 9.5M;
												var expectedTax = 1.14M;
												var expectedInsurerName = "System 3";

												var priceEnginePrice = priceEngine.GetPrice(request, out tax, out insurerName);

												Assert.AreEqual(expectedPrice, priceEnginePrice);
												Assert.AreEqual(expectedTax, tax);
												Assert.AreEqual(expectedInsurerName, insurerName);
								}


								// further tests could be applied to test for the logic within our QuotationSystemHandler
								// ...

				}
}
