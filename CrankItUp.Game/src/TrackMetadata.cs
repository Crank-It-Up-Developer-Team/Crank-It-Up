using System;
using System.IO;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using osu.Framework.Logging;

namespace CrankItUp.Game
{
    public class TrackMetadata
    {
        public string Name;
        public string Description;
        public string MapperName;
        public string TrackArtistName;
        public string TrackFilename;
        public float TrackPreviewStart;
        public string CoverImageFilename;
        public string CustomBackgroundFilename;

        public TrackMetadata(Stream metaStream)
        {
            JObject metadata;
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
                Name = metadata.GetValueOrFail<string>("name");
                MapperName = metadata.GetValueOrFail<string>("mapperName");
                TrackArtistName = metadata.GetValueOrFail<string>("trackArtistName");
                // TODO check if the file paths are valid
                TrackFilename = metadata.GetValueOrFail<string>("trackFilename");
            }
            catch (Exception e)
            {
                Logger.Error(e, "Required attributes could not be read!");
                throw;
            }

            Description = metadata.GetValue<string>("description");
            TrackPreviewStart = metadata.GetValue<float>("trackPreviewStart");
            CoverImageFilename = metadata.GetValue<string>("coverImageFilename");
            CustomBackgroundFilename = metadata.GetValue<string>("customBackgroundFilename");
        }
    }
}
