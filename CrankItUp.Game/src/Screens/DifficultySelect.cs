using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osu.Framework.Graphics.Textures;
using osuTK;
using osu.Framework.Audio;
using System.IO;
using osu.Framework.Graphics.Containers;

namespace CrankItUp.Game
{
    public class DifficultySelect : Screen
    {
        Container trackContainer;
        CIUButton backButton;
        string map;

        public DifficultySelect(string mapname)
        {
            map = mapname;
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio, TextureStore textures)
        {
            backButton = new CIUButton(textures)
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                Position = new Vector2(0, 700),
                Text = "Back",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Action = () => pushMenu(audio),
            };
            trackContainer = new Container()
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.BottomRight,
                Position = new Vector2(0, 100)
            };
            var difficulties = Directory.EnumerateFiles("maps/" + map);
            Vector2 position = new Vector2(0, 0);
            foreach (string difficulty in difficulties)
            {
                if (difficulty.EndsWith(".json"))
                {
                    string difficultyname = difficulty[
                        (difficulty.LastIndexOf("/") + 1)..(difficulty.Length - 5)
                    ];
                    trackContainer.Add(
                        new CIUButton(textures)
                        {
                            Text = difficultyname,
                            Size = new Vector2(200, 40),
                            Margin = new MarginPadding(10),
                            Position = position,
                            Action = () => pushLevel(difficultyname),
                        }
                    );
                }
                position.Y += 50;
                if (position.Y == 600)
                {
                    position.Y = 0;
                    position.X += 250;
                }
            }
            InternalChildren = new Drawable[]
            {
                new SpriteText
                {
                    Y = 20,
                    Text = "Select a difficulty",
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Font = FontUsage.Default.With(size: 40)
                },
                backButton,
                trackContainer,
            };
        }

        private void pushMenu(AudioManager audio)
        {
            this.Exit();
        }

        public void pushLevel(string difficultyname)
        {
            this.Push(new LevelScreen(map, difficultyname));
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
