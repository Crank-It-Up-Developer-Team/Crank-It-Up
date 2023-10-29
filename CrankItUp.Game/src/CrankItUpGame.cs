using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using System;
using System.IO;

namespace CrankItUp.Game
{
    public class CrankItUpGame : CrankItUpGameBase
    {
        private ScreenStack screenStack;

        [BackgroundDependencyLoader]
        private void load()
        {
            Directory.CreateDirectory(Constants.APPDATA_DIR);
            Directory.SetCurrentDirectory(Constants.APPDATA_DIR);
            // Add your top-level game components here.
            // A screen stack and sample screen has been provided for convenience, but you can replace it if you don't want to use screens.
            Child = screenStack = new ScreenStack { RelativeSizeAxes = Axes.Both };
        }

        protected override void LoadComplete()
        {
            // if we need to do initial setup
            if (!Directory.Exists("maps"))
            {
                screenStack.Push(new InitialSetup());
            }
            else
            {
                screenStack.Push(new TitleScreen());
            }
            base.LoadComplete();
        }
    }
}
