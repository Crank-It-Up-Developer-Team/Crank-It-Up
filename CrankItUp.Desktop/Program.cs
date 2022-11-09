using osu.Framework.Platform;
using osu.Framework;
using CrankItUp.Game;
using System;
using CrankItUp.Desktop;

namespace CrankItUp.Game
{
    public static class Program
    {
        private static osu.Framework.Game game = new CrankItUpGame();
        private static GameHost host = Host.GetSuitableDesktopHost(@"CrankItUp");
        public static Discord discordrpc = new Discord();
        public static void Main()
        {
            discordrpc.Initialize();
            using (host)
            using (game)
                host.Run(game);
        }

        public static CrankItUpGame getGame(){
            return (CrankItUpGame)game;
        }

        public static GameHost GetGameHost(){
            return host;
        }

    }
}
