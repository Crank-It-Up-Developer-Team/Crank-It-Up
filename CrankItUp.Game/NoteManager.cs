using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using System;
using System.Collections.Generic;

namespace CrankItUp.Game
{
    public class NoteManager : CompositeDrawable
    {
        Queue<BaseNote> notes;
        long elapsedTime;
        BaseNote nextNote;
        public static double radius,
            approachRate; //mapping parameter AR is gonna be in pixels/second
        public float dilation;

        LevelScreen screen;

        public NoteManager(LevelScreen screen, double setRadius, double setApproachRate)
        {
            radius = setRadius;
            approachRate = setApproachRate;
            this.screen = screen;
        }

        Boolean stopSpawning;

        protected override void LoadComplete()
        {
            notes = new Queue<BaseNote>();
            dilation = (float)(radius / Constants.NOTE_DEFAULT_RADIUS);
            notes.Enqueue(new BaseNote(Math.PI / 4.0, 500));
            elapsedTime = 0;
            nextNote = notes.Dequeue();
            stopSpawning = false;
        }

        protected override void Update()
        {
            elapsedTime += (long)Time.Elapsed;
            if (nextNote.getSpawnTime() < elapsedTime && !stopSpawning)
            {
                //determine which note it is by using switch case chain and subclass parking
                //default case is base note so that is impl I am going with
                RemoveInternal(nextNote, false);
                LoadComponent(nextNote);
                screen.addNote(nextNote);
                nextNote.spawn();
                nextNote.ScaleTo(dilation, 0);
                nextNote.TransformTo(
                    "Position",
                    Constants.NOTE_DESTINATION,
                    nextNote.getTravelTime()
                );
                if (notes.Count == 0)
                {
                    stopSpawning = true;
                }
                else
                {
                    nextNote = notes.Dequeue();
                }
            }

            base.Update();
        }
    }
}
