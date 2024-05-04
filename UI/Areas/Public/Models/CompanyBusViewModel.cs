using System;

namespace UI.Areas.Public.Models
{
	public class CompanyBusViewModel
	{
		public string CompanyName { get; set; }
		public string CompanyOwner { get; set; }
		public int BusNumber { get; set; }
		public string BusModel { get; set; }
		public int BusNumberOfSeats { get; set; }
		public DateTime BusReleaseDate { get; set; }
		public string DriverName { get; set; }
		public int DriverExperience { get; set; }
		public int DriverNumberOfAccidents { get; set; }
		public DateTime RepairDate { get; set; }
	}
}
