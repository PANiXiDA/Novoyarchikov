using System;
using System.Collections.Generic;
using System.Linq;
using Common.Enums;

namespace Entities
{
	public class Driver
	{
		public int DriverId { get; set; }
		public int CompanyId { get; set; }
		public string DriverName { get; set; }
		public int? Experience { get; set; }
		public int? NumberOfAccidents { get; set; }

		public Driver(int driverId, int companyId, string driverName, int? experience, int? numberOfAccidents)
		{
			DriverId = driverId;
			CompanyId = companyId;
			DriverName = driverName;
			Experience = experience;
			NumberOfAccidents = numberOfAccidents;
		}
	}
}
