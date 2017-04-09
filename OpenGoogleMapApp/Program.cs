using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQCore;
using RabbitMQCore.Configuration;

namespace OpenGoogleMapApp
{
    class Program
    {
        static void Main(string[] args)
        {

            IRabbitMQClient client = new RabbitMQClient(new UriRabbitMqConfiguration("GPSLocation",
                "amqp://kkxglvxw:WhGhDnh9_wSe-Jf_TTjwevZkG9A9bZhv@clam.rmq.cloudamqp.com/kkxglvxw"));
            client.ReceiveMessages(
                x =>
                {
                    Console.WriteLine(x);
                    var location = ParseLocation(x.Split(' '));
                    if (location != null)
                        Process.Start("chrome.exe", string.Format(@"https://www.google.com.do/maps/@{0},{1}", location.Latitude, location.Longitude));

                   // Console.ReadKey();

                });
            Console.ReadKey();
        }


        public static Location ParseLocation(string[] args)
        {
            if (args == null || args.Length != 2)
                return null;
            return new Location(args[1], args[0]);
        }

        public class Location
        {
            public Location(string longitude, string latitude)
            {
                Longitude = longitude;
                Latitude = latitude;
            }

            public string Latitude { get; private set; }
            public string Longitude { get; private set; }
        }
    }
}
