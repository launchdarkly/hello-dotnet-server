# LaunchDarkly sample .NET application

We've built a simple console application that demonstrates how LaunchDarkly's SDK works.

Below, you'll find the build procedure. For more comprehensive instructions, you can visit your [Quickstart page](https://app.launchdarkly.com/quickstart#/) or the [.NET SDK reference guide](https://docs.launchdarkly.com/sdk/server-side/dotnet).

This is a .NET Core application that can be built on any platform where .NET is available. It can be run either in Visual Studio, or from the command line if you have installed the .NET command-line tools.

## Build instructions

1. Set the environment variable `LAUNCHDARKLY_SERVER_KEY` to your LaunchDarkly SDK key. If there is an existing boolean feature flag in your LaunchDarkly project that you want to evaluate, set `LAUNCHDARKLY_FLAG_KEY` to the flag key; otherwise, a boolean flag of `sample-feature` will be assumed.

    ```bash
    export LAUNCHDARKLY_SERVER_KEY="1234567890abcdef"
    export LAUNCHDARKLY_FLAG_KEY="my-boolean-flag"
    ```

2. If you are using Visual Studio, open `HelloDotNet.sln` and run the application. Or, to run from the command line, type the following command:

```
    dotnet run --project HelloDotNet
```

You should receive the message "The <flagKey> feature flag evaluates to <flagValue>.". The application will run continuously and react to the flag changes in LaunchDarkly.
