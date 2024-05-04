using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common.Enums;
using Entities;

namespace UI.Areas.Admin.Models
{
	public class RouteListModel
	{
		[Required(ErrorMessage = "Укажите значение")]
		[Display(Name = "SheetId")]
		public int SheetId { get; set; }

		[Required(ErrorMessage = "Укажите значение")]
		[Display(Name = "BusId")]
		public int BusId { get; set; }

		[Required(ErrorMessage = "Укажите значение")]
		[Display(Name = "DriverId")]
		public int DriverId { get; set; }

		[Required(ErrorMessage = "Укажите значение")]
		[Display(Name = "RouteId")]
		public int RouteId { get; set; }

		[Display(Name = "Дата у маршрутного листа")]
		public DateTime? DataRoute { get; set; }

		public static RouteListModel FromEntity(RouteList obj)
		{
			return obj == null ? null : new RouteListModel
			{
				SheetId = obj.SheetId,
				BusId = obj.BusId,
				DriverId = obj.DriverId,
				RouteId = obj.RouteId,
				DataRoute = obj.DataRoute,
			};
		}

		public static RouteList ToEntity(RouteListModel obj)
		{
			return obj == null ? null : new RouteList(obj.SheetId, obj.BusId, obj.DriverId, obj.RouteId, obj.DataRoute);
		}

		public static List<RouteListModel> FromEntitiesList(IEnumerable<RouteList> list)
		{
			return list?.Select(FromEntity).ToList();
		}

		public static List<RouteList> ToEntitiesList(IEnumerable<RouteListModel> list)
		{
			return list?.Select(ToEntity).ToList();
		}
	}
}
