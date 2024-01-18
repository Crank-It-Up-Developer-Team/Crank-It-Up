using System.Collections.Generic;
using CrankItUp.Game.Screens;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Logging;
using osu.Framework.Screens;

namespace CrankItUp.Game.Elements
{
    public partial class NoteManager : CompositeDrawable
    {
        private Queue<BaseNote> notes;
        private readonly Beatmap beatmap;
        private BaseNote nextNote;

        public static double Radius,
                             ApproachRate; //mapping parameter AR is gonna be in pixels/second

        public float Dilation;

        private readonly LevelScreen screen;

        public NoteManager(LevelScreen screen, Beatmap map)
        {
            this.screen = screen;
            beatmap = map;
            Radius = beatmap.NoteRadius;
            ApproachRate = beatmap.ApproachRate;
        }

        private bool stopSpawning;

        protected override void LoadComplete()
        {
            notes = beatmap.GetBaseNoteQueue();
            Dilation = (float)(Radius / Constants.NOTE_DEFAULT_RADIUS);

            try
            {
                nextNote = notes.Dequeue();
                stopSpawning = false;
            }
            catch
            {
                Logger.Log("Level has no notes?");
                nextNote = new BaseNote(0, 0);
                stopSpawning = true;
            }
        }

        protected override void Update()
        {
            if (nextNote.GetSpawnTime() <= beatmap.Track.CurrentTime && !stopSpawning)
            {
                //determine which note it is by using switch case chain and subclass parking
                //default case is base note so that is impl I am going with
                RemoveInternal(nextNote, false);
                LoadComponent(nextNote);
                screen.AddNote(nextNote);
                nextNote.Spawn();
                nextNote.ScaleTo(Dilation, 0);
                nextNote.TransformTo(
                    "Position",
                    Constants.NOTE_DESTINATION,
                    nextNote.GetTravelTime()
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

            if (beatmap.Track.CurrentTime >= beatmap.EndTime)
            {
                beatmap.Track.Dispose();

                if (screen.IsCurrentScreen())
                {
                    beatmap.Track.Dispose();
                    screen.Exit();
                }
            }

            base.Update();
        }
    }
}
