# LaunchDarkly Sample .NET Application 

We've built a simple console application that demonstrates how LaunchDarkly's SDK works.

Below, you'll find the basic build procedure, but for more comprehensive instructions, you can visit your [Quickstart page](https://app.launchdarkly.com/quickstart#/) or the [.NET SDK reference guide](https://docs.launchdarkly.com/sdk/server-side/dotnet).

## Build instructions 

This is a .NET Core application that can be built on any platform where .NET is available. It can be run either in Visual Studio, or from the command line if you have installed the .NET command-line tools.

1. Edit `HelloDotNet/Hello.cs` and set the value of `SdkKey` to your LaunchDarkly SDK key. If there is an existing boolean feature flag in your LaunchDarkly project that you want to evaluate, set `FeatureFlagKey` to the flag key.

```csharp
    public const string SdkKey = "1234567890abcdef";

    public const string FeatureFlagKey = "my-flag";
```

2. If you are using Visual Studio, open `HelloDotNet.sln` and run the application. Or, to run from the command line, type the following command:

```
    dotnet run --project HelloDotNet
```

You should see the message `"Feature flag '<flag key>' is <true/false> for this user"`.
