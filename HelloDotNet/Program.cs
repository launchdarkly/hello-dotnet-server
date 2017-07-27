using System;

using LaunchDarkly.Client;
using Microsoft.Extensions.Logging;

namespace HelloDotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            ILoggerFactory factory = new LoggerFactory()
              .AddConsole(LogLevel.Debug)
              .AddDebug();

            Configuration ldConfig = LaunchDarkly.Client.Configuration
                    // TODO: Enter your LaunchDarkly SDK key here
                    .Default("YOUR_SDK_KEY")
                    .WithLoggerFactory(factory);

            LdClient client = new LdClient(ldConfig);
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
