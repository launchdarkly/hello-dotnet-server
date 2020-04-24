# LaunchDarkly Sample .NET Application 

We've built a simple console application that demonstrates how LaunchDarkly's SDK works.

 Below, you'll find the basic build procedure, but for more comprehensive instructions, you can visit your [Quickstart page](https://app.launchdarkly.com/quickstart#/) or the [.NET SDK reference guide](https://docs.launchdarkly.com/sdk/server-side/dotnet).

## Build instructions 

The project currently requires a Windows environment. It can be run either in Visual Studio, or from the command line if you have installed the .NET command-line tools.

1. Edit `HelloDotnet/Parameters.cs` and set the value of `SdkKey` to your LaunchDarkly SDK key. If there is an existing boolean feature flag in your LaunchDarkly project that you want to evaluate, set `FeatureFlagKey` to the flag key.

```csharp
    public const String SdkKey = "1234567890abcdef";

    public const String FeatureFlagKey = "my-flag";
```

2. If you are using Visual studio, open `HelloDotNet.sln` and run the application.

3. Or, to run from the command line, type the following commands:

```
    dotnet build
    .\HelloDotNet\bin\Debug\HelloDotnet
```

The demo should print `"Feature flag '<flag key>' is <True/False> for this user"`.
