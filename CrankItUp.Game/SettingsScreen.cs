using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Bindables;
using osuTK;
using osu.Framework.Audio;

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
        public static BindableDouble volume = new BindableDouble{
            Default = 0.5,
            Value = 0.5,
            MinValue = 0,
            MaxValue = 1
        };
        public static InputMode inputmode = InputMode.Rotational;
    }

    public class SettingsScreen : Screen
    {
        BasicButton inputModeButton;
        BasicButton backButton;
        SpriteText volumeText;
        BasicSliderBar<double> volumeSlider;

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            audio.AddAdjustment(AdjustableProperty.Volume, Settings.volume);
            inputModeButton = new BasicButton
            {
                Anchor = Anchor.Centre,
                Position = new Vector2(0, -50),
                Text = "Input Mode: " + Settings.inputmode.ToString(),
                BackgroundColour = Color4.White,
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Action = () => changeInputMode(),
            };
            volumeText = new SpriteText
            {
                Anchor = Anchor.Centre,
                Position = new Vector2(0, 20),
                Text = "Volume",
                Colour = Color4.White,
            };
            volumeSlider = new BasicSliderBar<double>
                {
                    Anchor = Anchor.Centre,
                    Position = new Vector2(10, 40), // shift 10 to the right as it looks odd otherwise, unsure why
                    Size = new Vector2(200, 20),
                    RangePadding = 20,
                    BackgroundColour = Color4.White,
                    SelectionColour = Color4.Blue,
                    KeyboardStep = 1,
                    Current = Settings.volume
                };
            backButton = new BasicButton
            {
                Anchor = Anchor.Centre,
                Position = new Vector2(0, 80),
                Text = "Back to menu",
                BackgroundColour = Color4.White,
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Action = () => pushMenu(audio),
            };

            InternalChildren = new Drawable[]
            {
                inputModeButton,
                volumeText,
                volumeSlider,
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

        public void pushMenu(AudioManager audio)
        {
            this.Exit();
        }
    }
}
