using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common.Enums;
using Entities;

namespace UI.Areas.Admin.Models
{
	public class RouteModel
	{
		[Required(ErrorMessage = "Укажите значение")]
		[Display(Name = "RouteId")]
		public int RouteId { get; set; }

		[Required(ErrorMessage = "Укажите значение")]
		[Display(Name = "CompanyId")]
		public int CompanyId { get; set; }

		[Display(Name = "Название маршрута")]
		public string RouteName { get; set; }

		[Display(Name = "Длительность маршрута")]
		public int? RouteDuration { get; set; }

		public static RouteModel FromEntity(Route obj)
		{
			return obj == null ? null : new RouteModel
			{
				RouteId = obj.RouteId,
				CompanyId = obj.CompanyId,
				RouteName = obj.RouteName,
				RouteDuration = obj.RouteDuration,
			};
		}

		public static Route ToEntity(RouteModel obj)
		{
			return obj == null ? null : new Route(obj.RouteId, obj.CompanyId, obj.RouteName, obj.RouteDuration);
		}

		public static List<RouteModel> FromEntitiesList(IEnumerable<Route> list)
		{
			return list?.Select(FromEntity).ToList();
		}

		public static List<Route> ToEntitiesList(IEnumerable<RouteModel> list)
		{
			return list?.Select(ToEntity).ToList();
		}
	}
}
