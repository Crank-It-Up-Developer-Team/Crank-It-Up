using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osu.Framework.Graphics.Textures;
using osuTK;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;

namespace CrankItUp.Game
{
    public partial class TitleScreen : Screen
    {
        CIUButton selectTrackButton;
        CIUButton settingsButton;
        CIUButton creditsButton;
        CIUButton mappingButton;
        Track track;

        [BackgroundDependencyLoader]
        private void load(AudioManager audio, TextureStore textures)
        {
            track = audio.GetTrackStore().Get("Body F10ating in the Zero Gravity Space.mp3");
            audio.AddAdjustment(
                AdjustableProperty.Volume,
                new BindableDouble(Settings.volume.Value)
            );
            track.Start();
            track.Looping = true;

            selectTrackButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Text = "Play!",
                //BackgroundColour = Color4.AntiqueWhite,
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(-100, 0),
                Action = () => pushSelectTrack(),
            };

            settingsButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Text = "Settings",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(-100, 50),
                Action = () => pushSettings(),
            };
            mappingButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Text = "Map editor",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(-100, 100),
                Action = () => pushMappingSetup(),
            };
            creditsButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Text = "Credits",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(-100, 150),
                Action = () => pushCredits(),
            };

            InternalChildren = new Drawable[]
            {
                new DrawSizePreservingFillContainer
                {
                    selectTrackButton,
                    settingsButton,
                    mappingButton,
                    creditsButton,
                    new SpriteText
                    {
                        Y = 20,
                        Text = "Welcome to Crank it Up",
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Font = FontUsage.Default.With(size: 40)
                    },
                }
            };
        }

        public void pushSettings()
        {
            this.Push(new SettingsScreen());
        }

        public void pushSelectTrack()
        {
            track.Stop();
            this.Push(new TrackSelect());
        }

        public void pushMappingSetup()
        {
            track.Stop();
            this.Push(new MappingSetupTrack());
        }

        public void pushCredits()
        {
            this.Push(new CreditsScreen());
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            this.FadeInFromZero(500, Easing.OutQuint);
        }

        public override void OnResuming(ScreenTransitionEvent e)
        {
            track.Start();
            this.FadeInFromZero(500, Easing.OutQuint);
        }
    }
}
