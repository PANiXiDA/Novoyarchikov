﻿using System;
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
	public class StopsController : Controller
	{
		public async Task<IActionResult> Index(int page = 1)
		{
			const int objectsPerPage = 20;
			var searchResult = await new StopsBL().GetAsync(new StopsSearchParams
			{
				StartIndex = (page - 1) * objectsPerPage,
				ObjectsCount = objectsPerPage,
			});
			var viewModel = new SearchResultViewModel<StopModel>(StopModel.FromEntitiesList(searchResult.Objects), 
				searchResult.Total, searchResult.RequestedStartIndex, searchResult.RequestedObjectsCount, 5);
			return View(viewModel);
		}

		public async Task<IActionResult> Update(int? id)
		{
			var model = new StopModel();
			if (id != null)
			{
				model = StopModel.FromEntity(await new StopsBL().GetAsync(id.Value));
				if (model == null)
					return NotFound();
			}
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(StopModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			await new StopsBL().AddOrUpdateAsync(StopModel.ToEntity(model));
			TempData[OperationResultType.Success.ToString()] = "Данные сохранены";
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(int id)
		{
			const int objectsPerPage = 100;

			var searchResultSchedules = await new ScheduleBL().GetAsync(new ScheduleSearchParams
			{
				StartIndex = 0,
				ObjectsCount = objectsPerPage,
			});
			var scheduleModel = new SearchResultViewModel<ScheduleModel>(ScheduleModel.FromEntitiesList(searchResultSchedules.Objects),
				searchResultSchedules.Total, searchResultSchedules.RequestedStartIndex, searchResultSchedules.RequestedObjectsCount, 5);

			if (scheduleModel.Objects.Where(x => x.StopId == id).FirstOrDefault() != null)
			{
				TempData[OperationResultType.Error.ToString()] = "Нельзя удалить, так как к этой остановке привязано расписание";
			}
			else
			{
				var result = await new StopsBL().DeleteAsync(id);
				if (result)
					TempData[OperationResultType.Success.ToString()] = "Объект удален";
				else
					TempData[OperationResultType.Error.ToString()] = "Объект не найден";
			}
			return RedirectToAction("Index");}
		}
	}

