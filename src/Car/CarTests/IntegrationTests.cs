using CarApi;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using CarApi.Models;
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CarTesty
{
    public class IntegrationTests : IClassFixture<TestFixture<Startup>>
    {

        private HttpClient Client;
        private string requestUrl = "/api/Car";

        public IntegrationTests(TestFixture<Startup> fixture)
        {
            Client = fixture.Client;
        }

        [Fact]
        public async Task TestGetAllAsync()
        {
            // Act
            var response = await Client.GetAsync(requestUrl);
            string jsonString = response.Content.ReadAsStringAsync().Result;
            var act = JsonConvert.DeserializeObject<List<Car>>(jsonString);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(act.Count > 0);
        }

        [Fact]
        public async Task TestGetAsync()
        {
            // Act
            var response = await Client.GetAsync(requestUrl + "/1");

            string jsonString = response.Content.ReadAsStringAsync().Result;
            var act = JsonConvert.DeserializeObject<Car>(jsonString);

            // Assert
            Assert.Equal(1, act.idCar);
            Assert.Equal("Astra", act.Model);
            Assert.Equal("Niebieski", act.Color);
            Assert.Equal(1, act.IsAvailable);
            Assert.Equal(new DateTime(2021, 1, 20), act.TechRev);

        }

        [Fact]
        public async Task TestPostItem()
        {
            // Dodanie nowego wiersza
            DateTime year = new DateTime(1111, 1, 1);
            var requestBody = new
            {
                Url = requestUrl,
                Body = new Car
                {
                     idCar=15,
                     Manufacturer="test",
                     Model="tets",
                     Color="test",
                     YofProd=1111,
                     Kilometers=12345,
                     PriceDay=135,
                     IsAvailable=1,
                     Insurance= year,
                     Segment=1,
                     RegNumbers="wwwww",
                     FilePath="/test",
                     TechRev= year
                }
            };

            // Act
            var response = await Client.PostAsync(requestBody.Url, ContentHelper.GetStringContent(requestBody.Body));
            var response2 = await Client.GetAsync(requestUrl + "/15");
            string jsonString = response2.Content.ReadAsStringAsync().Result;
            var act = JsonConvert.DeserializeObject<Car>(jsonString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal(requestBody.Body.idCar, act.idCar);
            Assert.Equal(requestBody.Body.Model, act.Model);
            Assert.Equal(requestBody.Body.Color, act.Color);
            Assert.Equal(requestBody.Body.IsAvailable, act.IsAvailable);
            Assert.Equal(requestBody.Body.TechRev, act.TechRev);
            Assert.Equal(requestBody.Body.Manufacturer, act.Manufacturer);
        }

        [Fact]
        public async Task TestPutItemAsync()
        {
            // Update nowego wiersza
            DateTime year = new DateTime(2000, 1, 1);
            var requestBody = new
            {
                Url = requestUrl + "/15",
                Body = new
                {
                    idCar = 15,
                    Manufacturer = "update",
                    Model = "update",
                    Color = "update",
                    YofProd = 2000,
                    Kilometers = 15000,
                    PriceDay = 20000,
                    IsAvailable = 0,
                    Insurance = year,
                    Segment = 1,
                    RegNumbers = "aaa",
                    FilePath = "/test2",
                    TechRev = year
                }
            };

            // Act
            var response = await Client.PutAsync(requestBody.Url, ContentHelper.GetStringContent(requestBody.Body));

            var response2 = await Client.GetAsync(requestUrl + "/15");
            string jsonString = response2.Content.ReadAsStringAsync().Result;
            var act = JsonConvert.DeserializeObject<Car>(jsonString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal(requestBody.Body.idCar, act.idCar);
            Assert.Equal(requestBody.Body.Model, act.Model);
            Assert.Equal(requestBody.Body.Color, act.Color);
            Assert.Equal(requestBody.Body.IsAvailable, act.IsAvailable);
            Assert.Equal(requestBody.Body.TechRev, act.TechRev);
            Assert.Equal(requestBody.Body.Manufacturer, act.Manufacturer);
        }

        [Fact]
        public async Task TestDeleteItemAsync()
        {
            var response = await Client.DeleteAsync(requestUrl + "/15");

            // Assert
            response.EnsureSuccessStatusCode();
            var response2 = await Client.GetAsync(requestUrl + "/15");
            //USTAWIC SPRAWDZANIE
            //  Assert.False(singleResponse.Id);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response2.IsSuccessStatusCode);

        }
        [Fact]
        public async Task Return_404_Result()
        {
            // Act
            var response = await Client.GetAsync(String.Empty);

            // Assert
            //response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task Return_400_Result()
        {
            // Act
            var response = await Client.DeleteAsync(requestUrl + "/{id_url}");

            // Assert
            //response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task Return_500_Result()
        {
            // Act
            var response = await Client.DeleteAsync(requestUrl + "/0");

            // Assert
            //response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
