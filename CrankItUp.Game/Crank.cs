using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Cursor;
using osuTK;
using System;
using osu.Framework.Input.Events;


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
        public float Sensitivity;

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
        public void updateRotation(Vector2 MousePos){
            box.Rotation = (float)((180 / Math.PI) * Math.Atan2(MousePos.Y, MousePos.X)) + 90;
        }
    }
}