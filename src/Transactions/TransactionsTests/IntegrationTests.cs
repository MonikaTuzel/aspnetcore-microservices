using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using TransactionsApi;
using TransactionsApi.Models;
using Xunit;

namespace TransactionsTests
{
    public class IntegrationTests : IClassFixture<TestFixture<Startup>>
    {

        private HttpClient Client;
        private string requestUrl = "/api/Transactions";

        public IntegrationTests(TestFixture<Startup> fixture)
        {
            Client = fixture.Client;
        }

        [Fact]
        public async Task TestGetAllAsync()
        {
            // Act
            var response = await Client.GetAsync(requestUrl);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TestGetAsync()
        {
            // Act
            var response = await Client.GetAsync(requestUrl + "/1");
            var responseString = response.Content.ReadAsStringAsync().Result;

            Transaction transaction = JsonConvert.DeserializeObject<Transaction>(responseString);

            double status = transaction.Price;

            Assert.Equal(500, status);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async Task TestPostItem()
        {
            // Arrange
            var requestBody = new
            {
                Url = requestUrl,
                Body = new Transaction
                {
                    idTransactions = 1004, // plus 1
                    User = "2391ec09-dd54-4203-9f5c-bedf69e263c6",
                    Car = 111,
                    Price = 200,
                    StartDate = new DateTime(2021, 1, 18),
                    EndDate = new DateTime(2021, 1, 28),
                    IsEnd = true,
                    IsReturned = true,
                }
            };

            // Act
            var response = await Client.PostAsync(requestBody.Url, ContentHelper.GetStringContent(requestBody.Body));
            //var value = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TestPutItemAsync()
        {
            var requestBody = new
            {
                Url = requestUrl + "/99",
                Body = new
                {
                    idTransactions = 99,
                    User = "testPut",
                    Car = 2,
                    Price = 123,
                    StartDate = new DateTime(2021, 3, 5),
                    EndDate = new DateTime(2021, 3, 12),
                    IsEnd = false,
                    IsReturned = false,
                }
            };

            // Act
            var response = await Client.PutAsync(requestBody.Url, ContentHelper.GetStringContent(requestBody.Body));

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TestDeleteItemAsync()
        {
            var response = await Client.DeleteAsync(requestUrl + "/1004");

            // Assert
            response.EnsureSuccessStatusCode();
            //USTAWIC SPRAWDZANIE
            //  Assert.False(singleResponse.Id);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }
    }
}
