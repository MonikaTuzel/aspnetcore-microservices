using SpendingsApi;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using SpendingsApi.Models;
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;




namespace SpendingsTests
{
    public class IntegrationTests : IClassFixture<TestFixture<Startup>>
    {

        private HttpClient Client;
        private string requestUrl = "/api/Spendings";

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
            var act = JsonConvert.DeserializeObject<List<Spendings>>(jsonString);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(act.Count > 0);
        }
        [Fact]
        public async Task TestGetAsync()
        {
            // Act
            var response = await Client.GetAsync(requestUrl + "/GetSpendings/2391ec09-dd54-4203-9f5c-bedf69e263c6");

            string jsonString = response.Content.ReadAsStringAsync().Result;
            var act = JsonConvert.DeserializeObject<List<Spendings>>(jsonString);




            // Assert
            Assert.Equal(1, act[0].idSpendings);
            Assert.Equal(new DateTime(2020, 10, 12), act[0].Date);
            Assert.Equal(2, act[0].CarID);
            Assert.Equal(2, act[0].CostID);
            Assert.Equal(912, act[0].Price);
            
         
        }
        [Fact]
        public async Task TestPostItem()
        {

            // Dodanie nowego wiersza
            var requestBody = new
            {

                Url = requestUrl,
                Body = new Spendings
                {
                    idSpendings = 0,
                    Date = new DateTime(2020, 10, 12),
                    CarID = 4,
                    CostID = 5,
                    Price = 4523,
                    idUser = "2391ec09-dd54-4203-9f5c-bedf69e263c6"
                }
            };

            // Act
            var response = await Client.PostAsync(requestBody.Url+ "/AddSpending/", ContentHelper.GetStringContent(requestBody.Body));

            var response2 = await Client.GetAsync(requestUrl + "/GetSpendings/2391ec09-dd54-4203-9f5c-bedf69e263c6");
            string jsonString = response2.Content.ReadAsStringAsync().Result;
            var act = JsonConvert.DeserializeObject<List<Spendings>>(jsonString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var count = act.Count - 1;

            Assert.Equal(requestBody.Body.Price, act[count].Price);
            Assert.Equal(requestBody.Body.CarID, act[count].CarID);
            Assert.Equal(requestBody.Body.CostID, act[count].CostID);
            Assert.Equal(requestBody.Body.Date, act[count].Date);

        }
        [Fact]
        public async Task TestPutItemAsync()
        {
            var response2 = await Client.GetAsync(requestUrl + "/GetSpendings/2391ec09-dd54-4203-9f5c-bedf69e263c6");
            string jsonString = response2.Content.ReadAsStringAsync().Result;
            var act = JsonConvert.DeserializeObject<List<Spendings>>(jsonString);
            var count = act.Count - 1;

            var requestBody = new
            {
                Url = requestUrl + "/"+ act[count].idSpendings,
                Body = new 
                {
                    idSpendings = act[count].idSpendings,
                    Date = new DateTime(2015, 5, 5),
                    CarID = 4,
                    CostID = 5,
                    Price = 6655,
                    idUser = "2391ec09-dd54-4203-9f5c-bedf69e263c6"

                }
            };

            // Act
            var response = await Client.PutAsync(requestBody.Url, ContentHelper.GetStringContent(requestBody.Body));

            var response3 = await Client.GetAsync(requestUrl + "/GetSpendings/2391ec09-dd54-4203-9f5c-bedf69e263c6");
            string jsonString2 = response3.Content.ReadAsStringAsync().Result;
            var act2 = JsonConvert.DeserializeObject<List<Spendings>>(jsonString2);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal(requestBody.Body.idSpendings, act2[count].idSpendings);
            Assert.Equal(requestBody.Body.Price, act2[count].Price);
            Assert.Equal(requestBody.Body.CarID, act2[count].CarID);
            Assert.Equal(requestBody.Body.CostID, act2[count].CostID);
            Assert.Equal(requestBody.Body.Date, act2[count].Date);
        }

        [Fact]
        public async Task TestDeleteItemAsync()
        {
            var response = await Client.DeleteAsync(requestUrl + "/108");

            // Assert
            response.EnsureSuccessStatusCode();
            var response2 = await Client.GetAsync(requestUrl + "/108");
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
