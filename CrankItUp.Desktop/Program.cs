using CrankItUp.Game;
using osu.Framework;
using osu.Framework.Platform;

namespace CrankItUp.Desktop
{
    public static class Program
    {
        private static readonly osu.Framework.Game game = new CrankItUpGame();
        private static readonly GameHost host = Host.GetSuitableDesktopHost(@"CrankItUp");

        private static readonly Discord discordrpc = new Discord();

        public static void Main()
        {
            discordrpc.Initialize();
            using (host)
            using (game)
                host.Run(game);
        }

        public static CrankItUpGame GetGame()
        {
            return (CrankItUpGame)game;
        }

        public static GameHost GetGameHost()
        {
            return host;
        }
    }
}
