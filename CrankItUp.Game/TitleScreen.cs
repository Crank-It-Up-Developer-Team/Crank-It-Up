using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Graphics;
using osu.Framework.Graphics.UserInterface;
using System;
using osu.Framework.Input;
using osuTK;



namespace CrankItUp.Game
{
    public class TitleScreen : Screen
    {

        BasicButton testLevelsButton;
        BasicButton settingsButton;

        [BackgroundDependencyLoader]
        private void load()
        {
            testLevelsButton = new BasicButton
            {
                Anchor = Anchor.Centre,
                Text = "Begin Test Levels",
                BackgroundColour = Color4.AntiqueWhite,
                Colour = Color4.Black,
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(0, 0),
                Action = () => pushLevel(),
            };

            settingsButton = new BasicButton
            {
                Anchor = Anchor.Centre,
                Text = "Settings",
                BackgroundColour = Color4.AntiqueWhite,
                Colour = Color4.Black,
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(0, 50),
                Action = () => pushSettings(),
            };

            InternalChildren = new Drawable[] {

            testLevelsButton,
            settingsButton,

            new SpriteText{
                Y = 20,
                Text = "Welcome to Crank it Up",
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                Font = FontUsage.Default.With(size: 40)
            },

        };
        }
        public void test()
        {
            testLevelsButton.Text = "button has been pushed";
        }

        public void pushSettings()
        {
            this.Push(new SettingsScreen());
        }
        public void pushLevel()
        {
            this.Push(new LevelScreen());
        }


    }

}
