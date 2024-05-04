
using Dal.DbModels;
using System.Collections.Generic;

namespace UI.Areas.Public.Models
{
	public class CompaniesViewModel
	{
		public List<Company> Companies { get; set; }
		public List<Bus> Buses { get; set; }
	}
}
