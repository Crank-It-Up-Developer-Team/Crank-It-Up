using osuTK;
namespace CrankItUp.Game{
    public class Point{
        //essentially going to be a nullable wrapper for Vector2, to be used in the Line class
        private float x, y;
        public Point(float x, float y){
            this.x = x;
            this.y = y;
        }
        public Point(Vector2 point){
            this.x = point.X;
            this.y = point.Y;
        }

        public float getX(){
            return x;
        }
        public float getY(){
            return y;
        }

        public string toString(){
            return "(" + x + "," + y + ")";
        }
    }
}