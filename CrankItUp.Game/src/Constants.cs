using osuTK;

namespace CrankItUp.Game
{
    public static class Constants
    {
        //any project wide constants go here
        public static readonly Vector2 CORNER_TO_CENTER_TRANSFORMATION = new Vector2(740, 375);
        public static readonly Vector2 NOTE_DESTINATION = new Vector2(740, 375);
        public static readonly double NOTE_DEFAULT_RADIUS = 74; //pixels

        public static readonly double CRANK_DEFAULT_LENGTH = 337;
        public static readonly double CRANK_DEFAULT_HEIGHT = 70;

        public static readonly int MAP_DATAVERSION = 1;
        public static readonly int METADATA_DATAVERSION = 1;
    }
}
