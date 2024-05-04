using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common.Enums;
using Entities;

namespace UI.Areas.Admin.Models
{
	public class StopModel
	{
		[Required(ErrorMessage = "Укажите значение")]
		[Display(Name = "StopId")]
		public int StopId { get; set; }

		[Display(Name = "Название остановки")]
		public string StopName { get; set; }

		public static StopModel FromEntity(Stop obj)
		{
			return obj == null ? null : new StopModel
			{
				StopId = obj.StopId,
				StopName = obj.StopName,
			};
		}

		public static Stop ToEntity(StopModel obj)
		{
			return obj == null ? null : new Stop(obj.StopId, obj.StopName);
		}

		public static List<StopModel> FromEntitiesList(IEnumerable<Stop> list)
		{
			return list?.Select(FromEntity).ToList();
		}

		public static List<Stop> ToEntitiesList(IEnumerable<StopModel> list)
		{
			return list?.Select(ToEntity).ToList();
		}
	}
}
