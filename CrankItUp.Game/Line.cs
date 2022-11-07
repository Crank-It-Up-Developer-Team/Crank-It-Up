using System;
namespace CrankItUp.Game{
    public class Line{
        private Point point1, point2;
        private float slope, intercept;
        private SlopeState slopeState;

        public enum SlopeState{
            NON_ZERO, ZERO, DNE
        }

        public Line(Point p1, Point p2){
            point1 = p1;
            point2 = p2;
            
            if( p1.getX() - p2.getX() == 0){ //x=a, unlikely but should be accounted for
                slopeState = SlopeState.DNE; 
                intercept = p1.getX();
            }else{
                slope = (p2.getY() - p1.getY()) / (p2.getX() - p1.getX());
                intercept = p1.getY() - slope * p1.getX();
                if(slope == 0){
                    slopeState = SlopeState.ZERO;
                }else{
                    slopeState = SlopeState.NON_ZERO;
                }
            }
            
        }

        /**
        returns the point of intersections of 2 lines
        returns null if no solution exists 
        **/
        public Point intersection(Line other){
            if(slopeState == SlopeState.DNE || other.getSlopeState() == SlopeState.DNE){
                if(slopeState == SlopeState.DNE){
                    if(Math.Min(other.getPoint1().getX(), other.getPoint2().getX()) < intercept && 
                    intercept < Math.Max(other.getPoint1().getX(), other.getPoint2().getX())){
                         Point intersection = new Point(getIntercept(), other.evaluate(other.getIntercept()));
                        if(existsOnLineSegment(intersection) && other.existsOnLineSegment(intersection)){
                            return intersection;
                        }else{
                            return null;
                        }
                    }else{
                        return null;
                    }
                }else {
                    if(Math.Min(getPoint1().getX(), getPoint2().getX()) < other.getIntercept() && 
                    other.getIntercept() < Math.Max(getPoint1().getX(), getPoint2().getX())){
                        Point intersection = new Point(other.getIntercept(), evaluate(other.getIntercept()));
                        if(existsOnLineSegment(intersection) && other.existsOnLineSegment(intersection)){
                            return intersection;
                        }else{
                            return null;
                        }
                        
                    }else{
                        return null;
                    }
                }
            }else {
                if(slope - other.getSlope() == 0){
                    return null; //TODO handle an edge case where we have infinite solutions show up, this could cause noclipping or crashing depending on how the logic handles this 
                }
                float x = (other.getIntercept() - intercept) / (slope - other.getSlope());
                if(evaluate(x) == other.evaluate(x)){
                     Point intersection = new Point(x, evaluate(x));;
                        if(existsOnLineSegment(intersection) && other.existsOnLineSegment(intersection)){
                            return intersection;
                        }else{
                            return null;
                        }
                }
            }
            return null;
        }


        public Boolean existsOnLineSegment(Point p){            
            return Math.Min(getPoint1().getX(), getPoint2().getX()) < p.getX() && Math.Max(getPoint1().getX(), getPoint2().getX()) > p.getX() &&
            Math.Min(getPoint1().getY(), getPoint2().getY()) < p.getY() && Math.Max(getPoint1().getY(), getPoint2().getY()) > p.getY();
        }


        public float evaluate(float x){
            return slope * x + intercept;
        }

        public float getSlope(){
            return slope;
        }
        public float getIntercept(){
            return intercept;
        }
        public SlopeState getSlopeState(){
            return slopeState;
        }
        public Point getPoint1(){
            return point1;
        }
        public Point getPoint2(){
            return point2;
        }

        public String toString(){
            return "y = " + slope + "x" + " + " + intercept;
        }

        //for outputting line segments to desmos
        public string toStringFull(){
            return toString() + "\\left\\{"+ Math.Min(point1.getX(), point2.getX()) + "< x < " + Math.Max(point1.getX(), point2.getX()) + "\\right\\}";
        }
    }
}