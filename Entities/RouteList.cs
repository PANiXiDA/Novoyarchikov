using System;
using System.Collections.Generic;
using System.Linq;
using Common.Enums;

namespace Entities
{
	public class RouteList
	{
		public int SheetId { get; set; }
		public int BusId { get; set; }
		public int DriverId { get; set; }
		public int RouteId { get; set; }
		public DateTime? DataRoute { get; set; }

		public RouteList(int sheetId, int busId, int driverId, int routeId, DateTime? dataRoute)
		{
			SheetId = sheetId;
			BusId = busId;
			DriverId = driverId;
			RouteId = routeId;
			DataRoute = dataRoute;
		}
	}
}
