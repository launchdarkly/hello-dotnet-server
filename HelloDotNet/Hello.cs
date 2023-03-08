using System;
using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Server;
using LaunchDarkly.Sdk.Server.Integrations;

namespace HelloDotNet
{
    class Hello
    {
        // Set SdkKey to your LaunchDarkly SDK key.
        public const string SdkKey = "your sdk key";

        // Set FeatureFlagKey to the feature flag key you want to evaluate.
        public const string FeatureFlagKey = "int-flag";

        private static void ShowMessage(string s) {
            Console.WriteLine("*** " + s);
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            if (string.IsNullOrEmpty(SdkKey))
            {
                ShowMessage("Please edit Hello.cs to set SdkKey to your LaunchDarkly SDK key first");
                Environment.Exit(1);
            }

            var ldConfig = Configuration.Builder(SdkKey)
            .BigSegments(
                Components.BigSegments(
                    Redis.BigSegmentStore()
                    .RedisConfiguration(new StackExchange.Redis.ConfigurationOptions{
                        AbortOnConnectFail = false
                    })
                    .HostAndPort("localhost", 6379)
                    .Prefix("pre"))
            )
            .Build();

            var client = new LdClient(ldConfig);

            if (client.Initialized)
            {
                ShowMessage("SDK successfully initialized!");
            }
            else
            {
                ShowMessage("SDK failed to initialize");
                Environment.Exit(1);
            }

            // Set up the evaluation context. This context should appear on your LaunchDarkly contexts
            // dashboard soon after you run the demo.
            var context = Context.Builder("toast")
                .Name("Toaster")
                .Build();

            var flagValue = client.IntVariation(FeatureFlagKey, context, 777);

            ShowMessage(string.Format("Feature flag '{0}' is {1} for this context",
                FeatureFlagKey, flagValue));

            // Here we ensure that the SDK shuts down cleanly and has a chance to deliver analytics
            // events to LaunchDarkly before the program exits. If analytics events are not delivered,
            // the context attributes and flag usage statistics will not appear on your dashboard. In
            // a normal long-running application, the SDK would continue running and events would be
            // delivered automatically in the background.
            client.Dispose();
        }
    }
}
