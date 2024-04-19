using System;
using System.Threading.Tasks;
using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Server;

namespace HelloDotNet
{
    class Hello
    {
        public static bool CI = Environment.GetEnvironmentVariable("CI") != null;

        // Set SdkKey to your LaunchDarkly SDK key.
        public static string SdkKey = Environment.GetEnvironmentVariable("LAUNCHDARKLY_SERVER_KEY");

        // Set FeatureFlagKey to the feature flag key you want to evaluate.
        public static string FeatureFlagKey = "sample-feature";

        public static void ShowBanner(){
            Console.WriteLine(
                "\n        ██       \n" +
                "          ██     \n" +
                "      ████████   \n" +
                "         ███████ \n" +
                "██ LAUNCHDARKLY █\n" +
                "         ███████ \n" +
                "      ████████   \n" +
                "          ██     \n" +
                "        ██       \n");
        }

        static void Main(string[] args)
        {
            if (string.IsNullOrEmpty(SdkKey))
            {
                Console.WriteLine("*** Please edit Hello.cs to set SdkKey to your LaunchDarkly SDK key first\n");
                Environment.Exit(1);
            }

            var ldConfig = Configuration.Default(SdkKey);

            var client = new LdClient(ldConfig);

            if (client.Initialized)
            {
                Console.WriteLine("*** SDK successfully initialized!\n");
            }
            else
            {
                Console.WriteLine("*** SDK failed to initialize\n");
                Environment.Exit(1);
            }

            // Set up the evaluation context. This context should appear on your LaunchDarkly contexts
            // dashboard soon after you run the demo.
            var context = Context.Builder("example-user-key")
                .Name("Sandy")
                .Build();

            if (Environment.GetEnvironmentVariable("LAUNCHDARKLY_FLAG_KEY") != null) {
                FeatureFlagKey = Environment.GetEnvironmentVariable("LAUNCHDARKLY_FLAG_KEY");
            }

            var flagValue = client.BoolVariation(FeatureFlagKey, context, false);

            Console.WriteLine(string.Format("*** The {0} feature flag evaluates to {1}.\n",
                FeatureFlagKey, flagValue));

            if (flagValue) ShowBanner();

            client.FlagTracker.FlagChanged += client.FlagTracker.FlagValueChangeHandler(
                FeatureFlagKey,
                context,
                (sender, changeArgs) => {
                    Console.WriteLine(string.Format("*** The {0} feature flag evaluates to {1}.\n",
                    FeatureFlagKey, changeArgs.NewValue));

                    if (changeArgs.NewValue.AsBool) ShowBanner();
                }
            );

            if(CI) Environment.Exit(0);

            Console.WriteLine("*** Waiting for changes \n");

            Task waitForever = new Task(() => {});
            waitForever.Wait();
        }
    }
}
