using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common.Enums;
using Entities;

namespace UI.Areas.Admin.Models
{
	public class BusModel
	{
		[Required(ErrorMessage = "Укажите значение")]
		[Display(Name = "BusId")]
		public int BusId { get; set; }

		[Display(Name = "Номер автобуса")]
		public int? BusNumber { get; set; }

		[Required(ErrorMessage = "Укажите значение")]
		[Display(Name = "CompanyId")]
		public int CompanyId { get; set; }

		[Display(Name = "Количество сидений")]
		public int? NumberOfSeats { get; set; }

		[Display(Name = "Модель автобуса")]
		public string ModelName { get; set; }

		[Display(Name = "Дата выпуска")]
		public DateTime? ReleaseDate { get; set; }

		public static BusModel FromEntity(Bus obj)
		{
			return obj == null ? null : new BusModel
			{
				BusId = obj.BusId,
				BusNumber = obj.BusNumber,
				CompanyId = obj.CompanyId,
				NumberOfSeats = obj.NumberOfSeats,
				ModelName = obj.Model,
				ReleaseDate = obj.ReleaseDate,
			};
		}

		public static Bus ToEntity(BusModel obj)
		{
			return obj == null ? null : new Bus(obj.BusId, obj.BusNumber, obj.CompanyId, obj.NumberOfSeats, obj.ModelName,
				obj.ReleaseDate);
		}

		public static List<BusModel> FromEntitiesList(IEnumerable<Bus> list)
		{
			return list?.Select(FromEntity).ToList();
		}

		public static List<Bus> ToEntitiesList(IEnumerable<BusModel> list)
		{
			return list?.Select(ToEntity).ToList();
		}
	}
}
