using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Bindables;
using osuTK;
using osu.Framework.Audio;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using Newtonsoft.Json.Linq;
using System.IO;
using NuGet.Protocol;
using osu.Framework.Platform;
using osu.Framework.Graphics.Containers;

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
        public static BindableDouble volume = new BindableDouble
        {
            Default = 0.5,
            Value = 0.5,
            MinValue = 0,
            MaxValue = 1
        };
        public static InputMode inputmode = InputMode.Rotational;
    }

    public partial class SettingsScreen : Screen
    {
        CIUButton inputModeButton;
        CIUButton backButton;
        CIUButton setupButton;
        SpriteText volumeText;
        BasicSliderBar<double> volumeSlider;
        Storage storage;

        [BackgroundDependencyLoader]
        private void load(AudioManager audio, TextureStore textures, Storage store)
        {
            storage = store;
            audio.AddAdjustment(AdjustableProperty.Volume, Settings.volume);
            volumeText = new SpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(0, -30),
                Text = "Volume",
                Colour = Color4.White,
            };
            volumeSlider = new BasicSliderBar<double>
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(0, -10),
                Size = new Vector2(200, 20),
                RangePadding = 20,
                BackgroundColour = Color4.White,
                SelectionColour = Color4.Blue,
                KeyboardStep = 1,
                Current = Settings.volume
            };
            inputModeButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(0, 30),
                Text = "Input Mode: " + Settings.inputmode.ToString(),
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Action = () => changeInputMode(),
            };
            setupButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(0, 80),
                Text = "Restart initial setup",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Action = () => pushInitialSetup(audio),
            };
            backButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(0, 130),
                Text = "Back to menu",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Action = () => pushMenu(),
            };

            InternalChildren = new Drawable[]
            {
                new DrawSizePreservingFillContainer
                {
                    inputModeButton,
                    volumeText,
                    volumeSlider,
                    setupButton,
                    backButton,
                    new SpriteText
                    {
                        Y = 20,
                        Text = "Settings",
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Font = FontUsage.Default.With(size: 40)
                    },
                }
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

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == osuTK.Input.Key.Escape)
            {
                pushMenu();
            }
            return base.OnKeyDown(e);
        }

        public void pushMenu()
        {
            // save settings to disk
            JObject settings = new JObject
            {
                { "inputMode", (int)Settings.inputmode },
                { "volume", Settings.volume.Value }
            };
            StreamWriter settingswriter = new StreamWriter(
                storage.CreateFileSafely("settings.json")
            );
            settingswriter.Write(settings.ToJson());
            settingswriter.Close();
            // finally, exit
            this.Exit();
        }

        public void pushInitialSetup(AudioManager audio)
        {
            this.Push(new InitialSetup());
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            this.FadeInFromZero(500, Easing.OutQuint);
        }
    }
}
