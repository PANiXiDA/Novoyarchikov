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
using UI.Areas.Public.Models;

namespace UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = nameof(UserRole.Admin))]
	public class CompaniesController : Controller
	{
		public async Task<IActionResult> Index(int page = 1)
		{
			const int objectsPerPage = 20;
			var searchResult = await new CompaniesBL().GetAsync(new CompaniesSearchParams
			{
				StartIndex = (page - 1) * objectsPerPage,
				ObjectsCount = objectsPerPage,
			});
			var viewModel = new SearchResultViewModel<CompanyModel>(CompanyModel.FromEntitiesList(searchResult.Objects), 
				searchResult.Total, searchResult.RequestedStartIndex, searchResult.RequestedObjectsCount, 5);
			return View(viewModel);
		}

		public async Task<IActionResult> Update(int? id)
		{
			var model = new CompanyModel();
			if (id != null)
			{
				model = CompanyModel.FromEntity(await new CompaniesBL().GetAsync(id.Value));
				if (model == null)
					return NotFound();
			}
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(CompanyModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			await new CompaniesBL().AddOrUpdateAsync(CompanyModel.ToEntity(model));
			TempData[OperationResultType.Success.ToString()] = "Данные сохранены";
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(int id)
		{
			const int objectsPerPage = 100;

			var searchResultBuses = await new BusesBL().GetAsync(new BusesSearchParams
			{
				StartIndex = 0,
				ObjectsCount = objectsPerPage,
			});
			var busModel = new SearchResultViewModel<BusModel>(BusModel.FromEntitiesList(searchResultBuses.Objects),
				searchResultBuses.Total, searchResultBuses.RequestedStartIndex, searchResultBuses.RequestedObjectsCount, 5);

			var searchResultRoutes = await new RoutesBL().GetAsync(new RoutesSearchParams
			{
				StartIndex = 0,
				ObjectsCount = objectsPerPage,
			});
			var routeModel = new SearchResultViewModel<RouteModel>(RouteModel.FromEntitiesList(searchResultRoutes.Objects),
				searchResultRoutes.Total, searchResultRoutes.RequestedStartIndex, searchResultRoutes.RequestedObjectsCount, 5);

			var searchResultDrivers = await new DriverBL().GetAsync(new DriverSearchParams
			{
				StartIndex = 0,
				ObjectsCount = objectsPerPage,
			});
			var driverModel = new SearchResultViewModel<DriverModel>(DriverModel.FromEntitiesList(searchResultDrivers.Objects),
				searchResultDrivers.Total, searchResultDrivers.RequestedStartIndex, searchResultDrivers.RequestedObjectsCount, 5);

			if (busModel.Objects.Where(x => x.CompanyId == id).FirstOrDefault() != null)
			{
				TempData[OperationResultType.Error.ToString()] = "Нельзя удалить, так как к этой компании привязан автобус";
			}
			else if (routeModel.Objects.Where(x => x.CompanyId == id).FirstOrDefault() != null)
			{
				TempData[OperationResultType.Error.ToString()] = "Нельзя удалить, так как к этой компании привязан маршрут";
			}
			else if (driverModel.Objects.Where(x => x.CompanyId == id).FirstOrDefault() != null)
			{
				TempData[OperationResultType.Error.ToString()] = "Нельзя удалить, так как к этой компании привязан водитель";
			}
			else
			{
				var result = await new CompaniesBL().DeleteAsync(id);
				if (result)
					TempData[OperationResultType.Success.ToString()] = "Объект удален";
				else
					TempData[OperationResultType.Error.ToString()] = "Объект не найден";
			}
			return RedirectToAction("Index");
		}
	}
}
