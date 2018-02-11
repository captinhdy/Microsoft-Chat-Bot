using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace Chatbot
{
    public class ChatbotConnector
    {
        private const string secret = "DUwONkGQMu4.cwA.Bfk.wpYSHHqtStkjQcmA8wlvZX46h0JUG2X3-n6IMUQEO-8";
        private const string directUrl = "https://directline.botframework.com";
        public ChatbotConnector()
        {

        }

        public bool AuthorizeConnection()
        {
            HttpWebRequest request = HttpWebRequest.Create(directUrl + "/v3/directline/tokens/generate") as HttpWebRequest;
            request.Method = "POST";
            request.Accept = "application/json";
            request.ContentType = "application/json; charset=utf-8";
            request.Headers["Authorization"] = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(secret));

            request.get
            request.BeginGetResponse((r) =>
            {
            HttpWebResponse response = request.EndGetResponse(r) as HttpWebResponse;

                using (Stream s = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(s, Encoding.UTF8);
                    string responseString = reader.ReadToEnd();
                }
            }, null);

            return true;

        }
    }
}
