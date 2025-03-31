namespace PharmCore.Client.Pages.Components.Donut3;

using Microsoft.AspNetCore.Components;
using System.Runtime.InteropServices;
using Utils;


public class WedgeButton
{
    public string WedgePath { get; }
    public int index;
    public int indexplus;
    public double animationOrder;
    public int animationRotation = 1;
    [Parameter]
    public Action OnClick { get; set; }
    public List<string> labels;
    public int number;
    public Section sector;
    //public OptionalAttribute svg;
    // Gap distance (linear distance between adjacent wedges)
    public double GapDistance { get; set; }

    // Internal fields to store parameters for later calculations.
    private int _total;
    private double _innerRadius;
    private double _outerRadius;

    // Variables for the placement and rotation of text
    public double text_x;
    public double text_y;
    public double text_rotation;
    public double outerText_x;
    public double outerText_y;
    public double outerText2_x;
    public double outerText2_y;
    public double outerText_dx;
    public double outerText_dy;
    public double outerText_rotation;
    public double outerText_width;

    public double line_dy = 1.1;
    public double outerText_line_dx;
    public double outerText_line_dy;
    public bool isBelow;


    public string hslColor;

    //private double _horizontalScale = 1.4; 

    // Internal variable for the corner rounding (in the same units as the radii)
    private double cornerRadius;

    public WedgeButton(int index, int total, double innerRadius, double outerRadius, double rounding,
    double gapDistance, string hsl, Action onClick, int number, List<string> labels, Section sector)//, OptionalAttribute svg)
    {
        this.index = index;
        indexplus = index + 1;

        _total = total;
        _innerRadius = innerRadius;
        _outerRadius = outerRadius;
        cornerRadius = rounding;
        GapDistance = gapDistance;
        hslColor = hsl;
        OnClick = onClick;
        this.number = number;
        this.labels = labels;
        this.sector = sector;
        //this.svg = svg;


        animationOrder = index;
        // animationOrder = index % (total / 4);

        if (indexplus > total / 2)
        {
            animationOrder = index - total / 2;
        }
        else
        {
            animationOrder = index;
        }

        // //19 -> 24
        // if      (indexplus > total * 3 / 4)
        // {
        //     animationOrder = indexplus - total * 3 / 4;
        //     animationRotation = -1;
        // }
        // // 13 <- 18
        // else if (indexplus > total * 2 / 4)
        // {
        //     animationOrder = total * 3 / 4 - index;
        // }
        // // 7 <- 12
        // else if (indexplus > total / 4)
        // {
        //     animationOrder = total * 2 / 4 - index + total / 4;
        // }
        // // 1 -> 6
        // else
        // {
        //     animationOrder = indexplus + total / 4;
        //     animationRotation = -1;
        // }

        // Console.WriteLine($"nr: {indexplus} > Animation order:{animationOrder}");
        SetTextAttributes();

        if (isBelow == true)
        {
            labels.Reverse();
            
        }

        WedgePath = GetWedgePath(index, total, innerRadius, outerRadius, gapDistance);
    }

