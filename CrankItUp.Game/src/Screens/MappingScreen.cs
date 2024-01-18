using System.IO;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;

namespace CrankItUp.Game.Screens
{
    public partial class MappingScreen : Screen
    {
        private readonly string difficulty;
        private readonly string trackFilename;
        private Track track;
        private readonly JObject beatmap = new JObject();
        private readonly JArray noteQueue = new JArray();
        private Storage storage;

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
                noteQueue.Add(note);
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
                beatmap.Add("noteQueue", noteQueue);
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
