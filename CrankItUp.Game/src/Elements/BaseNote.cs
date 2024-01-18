using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace CrankItUp.Game.Elements
{
    public partial class BaseNote : CompositeDrawable
    {
        /**
        so my thoughts on how mapping should be implemented
        we have a ring where notes can be placed of all the potential spots for the platter to
        be (may be a bit complicated because mapping a circle to a non circular object, we can probably
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

        private static readonly float projection_vector_magnitude = 375; //pixels

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
                (float)(X + NoteManager.Radius),
                (float)(Y + NoteManager.Radius)
            );
            double leftStartAngle = radians - Math.PI / 2.0;
            Vector2 leftTangent = new Vector2(
                centerPosition.X - (float)(NoteManager.Radius * Math.Cos(leftStartAngle)),
                centerPosition.Y - (float)(NoteManager.Radius * Math.Sin(leftStartAngle))
            );
            Vector2 leftWaypoint = new Vector2(
                leftTangent.X - (float)(NoteManager.Radius * Math.Cos(radians)),
                leftTangent.Y - (float)(NoteManager.Radius * Math.Sin(radians))
            );
            double rightStartAngle = radians + Math.PI / 2.0;
            Vector2 rightTangent = new Vector2(
                centerPosition.X - (float)(NoteManager.Radius * Math.Cos(rightStartAngle)),
                centerPosition.Y - (float)(NoteManager.Radius * Math.Sin(rightStartAngle))
            );
            Vector2 rightWaypoint = new Vector2(
                rightTangent.X - (float)(NoteManager.Radius * Math.Cos(radians)),
                rightTangent.Y - (float)(NoteManager.Radius * Math.Sin(radians))
            );
            Line collision = new Line(new Point(rightWaypoint), new Point(leftWaypoint));

            if (collision.Intersection(Crank.CollisionLine) != null)
            {
                //collision has happened, we do some math to determine the accuracy
                ClearTransforms();

                if (firstCollision)
                {
                    Console.WriteLine(Crank.CollisionLine.ToString());
                    Console.WriteLine(Crank.CollisionLine.GetPoint2().ToString());
                    Console.WriteLine(Crank.CollisionLine.GetPoint1().ToString());
                    Console.WriteLine("");
                    Console.WriteLine(collision.ToString());
                    Console.WriteLine(collision.GetPoint2().ToString());
                    Console.WriteLine(collision.GetPoint1().ToString());
                    Console.WriteLine(collision.Intersection(Crank.CollisionLine).ToString());
                    firstCollision = false;
                }
            }
        }

        public void Spawn()
        {
            double finalMagnitude = projection_vector_magnitude - NoteManager.Radius;
            Vector2 spawnPointCenteredCoordinates = new Vector2(
                (float)(finalMagnitude * Math.Cos(-radians)),
                (float)(finalMagnitude * Math.Sin(-radians))
            );
            Position = spawnPointCenteredCoordinates + Constants.NOTE_DESTINATION;
            velocity = new Vector2(
                (float)(NoteManager.ApproachRate * Math.Cos(radians)),
                (float)(NoteManager.ApproachRate * Math.Sin(radians))
            );
            double timeSeconds = finalMagnitude / NoteManager.ApproachRate;
            travelTime = (long)(timeSeconds * 1000);
        }

        public long GetTravelTime()
        {
            return travelTime;
        }

        public long GetSpawnTime()
        {
            return spawnTime;
        }
    }
}
