using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Assets.Editor
{
    internal static class CaptainProxy
    {
        private static readonly string CAPTAIN_ENDPOINT_URL = "http://localhost:3000/core";
        private static readonly HttpClient client = new HttpClient();

        public static async Task<CaptainMessage> InvokeCaptainAskAsync(string message)
        {
            var requestBody = new { content = message };
            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(CAPTAIN_ENDPOINT_URL, content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var captainMessage = JsonConvert.DeserializeObject<CaptainMessage>(responseBody);

            return captainMessage;
        }
    }

    public class CaptainMessage
    {
        public string Reasoning { get; set; }
        public string Content { get; set; }
    }
}

