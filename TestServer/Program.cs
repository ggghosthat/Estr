using Estr.Host;
using System;
using System.Threading.Tasks;

namespace Test
{
    static class Program
    {
        public static async Task Main(string[] args)
        {
            EstrHost host = new(null);
            await host.StartAsync();
        }
    }
}
