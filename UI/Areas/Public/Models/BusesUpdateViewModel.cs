using System;

namespace UI.Areas.Public.Models
{
	public class BusesUpdateViewModel
	{
		public int NumberOfSeats { get; set; }
		public string CompanyName { get; set; }
		public string Model { get; set; }
		public DateTime ReleaseDate { get; set; }
	}
}
