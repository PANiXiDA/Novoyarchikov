using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common.Enums;
using Entities;

namespace UI.Areas.Admin.Models
{
	public class CompanyModel
	{
		[Required(ErrorMessage = "Укажите значение")]
		[Display(Name = "CompanyId")]
		public int CompanyId { get; set; }

		[Display(Name = "Название компании")]
		public string CompanyName { get; set; }

		[Display(Name = "Владелец компании")]
		public string Owner { get; set; }

		public static CompanyModel FromEntity(Company obj)
		{
			return obj == null ? null : new CompanyModel
			{
				CompanyId = obj.CompanyId,
				CompanyName = obj.CompanyName,
				Owner = obj.Owner,
			};
		}

		public static Company ToEntity(CompanyModel obj)
		{
			return obj == null ? null : new Company(obj.CompanyId, obj.CompanyName, obj.Owner);
		}

		public static List<CompanyModel> FromEntitiesList(IEnumerable<Company> list)
		{
			return list?.Select(FromEntity).ToList();
		}

		public static List<Company> ToEntitiesList(IEnumerable<CompanyModel> list)
		{
			return list?.Select(ToEntity).ToList();
		}
	}
}
