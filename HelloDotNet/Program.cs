using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LaunchDarkly.Client;

namespace HelloDotNet
{
  class Program
  {
    static void Main(string[] args)
    {
      // TODO: Enter your LaunchDarkly API key here
      LdClient client = new LdClient("YOU_API_KEY");
      User user = User.WithKey("bob@example.com")
        .AndFirstName("Bob")
        .AndLastName("Loblaw")
        .AndCustomAttribute("groups", "beta_testers");

      // TODO: Enter the key for your feature flag key here
      var value = client.Toggle("YOU_FEATURE_FLAG_KEY", user, false).ContinueWith((task) =>
      {
        if (task.Result)
        {
          Console.WriteLine("Showing feature for user " + user.Key);
        }
        else
        {
          Console.WriteLine("Not showing feature for user " + user.Key);
        }

        client.Flush();
      });

      value.Wait();

      Console.WriteLine("Press any key to exit");
      Console.ReadKey();

    }
  }
}
