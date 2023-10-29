using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osu.Framework.Graphics.Textures;
using osuTK;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;

namespace CrankItUp.Game
{
    public class TitleScreen : Screen
    {
        CIUButton testLevelsButton;
        CIUButton settingsButton;
        CIUButton creditsButton;
        Track track;

        [BackgroundDependencyLoader]
        private void load(AudioManager audio, TextureStore textures)
        {
            track = audio.GetTrackStore().Get("Fly Wit Me.mp3");
            audio.AddAdjustment(
                AdjustableProperty.Volume,
                new BindableDouble(Settings.volume.Value)
            );
            track.Start();

            testLevelsButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Text = "Begin Test Levels",
                //BackgroundColour = Color4.AntiqueWhite,
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(-100, 0),
                Action = () => pushLevel(),
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
            creditsButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Text = "Credits",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(-100, 100),
                Action = () => pushCredits(),
            };

            InternalChildren = new Drawable[]
            {
                testLevelsButton,
                settingsButton,
                creditsButton,
                new SpriteText
                {
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
            track.Stop();
            this.Push(new LevelScreen());
        }

        public void pushCredits()
        {
            track.Stop();
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
