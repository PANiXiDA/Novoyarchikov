using System;
using System.Collections.Generic;
using System.Linq;
using Common.Enums;

namespace Entities
{
	public class Route
	{
		public int RouteId { get; set; }
		public int CompanyId { get; set; }
		public string RouteName { get; set; }
		public int? RouteDuration { get; set; }

		public Route(int routeId, int companyId, string routeName, int? routeDuration)
		{
			RouteId = routeId;
			CompanyId = companyId;
			RouteName = routeName;
			RouteDuration = routeDuration;
		}
	}
}
