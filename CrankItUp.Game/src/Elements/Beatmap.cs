using System.IO;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using System.Collections.Generic;
using System;
using osu.Framework.Audio.Track;
using osu.Framework.Audio;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;

namespace CrankItUp.Game
{
    public class Beatmap
    {
        private JObject beatmap;
        public Track track;
        public readonly double noteRadius;
        public readonly double approachRate;
        public readonly double endTime;

        /// <summary>Creates a new instance of the Beatmap class.</summary>
        /// <param name="mapname">The name of the map.</param>
        /// <param name="difficulty">The difficulty level of the map.</param>
        /// <param name="audio">An instance of the AudioManager class.</param>
        /// <param name="storage">An instance of the Storage class.</param>
        /// <remarks>
        /// This constructor initializes a Beatmap object by reading the map data from a JSON file.
        /// The map data is located in the "maps" directory, under the specified mapname and difficulty.
        /// The audio and storage objects are used to retrieve additional information and resources.
        /// If the map does not specify an endTime, the song length will be used
        /// </remarks>
        public Beatmap(string mapname, string difficulty, AudioManager audio, Storage storage)
        {
            try
            {
                // Open the text file using a stream reader.
                using (
                    var sr = new StreamReader(Path.Combine("maps", mapname, difficulty + ".json"))
                )
                {
                    // Read the stream as a string, and write the string to the console.
                    beatmap = JObject.Parse(sr.ReadToEnd());
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            var trackStore = audio.GetTrackStore(
                new StorageBackedResourceStore(
                    storage.GetStorageForDirectory(
                        Path.Combine(Constants.APPDATA_DIR, "maps", mapname)
                    )
                )
            );
            track = trackStore.Get("music.mp3");

            JToken meta = beatmap.GetValue("meta");
            noteRadius = meta.GetValue<double>("radius");
            approachRate = meta.GetValue<double>("approachRate");
            try
            {
                endTime = meta.GetValue<double>("endTime");
            }
            catch
            {
                Console.WriteLine(
                    "Map does not specify an endTime! Falling back to the song length."
                );
                endTime = track.Length;
            }
        }

        public Queue<BaseNote> GetBaseNoteQueue()
        {
            Queue<BaseNote> noteQueue = new Queue<BaseNote>();

            foreach (JToken noteobject in beatmap.GetValue("BaseNoteQueue"))
            {
                noteQueue.Enqueue(
                    new BaseNote(
                        noteobject.GetValue<float>("position"),
                        noteobject.GetValue<int>("spawnTime")
                    )
                );
            }
            return noteQueue;
        }
    }
}
