using CrankItUp.Game.Elements;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;

namespace CrankItUp.Game.Screens
{
    public partial class TrackSelect : Screen
    {
        private Container trackContainer;
        private CiuButton backButton;
        private TrackMetadata trackmeta;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, Storage storage)
        {
            backButton = new CiuButton(textures)
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                Position = new Vector2(0, 0),
                Text = "Back to menu",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Action = pushMenu,
            };
            trackContainer = new Container
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.BottomRight,
                Position = new Vector2(0, 100)
            };

            Vector2 position = new Vector2(0, 0);
            var maps = storage.GetDirectories("maps");

            foreach (string mapPath in maps)
            {
                var map = mapPath[5..];
                Logger.Log("Found map: " + map);

                var mapStorage = storage.GetStorageForDirectory(mapPath);

                try
                {
                    trackmeta = new TrackMetadata(mapStorage.GetStream("metadata.json"));
                }
                catch
                {
                    // if we can't get a metadata.json, or it's invalid, we can skip it
                    // logging the specific issue in the file is done by the TrackMetadata class
                    continue;
                }

                trackContainer.Add(
                    new CiuButton(textures)
                    {
                        Text = trackmeta.Name,
                        Size = new Vector2(200, 40),
                        Margin = new MarginPadding(10),
                        Position = position,
                        Action = () => PushDifficultySelect(map, mapPath, storage),
                    }
                );

                // change the position, relative to it's current position, preventing overlap
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

        public void PushDifficultySelect(string map, string mapPath, Storage storage)
        {
            // we get the metadata again here, as the metadata gathered in load() is overwriten each loop
            var mapStorage = storage.GetStorageForDirectory(mapPath);
            trackmeta = new TrackMetadata(mapStorage.GetStream("metadata.json"));
            this.Push(new DifficultySelect(map, trackmeta));
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
