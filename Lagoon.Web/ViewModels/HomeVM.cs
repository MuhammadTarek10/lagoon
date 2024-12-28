using Lagoon.Domain.Entities;

namespace Lagoon.Web.ViewModels
{
    public class HomeVM
    {
        public required IEnumerable<Villa> VillaList { get; set; }
        public required DateOnly CheckInDate { get; set; }
        public DateOnly? CheckOutDate { get; set; }
        public int Nights { get; set; }
    }
}
