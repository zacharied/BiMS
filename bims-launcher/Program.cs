using BiMS;
using System;

using theori;
using theori.Charting;

namespace BimsLauncher
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            using var host = Host.GetSuitableHost();
            host.Initialize();

            Entity.RegisterTypesFromGameMode(BimsGameMode.Instance);

            host.Run(new BimsClient());
        }
    }
}
