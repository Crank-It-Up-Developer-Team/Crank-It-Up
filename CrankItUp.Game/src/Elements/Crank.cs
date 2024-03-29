using System;
using System.IO;
using System.Runtime.CompilerServices;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osuTK;

namespace CrankItUp.Game.Elements
{
    public partial class Crank : CompositeDrawable
    {
        public Crank()
        {
            AutoSizeAxes = Axes.Both;
            Origin = Anchor.Centre;
        }

        private Container box;
        private CursorContainer mouse;
        public static Line CollisionLine;
        private string compositeString;
        public StreamWriter Writer;
        private double previousDirection;

        private Vector2 previousNearEnd,
                        previousFarEnd;

        private float dilation;

        private double crankScaledHeight,
                       crankScaledLength;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, Storage storage)
        {
            mouse = new CursorContainer();
            mouse.ToggleVisibility();
            compositeString = "";
            dilation = (float)(NoteManager.Radius / Constants.NOTE_DEFAULT_RADIUS);
            crankScaledHeight = Constants.CRANK_DEFAULT_HEIGHT * dilation;
            crankScaledLength = Constants.CRANK_DEFAULT_LENGTH * dilation;

            Writer = new StreamWriter(storage.CreateFileSafely("./debug.txt"));
            previousNearEnd = new Vector2(812, (float)crankScaledLength);
            previousFarEnd = new Vector2(813, -(float)crankScaledLength);

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

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected override void Update()
        {
            double direction = box.Rotation * (Math.PI / 180) - Math.PI / 2.0;
            double dtheta = direction - previousDirection;

            // compositeString += previousNearEnd.ToString() + "\n";
            // compositeString += previousFarEnd.ToString() + "\n";

            // previousFarEnd = previousFarEnd - Constants.CORNER_TO_CENTER_TRANSFORMATION;
            // previousNearEnd = previousNearEnd - Constants.CORNER_TO_CENTER_TRANSFORMATION;

            compositeString += previousNearEnd.ToString() + "\n";
            compositeString += previousFarEnd.ToString() + "\n";

            Vector2 farEnd = new Vector2(
                (float)(previousFarEnd.X * Math.Cos(dtheta) + previousFarEnd.Y * Math.Sin(dtheta)),
                (float)(-previousFarEnd.X * Math.Sin(dtheta) + previousFarEnd.Y * Math.Cos(dtheta))
            );
            Vector2 nearEnd = new Vector2(
                (float)(
                    previousNearEnd.X * Math.Cos(dtheta) + previousNearEnd.Y * Math.Sin(dtheta)
                ),
                (float)(
                    -previousNearEnd.X * Math.Sin(dtheta) + previousNearEnd.Y * Math.Cos(dtheta)
                )
            );
            compositeString += nearEnd.ToString() + "\n";
            compositeString += farEnd.ToString() + "\n";

            // nearEnd += Constants.CORNER_TO_CENTER_TRANSFORMATION;
            // farEnd += Constants.CORNER_TO_CENTER_TRANSFORMATION;

            // compositeString += nearEnd.ToString() + "\n";
            // compositeString += farEnd.ToString() + "\n";

            CollisionLine = new Line(new Point(nearEnd), new Point(farEnd));
            compositeString = CollisionLine.ToStringFull();
            Writer.WriteLine(compositeString);

            previousDirection = direction;
            previousFarEnd = farEnd;
            previousNearEnd = nearEnd;
            base.Update();
        }

        public void UpdateRotation(Vector2 mousePos)
        {
            box.Rotation = (float)((180 / Math.PI) * Math.Atan2(mousePos.Y, mousePos.X));
        }
    }
}
