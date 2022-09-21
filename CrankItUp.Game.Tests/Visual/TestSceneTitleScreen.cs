using osu.Framework.Graphics;
using osu.Framework.Screens;
using NUnit.Framework;

namespace CrankItUp.Game.Tests.Visual
{
    [TestFixture]
    public class TestSceneTitleScreen : CrankItUpTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.

        public TestSceneTitleScreen()
        {
            Add(new ScreenStack(new TitleScreen()) { RelativeSizeAxes = Axes.Both });
        }
    }
}
