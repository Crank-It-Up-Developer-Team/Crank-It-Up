using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Framework.Input.Events;
using osu.Framework.Audio;
using osu.Framework.Platform;
using osu.Framework.Audio.Track;
using osu.Framework.IO.Stores;
using System.IO;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osu.Framework.Graphics.Containers;

namespace CrankItUp.Game
{
    public partial class MappingScreen : Screen
    {
        string difficulty;
        private string trackFilename;
        Track track;
        JObject beatmap = new JObject();
        JArray NoteQueue = new JArray();
        Storage storage;

        public MappingScreen(string difficulty, string trackFilename)
        {
            this.difficulty = difficulty;
            this.trackFilename = trackFilename;
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio, Storage store)
        {
            storage = store;
            var trackStore = audio.GetTrackStore(
                new StorageBackedResourceStore(
                    storage.GetStorageForDirectory(Path.Combine("maps", "WIP"))
                )
            );

            InternalChildren = new Drawable[]
            {
                new DrawSizePreservingFillContainer
                {
                    new SpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Position = new Vector2(0, 0),
                        Text = "Tap ctrl, space or Z to add a note, press esc when you are done",
                    }
                }
            };

            track = trackStore.Get(trackFilename);
        }

        protected override void LoadComplete()
        {
            track.Start();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            // if we are adding a note
            if (
                e.Key == osuTK.Input.Key.ControlLeft
                | e.Key == osuTK.Input.Key.Z
                | e.Key == osuTK.Input.Key.Space
            )
            {
                JObject note = new JObject
                {
                    { "noteType", "Standard" },
                    { "position", 0 },
                    { "spawnTime", track.CurrentTime }
                };
                NoteQueue.Add(note);
            }
            // if the user wishes to exit
            else if (e.Key == osuTK.Input.Key.Escape)
            {
                JObject meta = new JObject
                {
                    // use default values for now
                    { "dataVersion", "1" },
                    { "radius", 50 },
                    { "approachRate", 100 },
                    // set the current time as the endTime
                    { "endTime", track.CurrentTime },
                    { "startTime", 0 }
                };
                // add everything to the beatmap
                beatmap.Add("meta", meta);
                beatmap.Add("noteQueue", NoteQueue);
                // save it to disk
                StreamWriter mapfile = new StreamWriter(
                    storage.CreateFileSafely(Path.Combine("maps", "WIP", difficulty + ".json"))
                );
                mapfile.Write(beatmap.ToJson(Newtonsoft.Json.Formatting.Indented));
                mapfile.Dispose();
                // clean up and exit
                track.Stop();
                this.Exit();
            }
            return base.OnKeyDown(e);
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            this.FadeInFromZero(500, Easing.OutQuint);
        }
    }
}
