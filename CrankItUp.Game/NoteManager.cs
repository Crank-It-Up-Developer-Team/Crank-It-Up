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
using System.Collections.Generic;

namespace CrankItUp.Game{
    public class NoteManager: CompositeComponent{

        Queue<BaseNote> notes;
        long elapsedTime;
        BaseNote nextNote;
        public static double radius, approachRate; //mapping parameter

        private void load(){

            notes = new Queue<BaseNote>();
            
        }

        protected override void LoadComplete(){
            elapsedTime = 0;
            nextNote = notes.Dequeue();
        }

          protected override void Update()
        {
            elapsedTime += (long)Time.Elapsed;
            if(nextNote.getSpawnTime() > elapsedTime){
                //determine which note it is by using switch case chain and subclass parking
                //default case is base note so that is impl I am going with
                nextNote.spawn();
            }
            base.Update();
        }
    
    }
}