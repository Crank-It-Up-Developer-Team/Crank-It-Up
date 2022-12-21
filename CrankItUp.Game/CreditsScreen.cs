using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;

    public class CreditsScreen : Screen
    {


        CIUButton backButton;
        Track track;

        [BackgroundDependencyLoader]
        private void load(AudioManager audio, TextureStore textures)
        {
            track = audio.GetTrackStore().Get("Tera I_O.mp3");
            track.Start();
            backButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Text = "Back to menu",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(0, 0),
                Action = () => PushMenu(),
            };

            string[] credits =
            {
                "AnnoyingRains - Original concept and Programming",
                "MrJamesGaming - Programming",
                "Camellia - Allowing free use of the album 'Tera I/O'"
            };
            SpriteText[] creditsTextList;
            creditsTextList = new SpriteText[credits.Length];
            SpriteText TempText;
            for (int i = 0; i < credits.Length; i++)
            {

                TempText = new SpriteText
                {
                    Y = 100 + (i * 30),
                    Text = credits[i],
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Font = FontUsage.Default.With(size: 20)
                };
                creditsTextList[i] = TempText;
            }
            ;

            InternalChildren = new Drawable[]
            {
                backButton,
                new SpriteText
                {
                    Y = 20,
                    Text = "Crank It up - Credits",
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Font = FontUsage.Default.With(size: 40)
                },
                creditsTextList[0],
                creditsTextList[1],
                creditsTextList[2]
            };

        }

        void PushMenu()
        {
            track.Stop();
            this.Exit();
        }
    }
}

