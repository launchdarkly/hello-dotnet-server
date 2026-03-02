using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Server;
using LaunchDarkly.Sdk.Server.Integrations;

namespace HelloDotNet
{
    /// <summary>
    /// A delegating handler that sets the HTTP version to 2.0 on all outgoing requests.
    /// This can be used with the LaunchDarkly SDK's HttpConfigurationBuilder.MessageHandler()
    /// to force all SDK HTTP traffic to use HTTP/2.
    /// </summary>
    class Http2Handler : DelegatingHandler
    {
        public Http2Handler(HttpMessageHandler innerHandler) : base(innerHandler) { }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Version = HttpVersion.Version20;
            return base.SendAsync(request, cancellationToken);
        }
    }

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

            // Configure the SDK to use HTTP/2 by providing a custom message handler
            // that sets the HTTP version on every outgoing request.
            var handler = new Http2Handler(new SocketsHttpHandler());

            var ldConfig = Configuration.Builder(SdkKey)
                .Http(
                    Components.HttpConfiguration()
                        .MessageHandler(handler)
                )
                .Build();

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