    /// <summary>
    /// Returns a string with text attributes to position and rotate text inside the wedge.
    /// Format: x="..." y="..." rotate="..."
    /// The rotation is computed from the wedge’s bisector and flipped if in the lower half.
    /// </summary>
    public void SetTextAttributes()
    {
        // Compute the base (unadjusted) angles for the wedge
        double angleStep = 360.0 / _total;
        // double halfStep = angleStep / 2.0;

        double baseStartAngle = sector.Start;   //index * angleStep + halfStep;
        double baseEndAngle = sector.End;       //(index + 1) * angleStep + halfStep;
                                                // The bisector angle of the wedge in degrees
        double centerAngle = (baseStartAngle + baseEndAngle) / 2.0;
        double numberAngle = (baseStartAngle * 7 + baseEndAngle) / 8.0;

        // Compute the center radius as the average of inner and outer radii
        double centerRadius = (_innerRadius * 4 + _outerRadius) / 5.0; // Name no longer accurate

        double radians = centerAngle * Math.PI / 180.0;
        double numberRadians = numberAngle * Math.PI / 180.0;
        // double x = centerRadius * Math.Cos(radians);
        // double y = centerRadius * Math.Sin(radians);
        double x = centerRadius * Math.Cos(numberRadians);
        double y = centerRadius * Math.Sin(numberRadians);
        double outer_x = _outerRadius * Math.Cos(radians);
        double outer_y = _outerRadius * Math.Sin(radians);


        // Determine the text rotation.
        // If the wedge is in the lower half (i.e. its bisector is between 90° and 270°),
        // flip the text rotation so it stays closer to horizontal.
        double textRotation = centerAngle;
        if (centerAngle > 90 && centerAngle < 270)
        {
            textRotation += 180;
        }
        // Normalize rotation to [0,360)
        textRotation %= 360;

        double outerOffset = -8.0;
        // outerText_line_dy = $"-{line_dy}em";
        outerText_line_dy = -line_dy;
        isBelow = true;

        double outerTextRotation = centerAngle;
        if (centerAngle > 180 && centerAngle < 360)
        {
            outerTextRotation += 180;
            // outerText_line_dy = $"{line_dy}em";
            outerText_line_dy = line_dy;
            isBelow = false;
            outerOffset = 4.0;
        }
        // Normalize rotation to [0,360)
        textRotation %= 360;

        // Set the values
        text_x = x;
        text_y = y;
        outerText_x = outer_x * 0.96;
        outerText_y = outer_y * 0.96;
        outerText2_x = outer_x * 0.90;
        outerText2_y = outer_y * 0.90;
        text_rotation = textRotation;
        outerText_rotation = outerTextRotation + 270;



        double arcLength = Math.PI * _outerRadius * angleStep / 180; // Approximate arc length
        outerText_width = arcLength * 0.8;  // Adjust factor to fit text properly
        outerText_dx = arcLength * Math.Cos(radians + 1);
        outerText_dy = outerOffset;
        // outerText_line_dx = -outer_x * 0.08;
        // outerText_line_dy = -outer_y * 0.08;

    }

