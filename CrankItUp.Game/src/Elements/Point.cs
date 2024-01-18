using osuTK;

namespace CrankItUp.Game.Elements
{
    public class Point
    {
        //essentially going to be a nullable wrapper for Vector2, to be used in the Line class
        private readonly float x;

        private readonly float y;

        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Point(Vector2 point)
        {
            this.x = point.X;
            this.y = point.Y;
        }

        public float GetX()
        {
            return x;
        }

        public float GetY()
        {
            return y;
        }

        public override string ToString()
        {
            return "(" + x + "," + y + ")";
        }
    }
}
