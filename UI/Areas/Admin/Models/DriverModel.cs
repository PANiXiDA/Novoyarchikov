using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common.Enums;
using Entities;

namespace UI.Areas.Admin.Models
{
	public class DriverModel
	{
		[Required(ErrorMessage = "Укажите значение")]
		[Display(Name = "DriverId")]
		public int DriverId { get; set; }

		[Required(ErrorMessage = "Укажите значение")]
		[Display(Name = "CompanyId")]
		public int CompanyId { get; set; }

		[Display(Name = "Имя водителя")]
		public string DriverName { get; set; }

		[Display(Name = "Опыт водителя")]
		public int? Experience { get; set; }

		[Display(Name = "Количество аварий")]
		public int? NumberOfAccidents { get; set; }

		public static DriverModel FromEntity(Driver obj)
		{
			return obj == null ? null : new DriverModel
			{
				DriverId = obj.DriverId,
				CompanyId = obj.CompanyId,
				DriverName = obj.DriverName,
				Experience = obj.Experience,
				NumberOfAccidents = obj.NumberOfAccidents,
			};
		}

		public static Driver ToEntity(DriverModel obj)
		{
			return obj == null ? null : new Driver(obj.DriverId, obj.CompanyId, obj.DriverName, obj.Experience,
				obj.NumberOfAccidents);
		}

		public static List<DriverModel> FromEntitiesList(IEnumerable<Driver> list)
		{
			return list?.Select(FromEntity).ToList();
		}

		public static List<Driver> ToEntitiesList(IEnumerable<DriverModel> list)
		{
			return list?.Select(ToEntity).ToList();
		}
	}
}
