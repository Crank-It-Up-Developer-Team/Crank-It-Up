using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osu.Framework.Graphics.Textures;
using osuTK;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using osu.Framework.Logging;

namespace CrankItUp.Game
{
    public partial class TrackSelect : Screen
    {
        Container trackContainer;
        CIUButton backButton;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, Storage storage)
        {
            backButton = new CIUButton(textures)
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                Position = new Vector2(0, 0),
                Text = "Back to menu",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Action = () => pushMenu(),
            };
            trackContainer = new Container()
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.BottomRight,
                Position = new Vector2(0, 100)
            };
            var maps = storage.GetDirectories("maps");
            Vector2 position = new Vector2(0, 0);
            foreach (string mapPath in maps)
            {
                var map = mapPath[5..];
                Logger.Log("Found map: " + map);
                trackContainer.Add(
                    new CIUButton(textures)
                    {
                        Text = map,
                        Size = new Vector2(200, 40),
                        Margin = new MarginPadding(10),
                        Position = position,
                        Action = () => pushDifficultySelect(map),
                    }
                );
                position.Y += 50;
                if (position.Y == 600)
                {
                    position.Y = 0;
                    position.X += 250;
                }
            }
            InternalChildren = new Drawable[]
            {
                new DrawSizePreservingFillContainer
                {
                    new SpriteText
                    {
                        Y = 20,
                        Text = "Select a track",
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Font = FontUsage.Default.With(size: 40)
                    },
                    trackContainer,
                    backButton
                },
            };
        }

        private void pushMenu()
        {
            this.Exit();
        }

        public void pushDifficultySelect(string map)
        {
            this.Push(new DifficultySelect(map));
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

        public override void OnResuming(ScreenTransitionEvent e)
        {
            this.FadeInFromZero(500, Easing.OutQuint);
        }
    }
}
