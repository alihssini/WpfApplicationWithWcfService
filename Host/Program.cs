using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using WCFService;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(NumberToCurrencyWordsService)))
            {
                // adding behaviour from code instead of defining in config file.  
                ServiceMetadataBehavior behavior = new ServiceMetadataBehavior { HttpGetEnabled = true };
                host.Description.Behaviors.Add(behavior);

                // adding end point from code.  
                BasicHttpBinding binding = new BasicHttpBinding();
                host.AddServiceEndpoint(typeof(INumberToCurrencyWordsService), binding, nameof(NumberToCurrencyWordsService));

                host.Open();//for exception "could not register URL..." : write this command on cmd-> netsh http add urlacl url=http://+:8733/ user=DOMAIN\user
                Console.WriteLine("NumberToCurrencyWordsService hosted");
                Console.WriteLine("Press any key to stop service");
                Console.ReadKey();
            }
        }
    }
}
