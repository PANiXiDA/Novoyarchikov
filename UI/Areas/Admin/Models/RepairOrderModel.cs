using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common.Enums;
using Entities;

namespace UI.Areas.Admin.Models
{
	public class RepairOrderModel
	{
		[Required(ErrorMessage = "Укажите значение")]
		[Display(Name = "OrderId")]
		public int OrderId { get; set; }

		[Required(ErrorMessage = "Укажите значение")]
		[Display(Name = "BusId")]
		public int BusId { get; set; }

		[Display(Name = "Цена ремонта")]
		public int? RepairPrice { get; set; }

		[Display(Name = "Дата ремонта")]
		public DateTime? RepairDate { get; set; }

		public static RepairOrderModel FromEntity(RepairOrder obj)
		{
			return obj == null ? null : new RepairOrderModel
			{
				OrderId = obj.OrderId,
				BusId = obj.BusId,
				RepairPrice = obj.RepairPrice,
				RepairDate = obj.RepairDate,
			};
		}

		public static RepairOrder ToEntity(RepairOrderModel obj)
		{
			return obj == null ? null : new RepairOrder(obj.OrderId, obj.BusId, obj.RepairPrice, obj.RepairDate);
		}

		public static List<RepairOrderModel> FromEntitiesList(IEnumerable<RepairOrder> list)
		{
			return list?.Select(FromEntity).ToList();
		}

		public static List<RepairOrder> ToEntitiesList(IEnumerable<RepairOrderModel> list)
		{
			return list?.Select(ToEntity).ToList();
		}
	}
}
