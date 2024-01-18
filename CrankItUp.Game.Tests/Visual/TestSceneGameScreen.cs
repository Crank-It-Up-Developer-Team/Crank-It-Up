using CrankItUp.Game.Screens;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using NUnit.Framework;

namespace CrankItUp.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneGameScreen : CrankItUpTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.

        public TestSceneGameScreen()
        {
            Add(new ScreenStack(new LevelScreen("Test", "easy")) { RelativeSizeAxes = Axes.Both });
        }
    }
}
