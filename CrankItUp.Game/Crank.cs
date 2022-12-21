using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Cursor;
using osuTK;
using System;
using System.IO;
using System.Runtime.CompilerServices;
<<<<<<< Updated upstream
=======
using osu.Framework.Input.Events;
using System.Collections.Generic;
using osu.Framework.Physics;
>>>>>>> Stashed changes


namespace CrankItUp.Game
{
    public class Crank : RigidBodyContainer<CompositeDrawable>
    {
        Queue<BaseNote> activeNotes;
        public Crank()
        {
            activeNotes = new Queue<BaseNote>();
            
            Position = Constants.CORNER_TO_CENTER_TRANSFORMATION;
        }

        private Container box;
        private CursorContainer mouse;
        public static Line collisionLine;
        StreamWriter writer;
        double previousDirection = 0;
<<<<<<< Updated upstream
        Vector2 previousNearEnd,
            previousFarEnd;
        float dilation;
        double crankScaledHeight,
            crankScaledLength;
=======
        float dilation;
        
>>>>>>> Stashed changes

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
<<<<<<< Updated upstream
=======
            AutoSizeAxes = Axes.Both;
>>>>>>> Stashed changes
            mouse = new CursorContainer();
            mouse.ToggleVisibility();
            dilation = (float)(NoteManager.radius / Constants.NOTE_DEFAULT_RADIUS);
<<<<<<< Updated upstream
            crankScaledHeight = Constants.CRANK_DEFAULT_HEIGHT * dilation;
            crankScaledLength = Constants.CRANK_DEFAULT_LENGTH * dilation;

            writer = new StreamWriter("./debug.txt");
            previousNearEnd = new Vector2(812, (float)crankScaledLength);
            previousFarEnd = new Vector2(813, -(float)crankScaledLength);
=======
            
            //writer = new StreamWriter("C:\\Users\\fizzi\\Documents\\GitHub\\Crank-It-Up\\CrankItUp.Game\\debug.txt"); //you 
>>>>>>> Stashed changes

            InternalChild = box = new Container
            {
                
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected override void Update()
        {
<<<<<<< Updated upstream
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

            collisionLine = new Line(new Point(nearEnd), new Point(farEnd));
            compositeString = collisionLine.toStringFull();
            writer.WriteLine(compositeString);

            previousDirection = direction;
            previousFarEnd = farEnd;
            previousNearEnd = nearEnd;
=======
            if(activeNotes.Count != 0){
                Console.WriteLine(activeNotes.Peek().Position.ToString());
                Console.WriteLine(Position.ToString());
                if(CheckAndHandleCollisionWith(activeNotes.Peek())){
                    activeNotes.Dequeue();
                    Console.WriteLine("Hit!");

                }
            }
>>>>>>> Stashed changes
            base.Update();
        }

<<<<<<< Updated upstream
        public void updateRotation(Vector2 MousePos)
        {
=======
        public void addNote(BaseNote note){
            activeNotes.Enqueue(note);
        }

        

        public void updateRotation(Vector2 MousePos){
>>>>>>> Stashed changes
            box.Rotation = (float)((180 / Math.PI) * Math.Atan2(MousePos.Y, MousePos.X));
        }
    }
}
