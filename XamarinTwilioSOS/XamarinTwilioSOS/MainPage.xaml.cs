using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace XamarinTwilioSOS
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string locationinfo = "";
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                CancellationTokenSource cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    locationinfo = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
                }
                else
                {
                    locationinfo = "Location Not Found";
                }
            }
            catch (Exception ex)
            {
                // Unable to get location
                locationinfo = "Location Not Found";
            }
            Message(locationinfo);
            Call();
        }

        private static void Message(string locationinfo)
        {
            string accountSid = Secrets.TWILIO_ACCOUNTSID;
            string authToken = Secrets.TWILIO_AUTHTOKEN;

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: $"I am in trouble\nNeed Help\nPlease call me right now or visit me at the following location:\n{locationinfo}",
                from: new PhoneNumber(Secrets.FROM_NUMBER),
                to: new PhoneNumber(Secrets.TO_NUMBER)
            ); ;

            //Console.WriteLine(message.Sid);
        }

        private static void Call()
        {
            string accountSid = Secrets.TWILIO_ACCOUNTSID;
            string authToken = Secrets.TWILIO_AUTHTOKEN;

            TwilioClient.Init(accountSid, authToken);

            var to = new PhoneNumber(Secrets.TO_NUMBER);
            var from = new PhoneNumber(Secrets.FROM_NUMBER);
            var call = CallResource.Create(to, from,
                twiml: new Twiml("<Response><Say>I am in trouble. Need help from you. Please check your message or call me</Say></Response>"));

            //Console.WriteLine(call.Sid);
        }
    }
}
