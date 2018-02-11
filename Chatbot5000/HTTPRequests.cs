using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Text;
using System.Net.Http.Headers;

namespace Chatbot5000
{
    public class HTTPRequests
    {
        string endpoint;
        public HTTPRequests(string EndPoint)
        {
            
        }
        public async Task<HttpResponseMessage> PostRequest(string PostData)
        {
            var client = new HttpClient();
            
            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "557e82b840ed4d0e9ed6beb36318a946");
            
            
            var uri = "https://southcentralus.api.cognitive.microsoft.com/text/analytics/v2.0/keyPhrases";

            HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes(PostData);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                
                response = await client.PostAsync(uri, content);

            }

            return response;
        }

            public async Task<HttpResponseMessage> GetRequest()
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "557e82b840ed4d0e9ed6beb36318a946");

            HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes("{body}");

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(endpoint, content);

            }

            return response;
        }
    }
}