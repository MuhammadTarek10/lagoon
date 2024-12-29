namespace Lagoon.Web.DTOs
{
    public class LineChartDto
    {
        public required List<ChartData> Series { get; set; }
        public required string[] Categories { get; set; }
    }

    public class ChartData
    {
        public required string Name { get; set; }
        public required int[] Data { get; set; }
    }
}
