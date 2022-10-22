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
using osu.Framework.Physics;

namespace CrankItUp.Game
{
    public class BaseNote : CompositeDrawable
    {
        /**
        so my thoughts on how mapping should be implemented
        we have a ring where notes can be placed of all the potential spots for the platter to
        be (may be a bit complicated because mapping a circle to a non circlular object, we can probably
        make a ring of some thickness tho to account for that if my mental math is right)
        this also makes it so all we have to do to instantiate any given note is pass in its radial direction and then use a
        vector of some static magnitude to be able to project it onto the spawning circle
        **/

        private double radians;
        private long spawnTime;
        private long travelTime;
        Sprite note;

        Vector2 velocity;
        Boolean firstCollision;

        private static float PROJECTION_VECTOR_MAGNITUDE = 375; //pixels

        public BaseNote(double radians, long spawnTime)
        {
            this.radians = radians;
            this.spawnTime = spawnTime;
            firstCollision = true;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            InternalChild = new Container
            {
                AutoSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    new Circle
                    {
                        RelativeSizeAxes = Axes.Both,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    },
                    note = new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Texture = textures.Get("BaseNote"),
                        Alpha = 1f,
                    },
                }
            };
        }

        protected override void Update()
        {
            Vector2 centerPosition = new Vector2(
                (float)(X + NoteManager.radius),
                (float)(Y + NoteManager.radius)
            );
            double leftStartAngle = radians - Math.PI / 2.0;
            Vector2 leftTangent = new Vector2(
                centerPosition.X - (float)(NoteManager.radius * Math.Cos(leftStartAngle)),
                centerPosition.Y - (float)(NoteManager.radius * Math.Sin(leftStartAngle))
            );
            Vector2 leftWaypoint = new Vector2(
                leftTangent.X - (float)(NoteManager.radius * Math.Cos(radians)),
                leftTangent.Y - (float)(NoteManager.radius * Math.Sin(radians))
            );
            double rightStartAngle = radians + Math.PI / 2.0;
            Vector2 rightTangent = new Vector2(
                centerPosition.X - (float)(NoteManager.radius * Math.Cos(rightStartAngle)),
                centerPosition.Y - (float)(NoteManager.radius * Math.Sin(rightStartAngle))
            );
            Vector2 rightWaypoint = new Vector2(
                rightTangent.X - (float)(NoteManager.radius * Math.Cos(radians)),
                rightTangent.Y - (float)(NoteManager.radius * Math.Sin(radians))
            );
            Line collision = new Line(new Point(rightWaypoint), new Point(leftWaypoint));

            if (collision.intersection(Crank.collisionLine) != null)
            {
                //collision has happened, we do some math to determine the accuracy
                ClearTransforms();
                if (firstCollision)
                {
                    Console.WriteLine(Crank.collisionLine.toString());
                    Console.WriteLine(Crank.collisionLine.getPoint2().toString());
                    Console.WriteLine(Crank.collisionLine.getPoint1().toString());
                    Console.WriteLine("");
                    Console.WriteLine(collision.toString());
                    Console.WriteLine(collision.getPoint2().toString());
                    Console.WriteLine(collision.getPoint1().toString());
                    Console.WriteLine(collision.intersection(Crank.collisionLine).toString());
                    firstCollision = false;
                }
            }
        }

        public void spawn()
        {
            double finalMagnitude = PROJECTION_VECTOR_MAGNITUDE - NoteManager.radius;
            Vector2 spawnPointCenteredCoordinates = new Vector2(
                (float)(finalMagnitude * Math.Cos(-radians)),
                (float)(finalMagnitude * Math.Sin(-radians))
            );
            Position = spawnPointCenteredCoordinates + Constants.NOTE_DESTINATION;
            velocity = new Vector2(
                (float)(NoteManager.approachRate * Math.Cos(radians)),
                (float)(NoteManager.approachRate * Math.Sin(radians))
            );
            double timeSeconds = finalMagnitude / NoteManager.approachRate;
            travelTime = (long)(timeSeconds * 1000);
        }

        public long getTravelTime()
        {
            return travelTime;
        }

        public long getSpawnTime()
        {
            return spawnTime;
        }
    }
}
