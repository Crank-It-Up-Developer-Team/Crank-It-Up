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
using osu.Framework.Configuration;
using Newtonsoft.Json.Linq;
using System.IO;
using NuGet.Protocol;
using osu.Framework.Platform;
using osu.Framework.Graphics.Containers;
using osu.Framework.Logging;

namespace CrankItUp.Game
{
    class Settings
    {
        public enum InputMode
        {
            Rotational,
            Linear
        }

        public static InputMode inputmode = InputMode.Rotational;
    }

    public partial class SettingsScreen : Screen
    {
        CIUButton inputModeButton;
        CIUButton windowModeButton;
        CIUButton backButton;
        CIUButton setupButton;
        SpriteText globalVolumeText;
        BasicSliderBar<double> globalVolumeSlider;
        SpriteText musicVolumeText;
        private BasicSliderBar<double> musicVolumeSlider;
        private SpriteText effectsVolumeText;
        private BasicSliderBar<double> effectsVolumeSlider;
        Storage storage;
        Bindable<WindowMode> windowModeBindable;

        [BackgroundDependencyLoader]
        private void load(
            AudioManager audio,
            TextureStore textures,
            Storage store,
            FrameworkConfigManager frameworkConfig
        )
        {
            storage = store;
            globalVolumeText = new SpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(0, -170),
                Text = "Global Volume",
                Colour = Color4.White,
            };
            globalVolumeSlider = new BasicSliderBar<double>
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(0, -140),
                Size = new Vector2(200, 20),
                RangePadding = 20,
                BackgroundColour = Color4.White,
                SelectionColour = Color4.Blue,
                KeyboardStep = 0.1f,
                Current = frameworkConfig.GetBindable<double>(FrameworkSetting.VolumeUniversal)
            };
            musicVolumeText = new SpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(0, -110),
                Text = "Music Volume",
                Colour = Color4.White,
            };
            musicVolumeSlider = new BasicSliderBar<double>
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(0, -80),
                Size = new Vector2(200, 20),
                RangePadding = 20,
                BackgroundColour = Color4.White,
                SelectionColour = Color4.Blue,
                KeyboardStep = 0.1f,
                Current = frameworkConfig.GetBindable<double>(FrameworkSetting.VolumeMusic)
            };
            effectsVolumeText = new SpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(0, -50),
                Text = "Effects Volume",
                Colour = Color4.White,
            };
            effectsVolumeSlider = new BasicSliderBar<double>
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(0, -20),
                Size = new Vector2(200, 20),
                RangePadding = 20,
                BackgroundColour = Color4.White,
                SelectionColour = Color4.Blue,
                KeyboardStep = 0.1f,
                Current = frameworkConfig.GetBindable<double>(FrameworkSetting.VolumeEffect)
            };
            inputModeButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(0, 30),
                Text = "Input mode: " + Settings.inputmode.ToString(),
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Action = () => changeInputMode(),
            };
            windowModeButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(0, 80),
                Text =
                    "Window type: " + frameworkConfig.Get<WindowMode>(FrameworkSetting.WindowMode),
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Action = () => changeWindowMode(frameworkConfig),
            };
            setupButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(0, 130),
                Text = "Restart initial setup",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Action = () => pushInitialSetup(audio),
            };
            backButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(0, 180),
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
                    windowModeButton,
                    globalVolumeText,
                    globalVolumeSlider,
                    effectsVolumeText,
                    effectsVolumeSlider,
                    musicVolumeText,
                    musicVolumeSlider,
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
            // set up bindables and events
            windowModeBindable = frameworkConfig.GetBindable<WindowMode>(
                FrameworkSetting.WindowMode
            );
            windowModeBindable.ValueChanged += onWindowModeChange;
        }

        private void onWindowModeChange(ValueChangedEvent<WindowMode> @event)
        {
            Logger.Log(@event.NewValue.ToString());
            windowModeButton.Text = "Window mode: " + @event.NewValue.ToString();
        }

        private void changeWindowMode(FrameworkConfigManager frameworkConfig)
        {
            // this function only changes the window mode, it doesn't change the text, as that is handled by the event
            WindowMode currentvalue = frameworkConfig.Get<WindowMode>(FrameworkSetting.WindowMode);
            frameworkConfig.SetValue(FrameworkSetting.WindowMode, currentvalue + 1);
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
            inputModeButton.Text = "Input mode: " + Settings.inputmode.ToString();
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
            JObject settings = new JObject { { "inputMode", (int)Settings.inputmode }, };
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
