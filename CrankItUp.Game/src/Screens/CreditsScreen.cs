using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;

namespace CrankItUp.Game
{
    public partial class CreditsScreen : Screen
    {
        CIUButton backButton;

        [BackgroundDependencyLoader]
        private void load(AudioManager audio, TextureStore textures)
        {
            backButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "Back to menu",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(0, 0),
                Action = () => PushMenu(),
            };
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
            };

            string[] credits =
            {
                "AnnoyingRains - Original concept and Programming",
                "MrJamesGaming - Programming",
                "Camellia - Allowing free use of the album 'Tera I/O'"
            };
            for (int i = 0; i < credits.Length; i++)
            {
                AddInternal(
                    new SpriteText
                    {
                        Y = 200 + (i * 30),
                        Text = credits[i],
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Font = FontUsage.Default.With(size: 20)
                    }
                );
            }
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == osuTK.Input.Key.Escape)
            {
                PushMenu();
            }
            return base.OnKeyDown(e);
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            this.FadeInFromZero(500, Easing.OutQuint);
        }

        void PushMenu()
        {
            this.Exit();
        }
    }
}
