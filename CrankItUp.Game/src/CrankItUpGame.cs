using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using System;
using System.IO;

namespace CrankItUp.Game
{
    public partial class CrankItUpGame : CrankItUpGameBase
    {
        private ScreenStack screenStack;

        [BackgroundDependencyLoader]
        private void load()
        {
            // Add your top-level game components here.
            // A screen stack and sample screen has been provided for convenience, but you can replace it if you don't want to use screens.
            Child = screenStack = new ScreenStack { RelativeSizeAxes = Axes.Both };
            // load settings
            try
            {
                var sr = new StreamReader("settings.json");
                // Read the stream as a string, and write the string to the console.
                var settings = JObject.Parse(sr.ReadToEnd());
                Settings.inputmode = (Settings.InputMode)settings.GetValue<int>("inputMode");
                Settings.volume.Value = settings.GetValue<double>("volume");
            }
            catch
            {
                Console.WriteLine("Invalid or non-existant settings! Using default values");
            }
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
