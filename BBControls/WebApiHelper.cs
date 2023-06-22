using BBControls.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BBControls.WebApiHelpers
{
    public class WebApiHelper
    {
        public async Task<List<Annotation>?> GetAnnosForIdAndUser(string id, string user)
        {
            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7099/");
            client.DefaultRequestHeaders.Add("User-Agent", "WebAPIClient");
            HttpResponseMessage response = await client.GetAsync($"/api/Annotations/user/{user}/imdbid/{id}");

            // Check if the response is successful
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                string content = await response.Content.ReadAsStringAsync();
                List<Annotation> annotations = JsonSerializer.Deserialize<List<Annotation>>(content);
                return annotations;
            }
            else
            {
                return null;
            }
        }

        public async Task WriteAnnotation(Annotation annotation)
        {
            var json = JsonSerializer.Serialize(annotation);
            using HttpClient client = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7099/api/Annotations", content);

            var responseString = await response.Content.ReadAsStringAsync();
        }
        public async Task UpdateAnnotation(Annotation annotation)
        {
            var json = JsonSerializer.Serialize(annotation);
            using HttpClient client = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"https://localhost:7099/api/Annotations/{annotation.Id}", content);

            var responseString = await response.Content.ReadAsStringAsync();
        }


        public async Task DeleteAnnotation(int id)
        {
            using HttpClient client = new HttpClient();
            var response = await client.DeleteAsync($"https://localhost:7099/api/Annotations/{id}");
            var responseString = await response.Content.ReadAsStringAsync();
        }

    }
}
