using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Graphics;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;


namespace CrankItUp.Game
{
    public class TitleScreen : Screen
    {

        BasicButton testLevelsButton;
        BasicButton settingsButton;
        BasicButton creditsButton;
        Track track;

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            track = audio.GetTrackStore().Get("Fly Wit Me.mp3");
            audio.AddAdjustment(AdjustableProperty.Volume, new BindableDouble(Settings.volume.Value));
            track.Start();
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
            creditsButton = new BasicButton
            {
                Anchor = Anchor.Centre,
                Text = "Credits",
                BackgroundColour = Color4.AntiqueWhite,
                Colour = Color4.Black,
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(0, 100),
                Action = () => pushCredits(),
            };

            InternalChildren = new Drawable[] {

            testLevelsButton,
            settingsButton,
            creditsButton,

            new SpriteText{
                Y = 20,
                Text = "Welcome to Crank it Up",
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                Font = FontUsage.Default.With(size: 40)
            },

        };
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


    }

}
