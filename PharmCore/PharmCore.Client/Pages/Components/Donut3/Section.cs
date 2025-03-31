namespace PharmCore.Client.Pages.Components.Donut3
{
    public struct Section
    {
        public double Start { get; }
        public double End { get; }

        public Section(double startAngle, double endAngle)
        {
            Start = startAngle;
            End = endAngle;
        }
    }

}
