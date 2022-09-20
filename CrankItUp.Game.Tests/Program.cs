using osu.Framework;
using osu.Framework.Platform;

namespace CrankItUp.Game.Tests
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost("visual-tests"))
            using (var game = new CrankItUpTestBrowser())
                host.Run(game);
        }
    }
}