    private string GetWedgePath(int idx, int total, double rInner, double rOuter, double gapDist)
    {
        //--------------------------------------------------
        // 1) Compute the raw wedge corners and arcs (before rounding).
        //--------------------------------------------------
        //double normalize(double angle) => () > angle % 360; // Ensures 0 <= angle < 360

        double angleStep = 360.0 / total;
        double halfStep = angleStep / 2.0;
        double startAngle = sector.Start;    //idx * angleStep + halfStep;
        double endAngle = sector.End;      //(idx + 1) * angleStep + halfStep;
                                           // startAngle %= 360;
                                           // endAngle %= 360;

        // if (idx == total / 2 - 1)
        // {
        //     double hold = startAngle;
        //     startAngle = endAngle;
        //     endAngle = hold;
        // }

        // Half-gap angles for each circle, so the wedge has a uniform linear gap:
        double halfGapOuterDeg = gapDist / rOuter * (180.0 / Math.PI) / 2.0;
        double halfGapInnerDeg = gapDist / rInner * (180.0 / Math.PI) / 2.0;

        // Outer arc angles
        double outerStart = startAngle + halfGapOuterDeg;
        double outerEnd = endAngle - halfGapOuterDeg;
        // Inner arc angles
        double innerStart = startAngle + halfGapInnerDeg;
        double innerEnd = endAngle - halfGapInnerDeg;

        // Convert degrees to radians helper remains the same.
        double toRad(double deg) => deg * Math.PI / 180.0;

        // Outer arc endpoints (apply horizontal scaling)
        double x1 = rOuter * Math.Cos(toRad(outerStart));
        double y1 = rOuter * Math.Sin(toRad(outerStart));
        double x2 = rOuter * Math.Cos(toRad(outerEnd));
        double y2 = rOuter * Math.Sin(toRad(outerEnd));

        // Inner arc endpoints (apply horizontal scaling)
        double x3 = rInner * Math.Cos(toRad(innerEnd));
        double y3 = rInner * Math.Sin(toRad(innerEnd));
        double x4 = rInner * Math.Cos(toRad(innerStart));
        double y4 = rInner * Math.Sin(toRad(innerStart));

        // So the original path (no rounding) would be:
        //   M x1,y1
        //   A rOuter,rOuter 0 0,1 x2,y2
        //   L x3,y3
        //   A rInner,rInner 0 0,0 x4,y4
        //   Z
        //
        // That yields 4 corners:
        //   Corner A: (x1,y1)   with next segment being the arc to (x2,y2) and previous the close from (x4,y4).
        //   Corner B: (x2,y2)   arc -> line
        //   Corner C: (x3,y3)   line -> arc
        //   Corner D: (x4,y4)   arc -> line (closing back to x1,y1).

        //--------------------------------------------------
        // 2) Build segment "helpers" so we can shorten them.
        //--------------------------------------------------

        // --- Outer arc (A1) from (x1,y1) to (x2,y2), center=(0,0), radius=rOuter, sweep= + (outerEnd - outerStart)
        var arcOuter = new ArcSegment(x1, y1, x2, y2, rOuter, sweepPositive: true);

        // --- Radial line (L1) from (x2,y2) to (x3,y3)
        var lineRadial = new LineSegment(x2, y2, x3, y3);

        // --- Inner arc (A2) from (x3,y3) to (x4,y4), center=(0,0), radius=rInner, sweep= (innerStart->innerEnd is also a forward angle, but we use a negative sweep if needed)
        // Actually we want the direction from x3->x4 to be "reverse" (outer circle is drawn anticlockwise with large-arc=1, the inner might be the opposite).
        // But let's keep it simple and rely on sign. We'll detect if it's clockwise or not:
        bool innerSweepPositive = innerEnd - innerStart >= 0 || idx == total;
        var arcInner = new ArcSegment(x3, y3, x4, y4, rInner, sweepPositive: false); //innerSweepPositive);

        // --- Closing line (L2) from (x4,y4) to (x1,y1)
        var lineClose = new LineSegment(x4, y4, x1, y1);

        //--------------------------------------------------
        // 3) Round each corner by:
        //    - Trimming cornerRadius from the end of the first segment
        //    - Trimming cornerRadius from the start of the second segment
        //    - Connecting those new endpoints with a small arc of radius=cornerRadius
        //--------------------------------------------------

        // We'll do it corner-by-corner. Each corner has 2 segments: (prevSegment, nextSegment).

        // Corner A: intersection of lineClose -> arcOuter at (x1,y1)
        var cornerA = RoundCorner(lineClose, arcOuter, cornerRadius, isArc1: false, isArc2: true);

        // Corner B: intersection of arcOuter -> lineRadial at (x2,y2)
        var cornerB = RoundCorner(arcOuter, lineRadial, cornerRadius, isArc1: true, isArc2: false);

        // Corner C: intersection of lineRadial -> arcInner at (x3,y3)
        var cornerC = RoundCorner(lineRadial, arcInner, cornerRadius, isArc1: false, isArc2: true);

        // Corner D: intersection of arcInner -> lineClose at (x4,y4)
        var cornerD = RoundCorner(arcInner, lineClose, cornerRadius, isArc1: true, isArc2: false);

        //--------------------------------------------------
        // 4) Build the final path from these trimmed segments + small corner arcs.
        //    We'll proceed in a loop: A->B->C->D->A
        //--------------------------------------------------

        // After rounding, each segment has a "start" point, a "trimmed end" point, etc.
        // cornerX gives us two points: cornerX.PrevEnd and cornerX.NextStart, plus the arc that joins them.

        // So the final path is:
        //   M cornerA.NextStart
        //   (arcOuter minus corners A and B in between)
        //   cornerB.ArcToCorner  (the small rounding arc at corner B)
        //   (lineRadial minus corners B and C in between)
        //   cornerC.ArcToCorner
        //   (arcInner minus corners C and D in between)
        //   cornerD.ArcToCorner
        //   (lineClose minus corners D and A in between)
        //   cornerA.ArcToCorner
        //   Z

        // For simplicity, let's build it segment by segment with the known trimmed endpoints.

        // Move to the first point of arcOuter (already trimmed by cornerA on the start):
        var path = $"M {cornerA.NextStart.x:F3},{cornerA.NextStart.y:F3} ";

        // 4a) Add the trimmed outer arc: from cornerA.NextStart to cornerB.PrevEnd
        //     We'll ask arcOuter to give us an SVG arc command that goes from A's nextStart to B's prevEnd.
        path += arcOuter.ToSvgArc(cornerA.NextStart, cornerB.PrevEnd);

        // 4b) Add the small rounding arc at corner B
        path += cornerB.ArcSvgCommand;

        // 4c) Add the trimmed radial line: from cornerB.NextStart to cornerC.PrevEnd
        path += lineRadial.ToSvgLine(cornerB.NextStart, cornerC.PrevEnd);

        // 4d) Add the small rounding arc at corner C
        path += cornerC.ArcSvgCommand;

        // 4e) Add the trimmed inner arc: from cornerC.NextStart to cornerD.PrevEnd
        path += arcInner.ToSvgArc(cornerC.NextStart, cornerD.PrevEnd);

        // 4f) Add the small rounding arc at corner D
        path += cornerD.ArcSvgCommand;

        // 4g) Add the trimmed closing line: from cornerD.NextStart to cornerA.PrevEnd
        path += lineClose.ToSvgLine(cornerD.NextStart, cornerA.PrevEnd);

        // 4h) Add the small rounding arc at corner A
        path += cornerA.ArcSvgCommand;

        // Finally close the path
        path += " Z";

        return path;
    }

