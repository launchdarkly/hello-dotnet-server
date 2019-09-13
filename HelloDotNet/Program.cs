using System;

using LaunchDarkly.Client;
using Common.Logging;
using Common.Logging.Simple;

namespace HelloDotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            var props = new Common.Logging.Configuration.NameValueCollection
            {
                { "level", "Debug" }
            };
            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter(props);

            Configuration ldConfig = LaunchDarkly.Client.Configuration
                    // TODO: Enter your LaunchDarkly SDK key here
                    .Default("YOUR_SDK_KEY");

            LdClient client = new LdClient(ldConfig);
            User user = User.Builder("bob@example.com")
              .FirstName("Bob")
              .LastName("Loblaw")
              .Custom("groups", "beta_testers")
              .Build();

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
