using System;
using System.Collections.Generic;
using System.Linq;
using Common.Enums;

namespace Entities
{
	public class Stop
	{
		public int StopId { get; set; }
		public string StopName { get; set; }

		public Stop(int stopId, string stopName)
		{
			StopId = stopId;
			StopName = stopName;
		}
	}
}
