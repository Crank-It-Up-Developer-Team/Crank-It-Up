using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Cursor;
using osuTK;
using System;


namespace CrankItUp.Game
{
    public class Crank : CompositeDrawable
    {
        public Crank()
        {
            AutoSizeAxes = Axes.Both;
            Origin = Anchor.Centre;
        }

        private Container box;
        private CursorContainer mouse;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {

            mouse = new CursorContainer();
            mouse.ToggleVisibility();

            InternalChild = box = new Container
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new Drawable[]
               {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    },
                    new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Texture = textures.Get("crank")
                    },
               }

            };
        }
        protected override void LoadComplete()
        {
            base.LoadComplete();
        }

        protected override void Update()
        {
            Vector2 pos = mouse.Position;
            float direction = (float)Math.Atan2(pos.Y, pos.X);
            box.RotateTo(direction, 1000);
        }
    }
}