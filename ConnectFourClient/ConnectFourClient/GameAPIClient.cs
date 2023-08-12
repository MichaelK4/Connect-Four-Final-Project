using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;

namespace ConnectFourClient
{
    public class GameAPIClient
    {
        private readonly HttpClient httpClient;


        public GameAPIClient()
        {
            var httpClientHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            httpClient = new HttpClient(httpClientHandler);
            httpClient.BaseAddress = new Uri("https://localhost:7083/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<int> GetRandomColumn()
        {

            string requestUri = "api/Users/getrandomcolumn";
                HttpResponseMessage response = await httpClient.GetAsync(requestUri);
                if(response.IsSuccessStatusCode)
                {
                    int randCol = await response.Content.ReadAsAsync<int>();
                    return randCol;
                }
                else
                {
                    MessageBox.Show($"Failed with status code: {response.StatusCode}");
                    return -1;
                }
        }



        


        public async Task<bool> SaveGame(Game game)
        {
            try
            {
                HttpResponseMessage resp = await httpClient.PostAsJsonAsync("api/Users/SaveGame", game);

                resp.EnsureSuccessStatusCode();

                return await resp.Content.ReadAsAsync<bool>(); 
            }
            catch(Exception e)
            {
                MessageBox.Show("Failed to save game: " + e.Message);
                return false;
            }
        }

        
        public async Task<bool> CheckUsername(string username)
        {
            try
            {
                //HttpResponseMessage resp = await httpClient.PostAsync("api/Users/CheckUsername", new StringContent(username, Encoding.UTF8, "application/json"));
                HttpResponseMessage resp = await httpClient.PostAsJsonAsync("api/Users/CheckUsername", username);
                resp.EnsureSuccessStatusCode();

                return await resp.Content.ReadAsAsync<bool>();
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to check username: " + e.Message);
                return false;
            }
        }

    }
}
