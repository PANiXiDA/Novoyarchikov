using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Common.Enums;

namespace Entities
{
	public class Bus
	{
		public int BusId { get; set; }
		public int? BusNumber { get; set; }
		public int CompanyId { get; set; }
		public int? NumberOfSeats { get; set; }
		public string Model { get; set; }
		public DateTime? ReleaseDate { get; set; }

		public Bus(int busId, int? busNumber, int companyId, int? numberOfSeats, string model, DateTime? releaseDate)
		{
			BusId = busId;
			BusNumber = busNumber;
			CompanyId = companyId;
			NumberOfSeats = numberOfSeats;
			Model = model;
			ReleaseDate = releaseDate;
		}
	}
}
