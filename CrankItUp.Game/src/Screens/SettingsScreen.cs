using System.IO;
using CrankItUp.Game.Elements;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace CrankItUp.Game.Screens
{
    internal class Settings
    {
        public enum InputMode
        {
            Rotational,
            Linear
        }

        public static InputMode Inputmode = InputMode.Rotational;
    }

    public partial class SettingsScreen : Screen
    {
        private CiuButton inputModeButton;
        private CiuButton windowModeButton;
        private CiuButton backButton;
        private CiuButton setupButton;
        private SpriteText globalVolumeText;
        private BasicSliderBar<double> globalVolumeSlider;
        private SpriteText musicVolumeText;
        private BasicSliderBar<double> musicVolumeSlider;
        private SpriteText effectsVolumeText;
        private BasicSliderBar<double> effectsVolumeSlider;
        private Storage storage;
        private Bindable<WindowMode> windowModeBindable;

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
            inputModeButton = new CiuButton(textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(0, 30),
                Text = "Input mode: " + Settings.Inputmode.ToString(),
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Action = ChangeInputMode,
            };
            windowModeButton = new CiuButton(textures)
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
            setupButton = new CiuButton(textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(0, 130),
                Text = "Restart initial setup",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Action = () => PushInitialSetup(audio),
            };
            backButton = new CiuButton(textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(0, 180),
                Text = "Back to menu",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Action = PushMenu,
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
            WindowMode currentValue = frameworkConfig.Get<WindowMode>(FrameworkSetting.WindowMode);
            frameworkConfig.SetValue(FrameworkSetting.WindowMode, currentValue + 1);
        }

        public void ChangeInputMode()
        {
            // switch input modes
            if (Settings.Inputmode == Settings.InputMode.Rotational)
            {
                Settings.Inputmode = Settings.InputMode.Linear;
            }
            else
            {
                Settings.Inputmode = Settings.InputMode.Rotational;
            }

            // update button text
            inputModeButton.Text = "Input mode: " + Settings.Inputmode.ToString();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == osuTK.Input.Key.Escape)
            {
                PushMenu();
            }

            return base.OnKeyDown(e);
        }

        public void PushMenu()
        {
            // save settings to disk
            JObject settings = new JObject { { "inputMode", (int)Settings.Inputmode }, };
            StreamWriter settingswriter = new StreamWriter(
                storage.CreateFileSafely("settings.json")
            );
            settingswriter.Write(settings.ToJson());
            settingswriter.Close();
            // finally, exit
            this.Exit();
        }

        public void PushInitialSetup(AudioManager audio)
        {
            this.Push(new InitialSetup());
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            this.FadeInFromZero(500, Easing.OutQuint);
        }
    }
}
