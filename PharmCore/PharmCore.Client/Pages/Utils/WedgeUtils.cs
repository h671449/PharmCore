namespace PharmCore.Client.Pages.Utils;

public struct CornerResult
{
    // Where the first segment ends after trimming
    public (double x, double y) PrevEnd;
    // Where the second segment starts after trimming
    public (double x, double y) NextStart;
    // The arc command that connects PrevEnd -> NextStart
    public string ArcSvgCommand;
}

//------------------------------------------------------------------------
// Interfaces for line/arc segments so we can unify "shorten from start/end" etc.
//------------------------------------------------------------------------

public interface ISegment
{
    (double x, double y) StartPoint();
    (double x, double y) EndPoint();

    /// <summary> Returns a point that is 'dist' away from the start of this segment along its length. </summary>
    (double x, double y) PointAtDistanceFromStart(double dist);

    /// <summary> Returns a point that is 'dist' away from the end of this segment along its length. </summary>
    (double x, double y) PointAtDistanceFromEnd(double dist);

    /// <summary> Tangent (unit vector) at the start of the segment. </summary>
    (double x, double y) TangentAtStart();

    /// <summary> Tangent (unit vector) at the end of the segment. </summary>
    (double x, double y) TangentAtEnd();
}

//------------------------------------------------------------------------
// Line segment
//------------------------------------------------------------------------

public class LineSegment : ISegment
{
    private (double x, double y) p1, p2;
    private double length;
    private (double x, double y) dir; // unit direction p1->p2

    public LineSegment(double x1, double y1, double x2, double y2)
    {
        p1 = (x1, y1);
        p2 = (x2, y2);
        double dx = x2 - x1;
        double dy = y2 - y1;
        length = Math.Sqrt(dx * dx + dy * dy);
        if (length < 1e-9) length = 1e-9; // avoid div by zero
        dir = (dx / length, dy / length);
    }

    public (double x, double y) StartPoint() => p1;
    public (double x, double y) EndPoint() => p2;

    public (double x, double y) PointAtDistanceFromStart(double dist)
    {
        if (dist >= length) dist = length;
        return (p1.x + dir.x * dist, p1.y + dir.y * dist);
    }

    public (double x, double y) PointAtDistanceFromEnd(double dist)
    {
        if (dist >= length) dist = length;
        return (p2.x - dir.x * dist, p2.y - dir.y * dist);
    }

    public (double x, double y) TangentAtStart() => dir;
    public (double x, double y) TangentAtEnd() => dir;

    // Build an SVG line command from pStart to pEnd
    public string ToSvgLine((double x, double y) pStart, (double x, double y) pEnd)
    {
        // "L xEnd,yEnd"
        return $"L {pEnd.x:F3},{pEnd.y:F3} ";
    }
}

//------------------------------------------------------------------------
// Arc segment (circle center at 0,0), from p1 to p2, with radius, possibly clockwise
//------------------------------------------------------------------------

public class ArcSegment : ISegment
{
    private (double x, double y) p1, p2;
    private double radius;
    private bool sweepPositive;

    private double length;  // arc length
    private double startAngle, endAngle; // in radians

    public ArcSegment(double x1, double y1, double x2, double y2, double r, bool sweepPositive)
    {
        p1 = (x1, y1);
        p2 = (x2, y2);
        radius = r;
        this.sweepPositive = sweepPositive;

        // angles
        startAngle = Math.Atan2(y1, x1);
        endAngle = Math.Atan2(y2, x2);

        double delta = endAngle - startAngle;
        if (sweepPositive)
        {
            // ensure delta is positive by possibly adding 2π
            if (delta < 0) delta += 2 * Math.PI;
        }
        else
        {
            // ensure delta is negative by possibly subtracting 2π
            if (delta > 0) delta -= 2 * Math.PI;
        }
        length = Math.Abs(delta) * radius;
    }

    public (double x, double y) StartPoint() => p1;
    public (double x, double y) EndPoint() => p2;

    public (double x, double y) PointAtDistanceFromStart(double dist)
    {
        if (dist < 0) dist = 0;
        if (dist > length) dist = length;

        double fraction = dist / length; // between 0..1
        double delta = (endAngle - startAngle);
        // If positive sweep, delta might be negative in raw form, but we corrected it above
        // If negative sweep, delta might be positive in raw form, also corrected above
        double angleTravel = fraction * delta; // total angle traveled
        double angle = startAngle + angleTravel;
        double x = radius * Math.Cos(angle);
        double y = radius * Math.Sin(angle);
        return (x, y);
    }

    public (double x, double y) PointAtDistanceFromEnd(double dist)
    {
        // distance from end is just length - dist from start
        double fromStart = length - dist;
        return PointAtDistanceFromStart(fromStart);
    }

    public (double x, double y) TangentAtStart()
    {
        // Tangent is perpendicular to radius => direction is ( -sin(a), +cos(a) ) or vice versa
        // The sign depends on whether the arc is swept CCW or CW
        double a = startAngle;
        double dx = -Math.Sin(a);
        double dy = Math.Cos(a);
        if (!sweepPositive)
        {
            // flip direction for CW
            dx = -dx;
            dy = -dy;
        }
        // normalize
        double len = Math.Sqrt(dx * dx + dy * dy);
        return (dx / len, dy / len);
    }

    public (double x, double y) TangentAtEnd()
    {
        double a = endAngle;
        double dx = -Math.Sin(a);
        double dy = Math.Cos(a);
        if (!sweepPositive)
        {
            dx = -dx;
            dy = -dy;
        }
        double len = Math.Sqrt(dx * dx + dy * dy);
        return (dx / len, dy / len);
    }

    // Build an SVG arc command from pStart to pEnd, using the same radius & sweep
    public string ToSvgArc((double x, double y) pStart, (double x, double y) pEnd)
    {
        // For largeArcFlag, we see if the angle spanned is > 180 deg
        double angleSpan = Math.Abs(endAngle - startAngle) * (180.0 / Math.PI);
        if (angleSpan > 180.0 && angleSpan < 360.0) // exclude full 360
            return $"A {radius:F3},{radius:F3} 0 1,{(sweepPositive ? 1 : 0)} {pEnd.x:F3},{pEnd.y:F3} ";
        else
            return $"A {radius:F3},{radius:F3} 0 0,{(sweepPositive ? 1 : 0)} {pEnd.x:F3},{pEnd.y:F3} ";
    }
}