using System;
using System.Collections.Generic;
using System.Linq;
using Common.Enums;

namespace Entities
{
	public class Company
	{
		public int CompanyId { get; set; }
		public string CompanyName { get; set; }
		public string Owner { get; set; }

		public Company(int companyId, string companyName, string owner)
		{
			CompanyId = companyId;
			CompanyName = companyName;
			Owner = owner;
		}
	}
}
