using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RestSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            //RunGetReleases();
            RunGetOnesourceModels();
        }

        private static void RunGetOnesourceModels()
        {
            const string url = @"https://onesource-cons.intel.com/FuseControllerService/api/Models";
            string userAgent =
                "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 58.0.3029.110 Safari / 537.36";
            string userAgentTwo =
                "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";

            JToken models;

            try
            {
                var client = new WebClient {UseDefaultCredentials = true};
                
                client.Headers.Add("user-agent", userAgent);
                var response = client.DownloadString(url);
                if (String.IsNullOrEmpty(response))
                {
                    throw new Exception("no content");
                }
                models = JArray.Parse(response);
                foreach (JToken el in models)
                {
                    Console.WriteLine(el.First);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("failed: " + ex.Message);
            }

            Console.Write("Hit <Enter> to continue...");
            Console.ReadLine();
        }

        private static void RunGetReleases()
        {
            const string url = @"https://api.github.com/repos/restsharp/restsharp/releases";
            string userAgent =
                "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 58.0.3029.110 Safari / 537.36";
            string userAgentTwo =
                "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";

            JToken releases;

            // HttpWebRequestResponse
            try
            {
                Console.WriteLine("UseHttpWebRequestResponse...");
                releases = UseHttpWebRequestResponse(url, userAgent);
                foreach (JToken el in releases)
                {
                    Console.WriteLine(el.First);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("failed: " + ex.Message);
            }
            Console.Write("Hit <Enter> to continue...");
            Console.ReadLine();

            // WebClient
            try
            {
                Console.WriteLine("UseWebClient...");
                releases = UseWebClient(url, userAgent);
                foreach (JToken el in releases)
                {
                    Console.WriteLine(el.First);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("failed: " + ex.Message);
            }
            Console.Write("Hit <Enter> to continue...");
            Console.ReadLine();

            // HttpClient
            try
            {
                Console.WriteLine("UseHttpClient...");
                releases = UseHttpClient(url, userAgentTwo);
                foreach (JToken el in releases)
                {
                    Console.WriteLine(el.First);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("failed: " + ex.Message);
            }
            Console.Write("Hit <Enter> to continue...");
            Console.ReadLine();

            // RestSharp
            try
            {
                Console.WriteLine("UseRestSharp...");
                releases = UseRestSharp(url, userAgentTwo);
                foreach (JToken el in releases)
                {
                    Console.WriteLine(el.First);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("failed: " + ex.Message);
            }
            Console.Write("Hit <Enter> to continue...");
            Console.ReadLine();
        }

        private static JToken UseRestSharp(string url, string userAgentTwo)
        {
            var client = new RestClient(url);
            IRestResponse response = client.Execute(new RestRequest());
            if (String.IsNullOrEmpty(response.Content))
            {
                throw new Exception("no content");
            }
            return JArray.Parse(response.Content);
        }

        private static JToken UseHttpClient(string url, string userAgent)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
                var response = httpClient.GetStringAsync(new Uri(url)).Result;
                if (String.IsNullOrEmpty(response))
                {
                    throw new Exception("no content");
                }
                return JArray.Parse(response);
            }
        }

        private static JToken UseWebClient(string url, string userAgent)
        {
            var client = new WebClient();
            client.Headers.Add("user-agent", userAgent);
            var response = client.DownloadString(url);
            if (String.IsNullOrEmpty(response))
            {
                throw new Exception("no content");
            }
            return JArray.Parse(response);
        }

        private static JToken UseHttpWebRequestResponse(string url, string userAgent)
        {
            HttpWebRequest request =
                WebRequest.Create(url) as HttpWebRequest;
            if (request == null)
            {
                throw new Exception("request is null");
            }
            request.Method = "GET";
            request.UserAgent = userAgent;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            if (response == null)
            {
                throw new Exception("response is null");
            }
            string content = String.Empty;
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    content = sr.ReadToEnd();
                }
            }
            if (String.IsNullOrEmpty(content))
            {
                throw new Exception("no content");
            }
            return JArray.Parse(content);
        }
    }
}
