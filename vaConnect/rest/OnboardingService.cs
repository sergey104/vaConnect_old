using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using NativeWifi;
namespace vaConnect
{
    class OnboardingService
    {
        private static OnboardingService instance;

        private OnboardingService()
        { }

        public static OnboardingService getInstance()
        {
            if (instance == null)
            {
                instance = new OnboardingService();
            }
            return instance;
        }
        public async void getWiFiProfile(String token, String identifier, WiFiProfile z)
        {
            Uri baseUri = new Uri("https://vmnac-int.fon.com/onboarding/windows/result/");
            String add = "?token=" + token + "&identifier=" + identifier;
            Uri myUri = new Uri(baseUri, add);
            // Create a request for the URL. 
            WebRequest request = WebRequest.Create(
              myUri);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            WebResponse response = await request.GetResponseAsync();
            // Display the status.

            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            String responseFromServer = reader.ReadToEnd();
           

            z = JsonConvert.DeserializeObject<WiFiProfile>(responseFromServer);

            // Clean up the streams and the response.
            //  reader
            //   response.Close();
        }
    }
}
