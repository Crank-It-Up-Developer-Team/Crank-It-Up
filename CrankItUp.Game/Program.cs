using osu.Framework.Platform;
using osu.Framework;

namespace CrankItUp.Game
{
    public static class Program
    {
        private static osu.Framework.Game game = new CrankItUpGame();
        private static  GameHost host = Host.GetSuitableDesktopHost(@"CrankItUp");
        public static void Main()
        {
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
