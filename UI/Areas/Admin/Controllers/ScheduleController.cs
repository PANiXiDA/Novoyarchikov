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
	public class ScheduleController : Controller
	{
		public async Task<IActionResult> Index(int page = 1)
		{
			const int objectsPerPage = 20;
			var searchResult = await new ScheduleBL().GetAsync(new ScheduleSearchParams
			{
				StartIndex = (page - 1) * objectsPerPage,
				ObjectsCount = objectsPerPage,
			});
			var viewModel = new SearchResultViewModel<ScheduleModel>(ScheduleModel.FromEntitiesList(searchResult.Objects), 
				searchResult.Total, searchResult.RequestedStartIndex, searchResult.RequestedObjectsCount, 5);
			return View(viewModel);
		}

		public async Task<IActionResult> Update(int? id)
		{
			var model = new ScheduleModel();
			if (id != null)
			{
				model = ScheduleModel.FromEntity(await new ScheduleBL().GetAsync(id.Value));
				if (model == null)
					return NotFound();
			}
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(ScheduleModel model)
		{
			const int objectsPerPage = 100;
			var searchResultRoutes = await new RoutesBL().GetAsync(new RoutesSearchParams
			{
				StartIndex = 0,
				ObjectsCount = objectsPerPage,
			});
			var routeModel = new SearchResultViewModel<RouteModel>(RouteModel.FromEntitiesList(searchResultRoutes.Objects),
				searchResultRoutes.Total, searchResultRoutes.RequestedStartIndex, searchResultRoutes.RequestedObjectsCount, 5);

			var searchResultStops = await new StopsBL().GetAsync(new StopsSearchParams
			{
				StartIndex = 0,
				ObjectsCount = objectsPerPage,
			});
			var stopModel = new SearchResultViewModel<StopModel>(StopModel.FromEntitiesList(searchResultStops.Objects),
				searchResultStops.Total, searchResultStops.RequestedStartIndex, searchResultStops.RequestedObjectsCount, 5);

			if (!routeModel.Objects.Any(x => x.RouteId == model.RouteId))
			{
				TempData[OperationResultType.Error.ToString()] = "Нельзя добавить или изменить, так как не существует маршрута по такому ID";
				return RedirectToAction("Index");
			}
			else if (!stopModel.Objects.Any(x => x.StopId == model.StopId))
			{
				TempData[OperationResultType.Error.ToString()] = "Нельзя добавить или изменить, так как не существует остановки по такому ID";
				return RedirectToAction("Index");
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}
			await new ScheduleBL().AddOrUpdateAsync(ScheduleModel.ToEntity(model));
			TempData[OperationResultType.Success.ToString()] = "Данные сохранены";
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(int id)
		{
			var result = await new ScheduleBL().DeleteAsync(id);
			if (result)
				TempData[OperationResultType.Success.ToString()] = "Объект удален";
			else
				TempData[OperationResultType.Error.ToString()] = "Объект не найден";
			return RedirectToAction("Index");
		}
	}
}
