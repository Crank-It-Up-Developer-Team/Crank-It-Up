using System.IO;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using System.Collections.Generic;
using osu.Framework.Audio.Track;
using osu.Framework.Audio;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Logging;

namespace CrankItUp.Game
{
    public class Beatmap
    {
        private JObject beatmap;
        public Track track;
        public readonly double noteRadius;
        public readonly double approachRate;
        public readonly double endTime;
        public readonly double startTime;
        private readonly Storage mapStorage;
        private readonly TrackMetadata trackmeta;

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
            mapStorage = storage.GetStorageForDirectory("maps").GetStorageForDirectory(mapname);
            try
            {
                // Open the text file using a stream reader.
                using (
                    var sr = new StreamReader(
                        mapStorage.GetStream(difficulty + ".json", mode: FileMode.Open)
                    )
                )
                {
                    // Read the stream as a string, and write the string to the console.
                    beatmap = JObject.Parse(sr.ReadToEnd());
                }
            }
            catch (IOException e)
            {
                Logger.Error(e, "Failed to read the beatmap json!");
            }

            ITrackStore trackStore = audio.GetTrackStore(
                new StorageBackedResourceStore(mapStorage)
            );
            trackmeta = new TrackMetadata(mapStorage.GetStream("metadata.json"));
            track = trackStore.Get(trackmeta.trackFilename);

            JToken meta = beatmap.GetValueOrFail("meta");
            noteRadius = meta.GetValueOrFail<double>("radius");
            approachRate = meta.GetValueOrFail<double>("approachRate");
            endTime = meta.GetValueOrFail<double>("endTime");
            // for some reason, GetValue<double> returns 0 if the value is not found
            if (endTime == 0)
            {
                // this is cursed, but .Length does not seem to work unless the song has been started
                track.Start();
                track.Stop(); // I know we might have skipped a ms or two of music this way, but we seek back later
                endTime = track.Length;
                Logger.LogPrint(
                    "Map does not specify an endTime! Falling back to the song length."
                );
            }
            // in this case, the default value of 0 is fine
            startTime = meta.GetValue<double>("startTime");
            track.Seek(startTime);
        }

        public Queue<BaseNote> GetBaseNoteQueue()
        {
            Queue<BaseNote> noteQueue = new Queue<BaseNote>();

            foreach (JToken noteobject in beatmap.GetValue("noteQueue"))
            {
                switch (noteobject.GetValue<string>("noteType"))
                {
                    case "Standard":
                        noteQueue.Enqueue(
                            new BaseNote(
                                noteobject.GetValue<float>("position"),
                                noteobject.GetValue<int>("spawnTime")
                            )
                        );
                        break;
                    case "Special":
                        Logger.Log(
                            "Map tried to spawn a special note, which is not yet implemented"
                        );
                        break;
                    default:
                        Logger.Log("Unknown note type!");
                        break;
                }
            }
            return noteQueue;
        }
    }
}
