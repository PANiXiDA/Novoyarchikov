using System;
using System.Collections.Generic;
using System.Linq;
using Common.Enums;

namespace Entities
{
	public class RepairOrder
	{
		public int OrderId { get; set; }
		public int BusId { get; set; }
		public int? RepairPrice { get; set; }
		public DateTime? RepairDate { get; set; }

		public RepairOrder(int orderId, int busId, int? repairPrice, DateTime? repairDate)
		{
			OrderId = orderId;
			BusId = busId;
			RepairPrice = repairPrice;
			RepairDate = repairDate;
		}
	}
}
