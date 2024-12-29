using Lagoon.Web.DTOs;

namespace Lagoon.Application.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<RadialBarChartDto> GetTotalBookingRadialChartData();
        Task<RadialBarChartDto> GetRegisteredUserChartData();
        Task<RadialBarChartDto> GetRevenueChartData();
        Task<PieChartDto> GetBookingPieChartData();
        Task<LineChartDto> GetMemberAndBookingLineChartData();
    }
}
