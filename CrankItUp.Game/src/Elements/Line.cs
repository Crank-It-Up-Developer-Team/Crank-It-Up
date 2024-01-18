using System;

namespace CrankItUp.Game.Elements
{
    public class Line
    {
        private readonly Point point1;

        private readonly Point point2;

        private readonly float slope;

        private readonly float intercept;

        private readonly SlopeState slopeState;

        public enum SlopeState
        {
            NON_ZERO,
            ZERO,
            DNE
        }

        public Line(Point p1, Point p2)
        {
            point1 = p1;
            point2 = p2;

            if (p1.GetX() - p2.GetX() == 0)
            {
                //x=a, unlikely but should be accounted for
                slopeState = SlopeState.DNE;
                intercept = p1.GetX();
            }
            else
            {
                slope = (p2.GetY() - p1.GetY()) / (p2.GetX() - p1.GetX());
                intercept = p1.GetY() - slope * p1.GetX();

                if (slope == 0)
                {
                    slopeState = SlopeState.ZERO;
                }
                else
                {
                    slopeState = SlopeState.NON_ZERO;
                }
            }
        }

        /**
        returns the point of intersections of 2 lines
        returns null if no solution exists
        **/
        public Point Intersection(Line other)
        {
            if (slopeState == SlopeState.DNE || other.GetSlopeState() == SlopeState.DNE)
            {
                if (slopeState == SlopeState.DNE)
                {
                    if (
                        Math.Min(other.GetPoint1().GetX(), other.GetPoint2().GetX()) < intercept
                        && intercept < Math.Max(other.GetPoint1().GetX(), other.GetPoint2().GetX())
                    )
                    {
                        Point intersection = new Point(
                            GetIntercept(),
                            other.Evaluate(other.GetIntercept())
                        );

                        if (
                            existsOnLineSegment(intersection)
                            && other.existsOnLineSegment(intersection)
                        )
                        {
                            return intersection;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (
                        Math.Min(GetPoint1().GetX(), GetPoint2().GetX()) < other.GetIntercept()
                        && other.GetIntercept() < Math.Max(GetPoint1().GetX(), GetPoint2().GetX())
                    )
                    {
                        Point intersection = new Point(
                            other.GetIntercept(),
                            Evaluate(other.GetIntercept())
                        );

                        if (
                            existsOnLineSegment(intersection)
                            && other.existsOnLineSegment(intersection)
                        )
                        {
                            return intersection;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                if (slope - other.GetSlope() == 0)
                {
                    return null; //TODO handle an edge case where we have infinite solutions show up, this could cause noclipping or crashing depending on how the logic handles this
                }

                float x = (other.GetIntercept() - intercept) / (slope - other.GetSlope());

                if (Evaluate(x) == other.Evaluate(x))
                {
                    Point intersection = new Point(x, Evaluate(x));

                    if (
                        existsOnLineSegment(intersection) && other.existsOnLineSegment(intersection)
                    )
                    {
                        return intersection;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        public Boolean existsOnLineSegment(Point p)
        {
            return Math.Min(GetPoint1().GetX(), GetPoint2().GetX()) < p.GetX()
                   && Math.Max(GetPoint1().GetX(), GetPoint2().GetX()) > p.GetX()
                   && Math.Min(GetPoint1().GetY(), GetPoint2().GetY()) < p.GetY()
                   && Math.Max(GetPoint1().GetY(), GetPoint2().GetY()) > p.GetY();
        }

        public float Evaluate(float x)
        {
            return slope * x + intercept;
        }

        public float GetSlope()
        {
            return slope;
        }

        public float GetIntercept()
        {
            return intercept;
        }

        public SlopeState GetSlopeState()
        {
            return slopeState;
        }

        public Point GetPoint1()
        {
            return point1;
        }

        public Point GetPoint2()
        {
            return point2;
        }

        public override String ToString()
        {
            return "y = " + slope + "x" + " + " + intercept;
        }

        //for outputting line segments to desmos
        public string ToStringFull()
        {
            return ToString() + "\\left\\{"
                              + Math.Min(point1.GetX(), point2.GetX())
                              + "< x < "
                              + Math.Max(point1.GetX(), point2.GetX())
                              + "\\right\\}";
        }
    }
}