    //------------------------------------------------------------------------
    // Corner rounding helper
    //------------------------------------------------------------------------

    /// <summary>
    ///  Given two segments that meet at a corner, shortens each by cornerRadius
    ///  and returns the arc command that connects them. We also return the
    ///  "trimmed endpoints" for each segment.
    /// </summary>
    private CornerResult RoundCorner(ISegment seg1, ISegment seg2, double cornerRadius,
                                     bool isArc1, bool isArc2)
    {
        // 1) The corner is seg1.EndPoint = seg2.StartPoint
        var corner = seg1.EndPoint();

        // 2) "Trim" seg1 by cornerRadius at its end, producing a new endpoint
        //    (the place where we will start the small corner arc).
        //    If seg1 is an arc, we measure arc length. If seg1 is a line, we measure linear distance.
        var prevEnd = seg1.PointAtDistanceFromEnd(cornerRadius);

        // 3) "Trim" seg2 by cornerRadius at its start, producing a new startpoint
        //    (the place where we will end the small corner arc).
        var nextStart = seg2.PointAtDistanceFromStart(cornerRadius);

        // 4) Build a small arc that goes from prevEnd to nextStart with radius=cornerRadius.
        //    The center/direction is determined by the tangents of seg1 and seg2 at that corner.
        //    A simpler approach (for small corners) is to assume "sweep=0" and let the arc figure it out
        //    from the cross product. But let's do a typical 0,0,1 or 0,0,0 flag approach.
        string arcCmd = BuildCornerArc(prevEnd, seg1.TangentAtEnd(), nextStart, seg2.TangentAtStart(), cornerRadius);

        return new CornerResult
        {
            PrevEnd = prevEnd,   // Where seg1 now ends
            NextStart = nextStart, // Where seg2 now begins
            ArcSvgCommand = arcCmd
        };
    }

    /// <summary>
    ///  Build an SVG arc command (using "A cornerRadius,cornerRadius ...") from p1 to p2
    ///  that smoothly transitions the tangents. For small corner arcs, a simple approach is
    ///  to use the sign of cross(tangent1, tangent2) to decide the sweep flag.
    /// </summary>
    private string BuildCornerArc((double x, double y) p1, (double x, double y) t1,
                                  (double x, double y) p2, (double x, double y) t2,
                                  double r)
    {
        // We can figure out if it's a left or right turn by cross product of t1 and t2.
        double cross = t1.x * t2.y - t1.y * t2.x;
        // If cross > 0 => one orientation, else the other
        int sweepFlag = cross > 0 ? 1 : 0;

        // For very small arcs, largeArcFlag is 0
        int largeArcFlag = 0;

        // "A rx,ry 0 largeArcFlag,sweepFlag x2,y2"
        return $" A{r},{r} 0 {largeArcFlag},{sweepFlag} {p2.x:F3},{p2.y:F3} ";
    }
}

