using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaunchDarkly.Client;

namespace HelloDotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Enter your LaunchDarkly SDK key here
            LdClient client = new LdClient("YOUR_SDK_KEY");
            User user = User.WithKey("bob@example.com")
              .AndFirstName("Bob")
              .AndLastName("Loblaw")
              .AndCustomAttribute("groups", "beta_testers");

            // TODO: Enter the key for your feature flag key here
            var value = client.BoolVariation("YOUR_FEATURE_FLAG_KEY", user, false);

            if (value)
            {
                Console.WriteLine("Showing feature for user " + user.Key);
            }
            else
            {
                Console.WriteLine("Not showing feature for user " + user.Key);
            }
            client.Flush();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
