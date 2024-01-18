using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;

namespace CrankItUp.Game.Elements
{
    public class Beatmap
    {
        private readonly JObject beatmap;
        public Track Track;
        public readonly double NoteRadius;
        public readonly double ApproachRate;
        public readonly double EndTime;
        public readonly double StartTime;

        /// <summary>Creates a new instance of the Beatmap class.</summary>
        /// <param name="mapName">The name of the map.</param>
        /// <param name="difficulty">The difficulty level of the map.</param>
        /// <param name="audio">An instance of the AudioManager class.</param>
        /// <param name="storage">An instance of the Storage class.</param>
        /// <remarks>
        /// This constructor initializes a Beatmap object by reading the map data from a JSON file.
        /// The map data is located in the "maps" directory, under the specified mapName and difficulty.
        /// The audio and storage objects are used to retrieve additional information and resources.
        /// If the map does not specify an endTime, the song length will be used
        /// </remarks>
        public Beatmap(string mapName, string difficulty, AudioManager audio, Storage storage)
        {
            var mapStorage = storage.GetStorageForDirectory("maps").GetStorageForDirectory(mapName);

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
            var trackMeta = new TrackMetadata(mapStorage.GetStream("metadata.json"));
            Track = trackStore.Get(trackMeta.TrackFilename);

            JToken meta = beatmap.GetValueOrFail("meta");
            NoteRadius = meta.GetValueOrFail<double>("radius");
            ApproachRate = meta.GetValueOrFail<double>("approachRate");
            // optional value
            EndTime = meta.GetValue<double>("endTime");
            // for some reason, GetValue<double> returns 0 if the value is not found

            if (EndTime == 0)
            {
                // this is cursed, but .Length does not seem to work unless the song has been started
                Track.Start();
                Track.Stop(); // I know we might have skipped a ms or two of music this way, but we seek back later
                EndTime = Track.Length;
                Logger.LogPrint(
                    "Map does not specify an endTime! Falling back to the song length."
                );
            }

            // in this case, the default value of 0 is fine
            StartTime = meta.GetValue<double>("startTime");
            Track.Seek(StartTime);
        }

        public Queue<BaseNote> GetBaseNoteQueue()
        {
            Queue<BaseNote> noteQueue = new Queue<BaseNote>();

            foreach (JToken noteObject in beatmap.GetValueOrFail("noteQueue"))
            {
                switch (noteObject.GetValue<string>("noteType"))
                {
                    case "Standard":
                        noteQueue.Enqueue(
                            new BaseNote(
                                noteObject.GetValue<float>("position"),
                                noteObject.GetValue<int>("spawnTime")
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
