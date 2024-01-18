using CrankItUp.Game.Elements;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using osuTK;

namespace CrankItUp.Game.Screens
{
    public partial class TitleScreen : Screen
    {
        private CiuButton selectTrackButton;
        private CiuButton settingsButton;
        private CiuButton creditsButton;
        private CiuButton mappingButton;
        private Track track;

        [BackgroundDependencyLoader]
        private void load(AudioManager audio, TextureStore textures)
        {
            track = audio.GetTrackStore().Get("Body F10ating in the Zero Gravity Space.mp3");
            track.Start();
            track.Looping = true;

            selectTrackButton = new CiuButton(textures)
            {
                Anchor = Anchor.Centre,
                Text = "Play!",
                //BackgroundColour = Color4.AntiqueWhite,
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(-100, 0),
                Action = PushSelectTrack,
            };

            settingsButton = new CiuButton(textures)
            {
                Anchor = Anchor.Centre,
                Text = "Settings",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(-100, 50),
                Action = PushSettings,
            };
            mappingButton = new CiuButton(textures)
            {
                Anchor = Anchor.Centre,
                Text = "Map editor",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(-100, 100),
                Action = PushMappingSetup,
            };
            creditsButton = new CiuButton(textures)
            {
                Anchor = Anchor.Centre,
                Text = "Credits",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(-100, 150),
                Action = PushCredits,
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

        public void PushSettings()
        {
            this.Push(new SettingsScreen());
        }

        public void PushSelectTrack()
        {
            track.Stop();
            this.Push(new TrackSelect());
        }

        public void PushMappingSetup()
        {
            track.Stop();
            this.Push(new MappingSetupTrack());
        }

        public void PushCredits()
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
