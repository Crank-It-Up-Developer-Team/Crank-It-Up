using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Graphics;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace CrankItUp.Game
{
    class Settings
    {
        public enum InputMode
        {
            Rotational,
            Linear
        }

        // init vars
        public static InputMode inputmode = InputMode.Rotational;
    }

    public class SettingsScreen : Screen
    {
        BasicButton inputModeButton;
        BasicButton backButton;
        Button test;

        [BackgroundDependencyLoader]
        private void load()
        {
            inputModeButton = new BasicButton
            {
                Anchor = Anchor.Centre,
                Text = "Input Mode: " + Settings.inputmode.ToString(),
                BackgroundColour = Color4.White,
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Action = () => changeInputMode(),
            };
            backButton = new BasicButton
            {
                Anchor = Anchor.Centre,
                Text = "Back to menu",
                BackgroundColour = Color4.White,
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(0, 50),
                Action = () => pushMenu(),
            };

            InternalChildren = new Drawable[]
            {
                inputModeButton,
                backButton,
                new SpriteText
                {
                    Y = 20,
                    Text = "Settings",
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Font = FontUsage.Default.With(size: 40)
                },
            };
        }

        public void changeInputMode()
        {
            // switch input modes
            if (Settings.inputmode == Settings.InputMode.Rotational)
            {
                Settings.inputmode = Settings.InputMode.Linear;
            }
            else
            {
                Settings.inputmode = Settings.InputMode.Rotational;
            }
            // update button text
            inputModeButton.Text = "Input Mode: " + Settings.inputmode.ToString();
        }

        public void pushMenu()
        {
            this.Push(new TitleScreen());
        }
    }
}
