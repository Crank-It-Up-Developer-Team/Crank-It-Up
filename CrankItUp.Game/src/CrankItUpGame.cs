using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Framework.Screens;
using System;
using System.IO;
using CrankItUp.Game.Screens;

namespace CrankItUp.Game
{
    public partial class CrankItUpGame : CrankItUpGameBase
    {
        private ScreenStack screenStack;
        private bool showSetup;

        [BackgroundDependencyLoader]
        private void load(Storage store)
        {
            // Add your top-level game components here.
            // A screen stack and sample screen has been provided for convenience, but you can replace it if you don't want to use screens.
            Child = screenStack = new ScreenStack { RelativeSizeAxes = Axes.Both };

            // load settings
            try
            {
                var sr = new StreamReader(store.GetStream("settings.json"));
                var settings = JObject.Parse(sr.ReadToEnd());
                Settings.Inputmode = (Settings.InputMode)settings.GetValue<int>("inputMode");
            }
            catch
            {
                Console.WriteLine("Invalid or non-existant settings! Using default values");
            }

            showSetup = !store.ExistsDirectory("maps");
        }

        protected override void LoadComplete()
        {
            // if we need to do initial setup
            if (showSetup)
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
