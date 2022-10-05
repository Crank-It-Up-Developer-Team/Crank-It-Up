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


namespace CrankItUp.Game{
    public class BaseNote: RigidBodySimulation{
        
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
        Sprite note;
        
        private static float PROJECTION_VECTOR_MAGNITUDE = 375; //pixels
        public BaseNote(double radians, long spawnTime){
            AutoSizeAxes = Axes.Both;
            this.radians = radians;
            this.spawnTime = spawnTime;

        }
        private void load(TextureStore textures){
               InternalChild = new Container
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
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
                        Alpha = 0f,
                    },
               }

            };
        }
        

        public void spawn(){
            double finalMagnitude = PROJECTION_VECTOR_MAGNITUDE - NoteManager.radius;
            Vector2 spawnPointCenteredCoordinates = new Vector2((float)(finalMagnitude * Math.Cos(radians)),(float)(finalMagnitude * Math.Sin(radians)));
            Position = spawnPointCenteredCoordinates + Constants.CORNER_TO_CENTER_TRANSFORMATION;
            Velocity = new Vector2((float)(NoteManager.approachRate * Math.Cos(radians)),(float)(NoteManager.approachRate * Math.Sin(radians)));
            note.Alpha = 1f;
        }

        public long getSpawnTime(){
            return spawnTime;
        }
        
    }
}