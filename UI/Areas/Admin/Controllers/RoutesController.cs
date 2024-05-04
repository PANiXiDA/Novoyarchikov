using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Common.Enums;
using Common.Search;
using BL;
using UI.Areas.Admin.Models;
using UI.Areas.Admin.Models.ViewModels;
using UI.Other;

namespace UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = nameof(UserRole.Admin))]
	public class RoutesController : Controller
	{
		public async Task<IActionResult> Index(int page = 1)
		{
			const int objectsPerPage = 20;
			var searchResult = await new RoutesBL().GetAsync(new RoutesSearchParams
			{
				StartIndex = (page - 1) * objectsPerPage,
				ObjectsCount = objectsPerPage,
			});
			var viewModel = new SearchResultViewModel<RouteModel>(RouteModel.FromEntitiesList(searchResult.Objects), 
				searchResult.Total, searchResult.RequestedStartIndex, searchResult.RequestedObjectsCount, 5);
			return View(viewModel);
		}

		public async Task<IActionResult> Update(int? id)
		{
			var model = new RouteModel();
			if (id != null)
			{
				model = RouteModel.FromEntity(await new RoutesBL().GetAsync(id.Value));
				if (model == null)
					return NotFound();
			}
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(RouteModel model)
		{
			const int objectsPerPage = 100;
			var searchResultCompanies = await new CompaniesBL().GetAsync(new CompaniesSearchParams
			{
				StartIndex = 0,
				ObjectsCount = objectsPerPage,
			});

			var companyModel = new SearchResultViewModel<CompanyModel>(CompanyModel.FromEntitiesList(searchResultCompanies.Objects),
				searchResultCompanies.Total, searchResultCompanies.RequestedStartIndex, searchResultCompanies.RequestedObjectsCount, 5);

			if (!companyModel.Objects.Any(x => x.CompanyId == model.CompanyId))
			{
				TempData[OperationResultType.Error.ToString()] = "Нельзя добавить или изменить, так как не существует компании по такому ID";
				return RedirectToAction("Index");
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}
			await new RoutesBL().AddOrUpdateAsync(RouteModel.ToEntity(model));
			TempData[OperationResultType.Success.ToString()] = "Данные сохранены";
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(int id)
		{
			const int objectsPerPage = 100;

			var searchResultRouteLists = await new RouteListBL().GetAsync(new RouteListSearchParams
			{
				StartIndex = 0,
				ObjectsCount = objectsPerPage,
			});
			var routeListModel = new SearchResultViewModel<RouteListModel>(RouteListModel.FromEntitiesList(searchResultRouteLists.Objects),
				searchResultRouteLists.Total, searchResultRouteLists.RequestedStartIndex, searchResultRouteLists.RequestedObjectsCount, 5);

			var searchResultSchedules = await new ScheduleBL().GetAsync(new ScheduleSearchParams
			{
				StartIndex = 0,
				ObjectsCount = objectsPerPage,
			});
			var scheduleModel = new SearchResultViewModel<ScheduleModel>(ScheduleModel.FromEntitiesList(searchResultSchedules.Objects),
				searchResultSchedules.Total, searchResultSchedules.RequestedStartIndex, searchResultSchedules.RequestedObjectsCount, 5);

			if (routeListModel.Objects.Where(x => x.RouteId == id).FirstOrDefault() != null)
			{
				TempData[OperationResultType.Error.ToString()] = "Нельзя удалить, так как к этому маршруту привязан маршрутный лист";
			}
			else if (scheduleModel.Objects.Where(x => x.RouteId == id).FirstOrDefault() != null)
			{
				TempData[OperationResultType.Error.ToString()] = "Нельзя удалить, так как к этому маршруту привязано расписание";
			}
			else
			{
				var result = await new RoutesBL().DeleteAsync(id);
				if (result)
					TempData[OperationResultType.Success.ToString()] = "Объект удален";
				else
					TempData[OperationResultType.Error.ToString()] = "Объект не найден";
			}
			return RedirectToAction("Index");
		}
	}
}
