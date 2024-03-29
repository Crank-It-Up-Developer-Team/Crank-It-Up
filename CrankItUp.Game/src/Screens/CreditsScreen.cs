using CrankItUp.Game.Elements;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;

namespace CrankItUp.Game.Screens
{
    public partial class CreditsScreen : Screen
    {
        private CiuButton backButton;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            backButton = new CiuButton(textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "Back to menu",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(0, 0),
                Action = pushMenu,
            };
            InternalChildren = new Drawable[]
            {
                new DrawSizePreservingFillContainer
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
                }
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
                pushMenu();
            }

            return base.OnKeyDown(e);
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            this.FadeInFromZero(500, Easing.OutQuint);
        }

        private void pushMenu()
        {
            this.Exit();
        }
    }
}
