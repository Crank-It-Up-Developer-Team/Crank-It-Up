using System;
using System.IO;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using osu.Framework.Logging;

namespace CrankItUp.Game
{
    public class TrackMetadata
    {
        public string name;
        public string description;
        public string mapperName;
        public string trackArtistName;
        public string trackFilename;
        public float trackPreviewStart;
        public string coverImageFilename;
        public string customBackgroundFilename;

        private JObject metadata;

        public TrackMetadata(Stream metaStream)
        {
            var reader = new StreamReader(metaStream);
            try
            {
                metadata = JObject.Parse(reader.ReadToEnd());
            }
            catch
            {
                var e = new InvalidDataException("Failed to read the metadata JSON!");
                Logger.Error(e, "Failed to read the metadata JSON!");
                throw e;
            }

            reader.Dispose();

            try
            {
                int dataVersion = metadata.GetValue<int>("dataVersion");
                if (dataVersion != Constants.METADATA_DATAVERSION)
                {
                    var e = new InvalidDataException(
                        "The metadata dataVersion is invalid for this version of the game!"
                    );
                    Logger.Error(
                        e,
                        "The metadata dataVersion is invalid for this version of the game!"
                    );
                    throw e;
                }
            }
            catch
            {
                var e = new InvalidDataException("Failed to read the metadata dataVersion!");
                Logger.Error(e, "Failed to read the metadata dataVersion!");
                throw e;
            }

            // required properties
            try
            {
                name = metadata.GetValueOrFail<string>("name");
                mapperName = metadata.GetValueOrFail<string>("mapperName");
                trackArtistName = metadata.GetValueOrFail<string>("trackArtistName");
                // TODO check if the file paths are valid
                trackFilename = metadata.GetValueOrFail<string>("trackFilename");
            }
            catch (Exception e)
            {
                Logger.Error(e, "Required attributes could not be read!");
                throw;
            }
            description = metadata.GetValue<string>("description");
            trackPreviewStart = metadata.GetValue<float>("trackPreviewStart");
            coverImageFilename = metadata.GetValue<string>("coverImageFilename");
            customBackgroundFilename = metadata.GetValue<string>("customBackgroundFilename");
        }
    }
}
