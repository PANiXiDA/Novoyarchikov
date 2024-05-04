using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common.Enums;
using Entities;

namespace UI.Areas.Admin.Models
{
	public class ScheduleModel
	{
		[Required(ErrorMessage = "Укажите значение")]
		[Display(Name = "ScheduleId")]
		public int ScheduleId { get; set; }

		[Required(ErrorMessage = "Укажите значение")]
		[Display(Name = "RouteId")]
		public int RouteId { get; set; }

		[Required(ErrorMessage = "Укажите значение")]
		[Display(Name = "StopId")]
		public int StopId { get; set; }

		[Display(Name = "Дата отправки")]
		public TimeSpan? DepartureTime { get; set; }

		[Display(Name = "Дата прибытия")]
		public TimeSpan? ArrivalTime { get; set; }

		public static ScheduleModel FromEntity(Schedule obj)
		{
			return obj == null ? null : new ScheduleModel
			{
				ScheduleId = obj.ScheduleId,
				RouteId = obj.RouteId,
				StopId = obj.StopId,
				DepartureTime = obj.DepartureTime,
				ArrivalTime = obj.ArrivalTime,
			};
		}

		public static Schedule ToEntity(ScheduleModel obj)
		{
			return obj == null ? null : new Schedule(obj.ScheduleId, obj.RouteId, obj.StopId, obj.DepartureTime,
				obj.ArrivalTime);
		}

		public static List<ScheduleModel> FromEntitiesList(IEnumerable<Schedule> list)
		{
			return list?.Select(FromEntity).ToList();
		}

		public static List<Schedule> ToEntitiesList(IEnumerable<ScheduleModel> list)
		{
			return list?.Select(ToEntity).ToList();
		}
	}
}
