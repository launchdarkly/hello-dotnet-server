using System;
using System.Threading.Tasks;
using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Server;

namespace HelloDotNet
{
    class Hello
    {
        public static void ShowBanner(){
            Console.WriteLine(
@"        ██
          ██
      ████████
         ███████
██ LAUNCHDARKLY █
         ███████
      ████████
          ██
        ██
");
        }

        static void Main(string[] args)
        {
            bool CI = Environment.GetEnvironmentVariable("CI") != null;

            string SdkKey = Environment.GetEnvironmentVariable("LAUNCHDARKLY_SDK_KEY");

            // Set FeatureFlagKey to the feature flag key you want to evaluate.
            string FeatureFlagKey = "sample-feature";

            if (string.IsNullOrEmpty(SdkKey))
            {
                Console.WriteLine("*** Please set LAUNCHDARKLY_SDK_KEY environment variable to your LaunchDarkly SDK key first\n");
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

            if (Environment.GetEnvironmentVariable("LAUNCHDARKLY_FLAG_KEY") != null)
            {
                FeatureFlagKey = Environment.GetEnvironmentVariable("LAUNCHDARKLY_FLAG_KEY");
            }

            var flagValue = client.BoolVariation(FeatureFlagKey, context, false);

            Console.WriteLine($"*** The {FeatureFlagKey} feature flag evaluates to {flagValue}.\n");

            if (flagValue)
            {
                ShowBanner();
            }

            client.FlagTracker.FlagChanged += client.FlagTracker.FlagValueChangeHandler(
                FeatureFlagKey,
                context,
                (sender, changeArgs) => {
                    Console.WriteLine($"*** The {FeatureFlagKey} feature flag evaluates to {changeArgs.NewValue}.\n");

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
