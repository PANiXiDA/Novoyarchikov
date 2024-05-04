using System;
using System.Collections.Generic;
using System.Linq;
using Common.Enums;

namespace Entities
{
	public class Schedule
	{
		public int ScheduleId { get; set; }
		public int RouteId { get; set; }
		public int StopId { get; set; }
		public TimeSpan? DepartureTime { get; set; }
		public TimeSpan? ArrivalTime { get; set; }

		public Schedule(int scheduleId, int routeId, int stopId, TimeSpan? departureTime, TimeSpan? arrivalTime)
		{
			ScheduleId = scheduleId;
			RouteId = routeId;
			StopId = stopId;
			DepartureTime = departureTime;
			ArrivalTime = arrivalTime;
		}
	}
}
