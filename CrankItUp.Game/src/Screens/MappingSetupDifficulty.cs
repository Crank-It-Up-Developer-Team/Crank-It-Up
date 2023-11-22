using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Graphics.Containers;

namespace CrankItUp.Game
{
    public partial class MappingSetupDifficulty : Screen
    {
        CIUButton backButton;
        CIUButton confirmButton;
        BasicTextBox difficultySelector;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            backButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "Back",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(0, 50),
                Action = () => PushMenu(),
            };

            confirmButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "Confirm",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(0, 0),
                Action = () => PushMappingScreen(),
            };

            difficultySelector = new BasicTextBox()
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
                PushMenu();
            }
            return base.OnKeyDown(e);
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            this.FadeInFromZero(500, Easing.OutQuint);
        }

        void PushMappingScreen()
        {
            this.Push(new MappingScreen(difficultySelector.Text));
        }

        void PushMenu()
        {
            this.Exit();
        }
    }
}
