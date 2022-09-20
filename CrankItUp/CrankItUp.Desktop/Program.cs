using osu.Framework.Platform;
using osu.Framework;
using CrankItUp.Game;

namespace CrankItUp.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(@"CrankItUp"))
            using (osu.Framework.Game game = new CrankItUpGame())
                host.Run(game);
        }
    }
}
