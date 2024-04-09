using Menu.REST.DTO;
using Moq.Protected;
using Moq;
using System.Net;
using System.Text;
using System.Text.Json;
using Razor.Services;

namespace UnitTestServices
{
    public class CustomerTests
    {

        private CustomerDTO _customerValid;
        public CustomerTests()
        {
            _customerValid = new CustomerDTO
            {
                Id = Guid.NewGuid(),
                FirstName = "Yazan",
                LastName = "Tellawe",
                PhoneNumber = "1234567890",
                City = "Gent",
                BusNumber = "10",
                IsActive = true,
                Country = "Belgie",
                Email = "yazan@gmail.com",
                HouseNumber = "2",
                Password = "2123",
                Street = "overwale",
                ZipCode = "9000",
            };              
        }
        [Fact]
        public async Task GetAllCustomers_ReturnsCorrectCustomer()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(new List<CustomerDTO> { _customerValid }), Encoding.UTF8, "application/json")
                });

            var client = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost/") 
            };
            var customerService = new CustomerService(client);


            var customers = await customerService.GetAllCustomers();


            Assert.Single(customers);
            Assert.Equal(_customerValid.Id, customers[0].Id);
            Assert.Equal(_customerValid.FirstName, customers[0].FirstName);
            Assert.Equal(_customerValid.LastName, customers[0].LastName);
            Assert.Equal(_customerValid.PhoneNumber, customers[0].PhoneNumber);
            Assert.Equal(_customerValid.City, customers[0].City);
            Assert.Equal(_customerValid.BusNumber, customers[0].BusNumber);
            Assert.Equal(_customerValid.IsActive, customers[0].IsActive);
            Assert.Equal(_customerValid.Country, customers[0].Country);
            Assert.Equal(_customerValid.Email, customers[0].Email);
            Assert.Equal(_customerValid.HouseNumber, customers[0].HouseNumber);
            Assert.Equal(_customerValid.Password, customers[0].Password);
            Assert.Equal(_customerValid.Street, customers[0].Street);
            Assert.Equal(_customerValid.ZipCode, customers[0].ZipCode);

        }

        [Fact]
        public async Task CreateCustomer_SendsCorrectHttpRequest()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(message =>
                        message.Method == HttpMethod.Post
                        && message.RequestUri == new Uri("http://localhost/Customers") 
                        && message.Content.Headers.ContentType.ToString() == "application/json; charset=utf-8" 
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("", Encoding.UTF8, "application/json")
                });
            var client = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost/") // Dummy base 
            };

            var customerService = new CustomerService(client);
            var passwordBeforeHash = _customerValid.Password;


            await customerService.CreateCustomer(_customerValid);

            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), //  exactly once
                ItExpr.Is<HttpRequestMessage>(message =>
                    message.Method == HttpMethod.Post
                    && message.RequestUri == new Uri("http://localhost/Customers") 
                    && message.Content.Headers.ContentType.ToString() == "application/json; charset=utf-8"
                ),
            ItExpr.IsAny<CancellationToken>()
            );

            // Verify that the password was hashed
            Assert.NotEqual(passwordBeforeHash, _customerValid.Password);
            Assert.True(BCrypt.Net.BCrypt.Verify(passwordBeforeHash, _customerValid.Password));


        }

        [Fact]
        public async Task GetCustomerByName_SendsCorrectHttpRequest()
        {
            var firstName = "John";
            var lastName = "Doe";
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var customerJson = JsonSerializer.Serialize(_customerValid, jsonOptions);

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(message =>
                        message.Method == HttpMethod.Get
                        && message.RequestUri == new Uri($"http://localhost/Customers/ByName/{firstName}/{lastName}") // verify the URL
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(customerJson, Encoding.UTF8, "application/json")
                });
            var client = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost/") // Dummy base address for testing
            };

            var customerService = new CustomerService(client);
            var customer = await customerService.GetCustomerByName(firstName, lastName);

            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // verify that the method was called exactly once
                ItExpr.Is<HttpRequestMessage>(message =>
                     message.Method == HttpMethod.Get
                    && message.RequestUri == new Uri($"http://localhost/Customers/ByName/{firstName}/{lastName}") // verify the URL
                ),
                ItExpr.IsAny<CancellationToken>()
            );

            Assert.Equal(_customerValid.Id, customer.Id);
            Assert.Equal(_customerValid.FirstName, customer.FirstName);
            Assert.Equal(_customerValid.LastName, customer.LastName);

        }


        [Fact]
        public async Task DeleteCustomer_SendsCorrectHttpRequest()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(message =>
                        message.Method == HttpMethod.Delete
                        && message.RequestUri == new Uri($"http://localhost/Customers/{customerId}") // verify the URL
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent,
                });

            var client = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost/") // Dummy base address for testing
            };

            var customerService = new CustomerService(client);

            // Act
            await customerService.DeleteCustomer(customerId);

            // Assert
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // verify that the method was called exactly once
                ItExpr.Is<HttpRequestMessage>(message =>
                    message.Method == HttpMethod.Delete
                    && message.RequestUri == new Uri($"http://localhost/Customers/{customerId}") // verify the URL
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }


        [Fact]
        public async Task UpdateCustomer_SendsCorrectHttpRequest()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NoContent))  
                .Callback<HttpRequestMessage, CancellationToken>(async (request, cancellationToken) =>
                {
                    Assert.Equal(HttpMethod.Put, request.Method);
                    Assert.Equal($"https://localhost:7020/api/Customers/{_customerValid.Id}", request.RequestUri.ToString());

                    var updatedCustomerJson = await request.Content.ReadAsStringAsync();
                    var updatedCustomer = JsonSerializer.Deserialize<CustomerDTO>(updatedCustomerJson);
                    Assert.Equal(_customerValid.Id, updatedCustomer.Id);
                    Assert.Equal(_customerValid.FirstName, updatedCustomer.FirstName);

                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://localhost:7020/api/")
            };

            var customerService = new CustomerService(httpClient);

            // Act
            await customerService.UpdateCustomer(_customerValid);

        }



    }
}