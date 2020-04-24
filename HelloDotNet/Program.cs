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
            if (Parameters.SdkKey == "")
            {
                Console.WriteLine("Please edit Parameters.cs to set SdkKey to your LaunchDarkly SDK key first");
                Environment.Exit(1);
            }

            var props = new Common.Logging.Configuration.NameValueCollection
            {
                { "level", "Warn" }
            };
            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter(props);

            Configuration ldConfig = LaunchDarkly.Client.Configuration
                .Default(Parameters.SdkKey);

            LdClient client = new LdClient(ldConfig);

            // Set up the user properties. This user should appear on your LaunchDarkly users dashboard
            // soon after you run the demo.
            User user = User.Builder("bob@example.com")
                .FirstName("Bob")
                .LastName("Loblaw")
                .Custom("groups", LdValue.ArrayOf(LdValue.Of("beta_testers")))
                .Build();

            var value = client.BoolVariation(Parameters.FeatureFlagKey, user, false);

            Console.WriteLine(string.Format("Feature flag '{0}' is {1} for this user",
                Parameters.FeatureFlagKey, value));

            // Calling client.Dispose() ensures that the SDK shuts down cleanly before the program exits.
            // Unless you do this, the SDK may not have a chance to deliver analytics events to LaunchDarkly,
            // so the user properties and the flag usage statistics may not appear on your dashboard. In a
            // normal long-running application, events would be delivered automatically in the background
            // and you would not need to close the client.
            client.Dispose();
        }
    }
}
