using CrankItUp.Game.Screens;
using osu.Framework.Allocation;
using osu.Framework.Platform;
using NUnit.Framework;
using osu.Framework.Screens;
using osu.Framework.Graphics;

namespace CrankItUp.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneCrankItUpGame : CrankItUpTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.

        private CrankItUpGame game;

        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            game = new CrankItUpGame();
            game.SetHost(host);

            AddGame(game);
            Add(new ScreenStack(new TitleScreen()) { RelativeSizeAxes = Axes.Both });
        }
    }
}
