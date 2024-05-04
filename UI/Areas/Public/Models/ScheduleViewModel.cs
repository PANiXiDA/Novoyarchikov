using System;

namespace UI.Areas.Public.Models
{
	public class ScheduleViewModel
	{
		public string StopName { get; set; }
		public TimeSpan DepartureTime { get; set; }
		public TimeSpan ArrivalTime { get; set; }
	}
}
