using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Cursor;
using osuTK;
using System;
using System.Runtime.CompilerServices;
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
        public static Line collisionLine;

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
        
        [MethodImpl (MethodImplOptions.Synchronized)]
        protected override void Update()
        {
            double direction = box.Rotation;
            Vector2 farEnd = Constants.CORNER_TO_CENTER_TRANSFORMATION + new Vector2((float)(Constants.CRANK_DEFAULT_HEIGHT / 2.0 * Math.Cos(direction)), (float)(Constants.CRANK_DEFAULT_LENGTH / 2.0 * Math.Sin(direction)));
            Vector2 nearEnd = Constants.CORNER_TO_CENTER_TRANSFORMATION - new Vector2((float)(Constants.CRANK_DEFAULT_HEIGHT / 2.0 * Math.Cos(direction)), (float)(Constants.CRANK_DEFAULT_LENGTH / 2.0 * Math.Sin(direction)));
            collisionLine = new Line(new Point(nearEnd), new Point(farEnd));
            base.Update();
        }

        

        public void updateRotation(Vector2 MousePos){
            box.Rotation = (float)((180 / Math.PI) * Math.Atan2(MousePos.Y, MousePos.X));
        }
    }
}