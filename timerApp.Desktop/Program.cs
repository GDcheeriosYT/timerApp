using System.Threading.Tasks;
using osu.Framework.Platform;
using osu.Framework;
using timerApp.Game;
using Velopack;

namespace timerApp.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            VelopackApp.Build().Run();
            using (GameHost host = Host.GetSuitableDesktopHost(@"timerApp"))
            using (osu.Framework.Game game = new timerAppGame())
                host.Run(game);
        }

        private static async Task UpdateMyApp()
        {
            var mgr = new UpdateManager("https://github.com/GDcheerios/timerApp/releases/latest/download/");

            // check for new version
            var newVersion = await mgr.CheckForUpdatesAsync();
            if (newVersion == null)
                return; // no update available

            // download new version
            await mgr.DownloadUpdatesAsync(newVersion);

            // install new version and restart app
            mgr.ApplyUpdatesAndRestart(newVersion);
        }
    }
}
