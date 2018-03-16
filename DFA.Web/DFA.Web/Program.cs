using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace DFA.Web
{
    public class Program
    {
        public static void Main(string[] args)
            => WebHost.CreateDefaultBuilder(args)
                      .UseIISIntegration()
                      .UseStartup<Startup>()
                      .Build()
                      .Run();
    }
}
