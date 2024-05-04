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
	public class RouteListController : Controller
	{
		public async Task<IActionResult> Index(int page = 1)
		{
			const int objectsPerPage = 20;
			var searchResult = await new RouteListBL().GetAsync(new RouteListSearchParams
			{
				StartIndex = (page - 1) * objectsPerPage,
				ObjectsCount = objectsPerPage,
			});
			var viewModel = new SearchResultViewModel<RouteListModel>(RouteListModel.FromEntitiesList(searchResult.Objects), 
				searchResult.Total, searchResult.RequestedStartIndex, searchResult.RequestedObjectsCount, 5);
			return View(viewModel);
		}

		public async Task<IActionResult> Update(int? id)
		{
			var model = new RouteListModel();
			if (id != null)
			{
				model = RouteListModel.FromEntity(await new RouteListBL().GetAsync(id.Value));
				if (model == null)
					return NotFound();
			}
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(RouteListModel model)
		{
			const int objectsPerPage = 100;

			var searchResultBuses = await new BusesBL().GetAsync(new BusesSearchParams
			{
				StartIndex = 0,
				ObjectsCount = objectsPerPage,
			});
			var busModel = new SearchResultViewModel<BusModel>(BusModel.FromEntitiesList(searchResultBuses.Objects),
				searchResultBuses.Total, searchResultBuses.RequestedStartIndex, searchResultBuses.RequestedObjectsCount, 5);

			var searchResultDrivers = await new DriverBL().GetAsync(new DriverSearchParams
			{
				StartIndex = 0,
				ObjectsCount = objectsPerPage,
			});
			var driverModel = new SearchResultViewModel<DriverModel>(DriverModel.FromEntitiesList(searchResultDrivers.Objects),
				searchResultDrivers.Total, searchResultDrivers.RequestedStartIndex, searchResultDrivers.RequestedObjectsCount, 5);

			var searchResultRoutes = await new RoutesBL().GetAsync(new RoutesSearchParams
			{
				StartIndex = 0,
				ObjectsCount = objectsPerPage,
			});
			var routeModel = new SearchResultViewModel<RouteModel>(RouteModel.FromEntitiesList(searchResultRoutes.Objects),
				searchResultRoutes.Total, searchResultRoutes.RequestedStartIndex, searchResultRoutes.RequestedObjectsCount, 5);

			if (!busModel.Objects.Any(x => x.BusId == model.BusId))
			{
				TempData[OperationResultType.Error.ToString()] = "Нельзя добавить или изменить, так как не существует автобуса по такому ID";
				return RedirectToAction("Index");
			}
			else if (!driverModel.Objects.Any(x => x.DriverId == model.DriverId))
			{
				TempData[OperationResultType.Error.ToString()] = "Нельзя добавить или изменить, так как не существует водителя по такому ID";
				return RedirectToAction("Index");
			}
			else if (!routeModel.Objects.Any(x => x.RouteId == model.RouteId))
			{
				TempData[OperationResultType.Error.ToString()] = "Нельзя добавить или изменить, так как не существует маршрута по такому ID";
				return RedirectToAction("Index");
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}
			await new RouteListBL().AddOrUpdateAsync(RouteListModel.ToEntity(model));
			TempData[OperationResultType.Success.ToString()] = "Данные сохранены";
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(int id)
		{
			var result = await new RouteListBL().DeleteAsync(id);
			if (result)
				TempData[OperationResultType.Success.ToString()] = "Объект удален";
			else
				TempData[OperationResultType.Error.ToString()] = "Объект не найден";
			return RedirectToAction("Index");
		}
	}
}
