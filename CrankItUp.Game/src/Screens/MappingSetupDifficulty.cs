using CrankItUp.Game.Elements;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;

namespace CrankItUp.Game.Screens
{
    public partial class MappingSetupDifficulty : Screen
    {
        private CiuButton backButton;
        private CiuButton confirmButton;
        private BasicTextBox difficultySelector;
        private readonly string trackFilename;

        public MappingSetupDifficulty(string trackFilename)
        {
            this.trackFilename = trackFilename;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            backButton = new CiuButton(textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "Back",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(0, 50),
                Action = pushMenu,
            };

            confirmButton = new CiuButton(textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "Confirm",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(0, 0),
                Action = pushMappingScreen,
            };

            difficultySelector = new BasicTextBox
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(0, -50),
                Size = new Vector2(200, 40),
                PlaceholderText = "Difficulty Name"
            };

            InternalChildren = new Drawable[]
            {
                new DrawSizePreservingFillContainer
                {
                    backButton,
                    difficultySelector,
                    confirmButton,
                    new SpriteText
                    {
                        Y = 20,
                        Text = "Difficulty Setup",
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Font = FontUsage.Default.With(size: 40)
                    },
                }
            };
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

        private void pushMappingScreen()
        {
            this.Push(new MappingScreen(difficultySelector.Text, trackFilename));
        }

        private void pushMenu()
        {
            this.Exit();
        }
    }
}
