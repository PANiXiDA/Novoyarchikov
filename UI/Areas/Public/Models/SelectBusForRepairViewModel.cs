using System;

namespace UI.Areas.Public.Models
{
	public class SelectBusForRepairViewModel
	{
		public int BusID { get; set; }
		public int BusNumber { get; set; }
		public int CompanyID { get; set; }
		public int NumberOfSeats { get; set; }
		public string Model { get; set; }
		public DateTime ReleaseDate { get; set; }
	}
}
