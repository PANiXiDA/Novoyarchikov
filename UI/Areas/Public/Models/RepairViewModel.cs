using System.Collections.Generic;

namespace UI.Areas.Public.Models
{
	public class RepairViewModel
	{
		public List<SelectBusForRepairViewModel> BusesForRepair { get; set; }
		public int RepairCount { get; set; }
	}
}
