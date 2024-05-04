using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using UI.Areas.Public.Models;
using System.Data;
using Dal.DbModels;
using BL;
using Dal;
using UI.Areas.Admin.Models;
using System.Threading.Tasks;
using Entities;
using Common.Search;
using UI.Areas.Admin.Models.ViewModels;

namespace UI.Areas.Public.Controllers
{
	[Area("Public")]
	public class HomeController : Controller
	{
		static string connectionString = "Data Source=DESKTOP-BJFC27S\\SQLEXPRESS;Initial Catalog=RouteTaxi;TrustServerCertificate=True;Integrated Security=True";
		public async Task<IActionResult> IndexAsync()
		{
			const int objectsPerPage = 100;
			var searchResultCompanies = await new CompaniesBL().GetAsync(new CompaniesSearchParams
			{
				StartIndex = 0,
				ObjectsCount = objectsPerPage,
			});

			var companyModel = new SearchResultViewModel<CompanyModel>(CompanyModel.FromEntitiesList(searchResultCompanies.Objects),
				searchResultCompanies.Total, searchResultCompanies.RequestedStartIndex, searchResultCompanies.RequestedObjectsCount, 5);

			var searchResultBuses = await new BusesBL().GetAsync(new BusesSearchParams
			{
				StartIndex = 0,
				ObjectsCount = objectsPerPage,
			});
			var busModel = new SearchResultViewModel<BusModel>(BusModel.FromEntitiesList(searchResultBuses.Objects),
				searchResultBuses.Total, searchResultBuses.RequestedStartIndex, searchResultBuses.RequestedObjectsCount, 5);

			var searchResultStops = await new StopsBL().GetAsync(new StopsSearchParams
			{
				StartIndex = 0,
				ObjectsCount = objectsPerPage,
			});
			var stopModel = new SearchResultViewModel<StopModel>(StopModel.FromEntitiesList(searchResultStops.Objects),
				searchResultStops.Total, searchResultStops.RequestedStartIndex, searchResultStops.RequestedObjectsCount, 5);

			var searchResultRepairOrders = await new RepairOrderBL().GetAsync(new RepairOrderSearchParams
			{
				StartIndex = 0,
				ObjectsCount = objectsPerPage,
			});
			var repairOrderModel = new SearchResultViewModel<RepairOrderModel>(RepairOrderModel.FromEntitiesList(searchResultRepairOrders.Objects),
				searchResultRepairOrders.Total, searchResultRepairOrders.RequestedStartIndex, searchResultRepairOrders.RequestedObjectsCount, 5);

			var searchResultRouteLists = await new RouteListBL().GetAsync(new RouteListSearchParams
			{
				StartIndex = 0,
				ObjectsCount = objectsPerPage,
			});
			var routeListModel = new SearchResultViewModel<RouteListModel>(RouteListModel.FromEntitiesList(searchResultRouteLists.Objects),
				searchResultRouteLists.Total, searchResultRouteLists.RequestedStartIndex, searchResultRouteLists.RequestedObjectsCount, 5);

			var searchResultDrivers = await new DriverBL().GetAsync(new DriverSearchParams
			{
				StartIndex = 0,
				ObjectsCount = objectsPerPage,
			});
			var driverModel = new SearchResultViewModel<DriverModel>(DriverModel.FromEntitiesList(searchResultDrivers.Objects),
				searchResultDrivers.Total, searchResultDrivers.RequestedStartIndex, searchResultDrivers.RequestedObjectsCount, 5);

			var searchResultSchedules = await new ScheduleBL().GetAsync(new ScheduleSearchParams
			{
				StartIndex = 0,
				ObjectsCount = objectsPerPage,
			});
			var scheduleModel = new SearchResultViewModel<ScheduleModel>(ScheduleModel.FromEntitiesList(searchResultSchedules.Objects),
				searchResultSchedules.Total, searchResultSchedules.RequestedStartIndex, searchResultSchedules.RequestedObjectsCount, 5);

			var searchResultRoutes = await new RoutesBL().GetAsync(new RoutesSearchParams
			{
				StartIndex = 0,
				ObjectsCount = objectsPerPage,
			});
			var routeModel = new SearchResultViewModel<RouteModel>(RouteModel.FromEntitiesList(searchResultRoutes.Objects),
				searchResultRoutes.Total, searchResultRoutes.RequestedStartIndex, searchResultRoutes.RequestedObjectsCount, 5);

			FullScheduleViewModel viewModel = new FullScheduleViewModel
			{
				companyModel = companyModel,
				busModel = busModel,
				stopModel = stopModel,
				repairOrderModel = repairOrderModel,
				routeListModel = routeListModel,
				driverModel = driverModel,
				scheduleModel = scheduleModel,
				routeModel = routeModel
			};

			return View(viewModel);
		}

		[Route("robots.txt")]
		public IActionResult Robots()
		{
			string filename;
			if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
			{
				filename = "robotsDevelopment.txt";
			}
			else
			{
				filename = "robotsProduction.txt";
			}
			
			string filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filename);
			byte[] filedata = System.IO.File.ReadAllBytes(filepath);
			string contentType = "text/plain";

			return File(filedata, contentType);
		}

		public IActionResult ShowCompanyBuses(int companyId)
		{
			string sqlExpression = "ShowCompanyBuses";

			List<CompanyBusViewModel> companyBuses = new List<CompanyBusViewModel>();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				SqlCommand command = new SqlCommand(sqlExpression, connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@CompanyID", companyId);
				var reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						CompanyBusViewModel bus = new CompanyBusViewModel
						{
							CompanyName = reader.GetString(0),
							CompanyOwner = reader.GetString(1),
							BusNumber = reader.GetInt32(2),
							BusModel = reader.GetString(3),
							BusNumberOfSeats = reader.GetInt32(4),
							BusReleaseDate = reader.GetDateTime(5),
							DriverName = reader.GetString(6),
							DriverExperience = reader.GetInt32(7),
							DriverNumberOfAccidents = reader.GetInt32(8),
							RepairDate = reader.GetDateTime(9)
						};

						companyBuses.Add(bus);
					}
				}
				reader.Close();
			}

			return View(companyBuses);
		}

		public IActionResult SelectBusForRepair(string busModel, DateTime repairDate)
		{
			string sqlExpression = "SelectBusForRepair";

			List<SelectBusForRepairViewModel> BusesForRepair = new List<SelectBusForRepairViewModel>();
			int repairCount = 0;

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				SqlCommand command = new SqlCommand(sqlExpression, connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@busModel", busModel);
				command.Parameters.AddWithValue("@repairDate", repairDate);
				command.Parameters.Add("@repairCount", SqlDbType.Int).Direction = ParameterDirection.Output;

				var reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						SelectBusForRepairViewModel bus = new SelectBusForRepairViewModel
						{
							BusID = reader.GetInt32(0),
							BusNumber = reader.GetInt32(1),
							CompanyID = reader.GetInt32(2),
							NumberOfSeats = reader.GetInt32(3),
							Model = reader.GetString(4),
							ReleaseDate = reader.GetDateTime(5),
						};

						BusesForRepair.Add(bus);
					}
				}
				reader.Close();

				repairCount = (int)command.Parameters["@repairCount"].Value;
			}

			var model = new RepairViewModel
			{
				BusesForRepair = BusesForRepair,
				RepairCount = repairCount
			};

			return View(model);
		}

		public IActionResult CalculateRepairExpensesForPeriod(DateTime startDate, DateTime endDate)
		{
			string sqlExpression = "CalculateRepairExpensesForPeriod";

			int totalRepairCost = 0;

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				SqlCommand command = new SqlCommand(sqlExpression, connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.AddWithValue("@start_date", startDate);
				command.Parameters.AddWithValue("@end_date", endDate);
				command.Parameters.Add("@TotalRepairCost", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

				command.ExecuteNonQuery();

				totalRepairCost = (int)command.Parameters["@TotalRepairCost"].Value;
			}

			TempData["TotalRepairCost"] = totalRepairCost;

			return RedirectToAction("Index", null, new { area = "Public" });
		}

		public async Task<IActionResult> CalculateRouteListTotalCost(int SheetID)
		{
			string sqlExpression = "CalculateRouteListTotalCost";

			int totalCost = 0;

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				SqlCommand command = new SqlCommand(sqlExpression, connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;
				command.Parameters.Add("@RepairPrice", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
				command.Parameters.AddWithValue("@sheetId", SheetID);

				command.ExecuteNonQuery();
			}
			var routeList = RouteListModel.FromEntity(await new RouteListBL().GetAsync(SheetID));

			if (routeList != null)
			{
				var bus = BusModel.FromEntity(await new BusesBL().GetAsync(routeList.BusId));

				if (bus != null)
				{
					var repairOrder = RepairOrderModel.FromEntity(await new RepairOrderBL().GetAsync(bus.BusId));

					if (repairOrder != null)
					{
						totalCost = (int)repairOrder.RepairPrice;
					}
				}
			}
			TempData["repairPrice"] = totalCost;

			return RedirectToAction("Index", null, new { area = "Public" });
		}

		public IActionResult GetAllCompanies()
		{
			string sqlExpression = "GetAllCompanies";

			List<Dal.DbModels.Company> companies = new List<Dal.DbModels.Company>();
			List<Dal.DbModels.Bus> buses = new List<Dal.DbModels.Bus>();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				SqlCommand command = new SqlCommand(sqlExpression, connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;

				var reader = command.ExecuteReader();

				string companyName = "";
				string owner = "";

				while (reader.Read())
				{
					companyName = reader.GetString(0);
					owner = reader.GetString(1);

					Dal.DbModels.Company company = new Dal.DbModels.Company
					{
						CompanyName = companyName,
						Owner = owner
					};

					companies.Add(company);

					reader.NextResult();

					while (reader.Read())
					{
						int busNumber = reader.GetInt32(0);
						int numberOfSeats = reader.GetInt32(1);
						string model = reader.GetString(2);
						DateTime releaseDate = reader.GetDateTime(3);

						Dal.DbModels.Bus bus = new Dal.DbModels.Bus
						{
							BusNumber = busNumber,
							NumberOfSeats = numberOfSeats,
							Model = model,
							ReleaseDate = releaseDate
						};

						buses.Add(bus);
					}

					reader.NextResult();
				}
				reader.Close();
			}

			var allCompanies = new CompaniesViewModel
			{
				Companies = companies,
				Buses = buses
			};

			return View(allCompanies);
		}

		public IActionResult ShowRepairBuses()
		{
			string sqlExpression = "SELECT * FROM RepairBuses";

			List<RepairBusViewModel> repairBuses = new List<RepairBusViewModel>();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				SqlCommand command = new SqlCommand(sqlExpression, connection);

				var reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						RepairBusViewModel repairBus = new RepairBusViewModel
						{
							BusID = reader.GetInt32(0),
							RepairDate = reader.GetDateTime(1),
							Model = reader.GetString(2)
						};

						repairBuses.Add(repairBus);
					}
				}
				reader.Close();
			}

			return View(repairBuses);
		}

		public IActionResult ShowRoute()
		{
			string sqlExpression = "SELECT * FROM RouteView";

			List<RouteViewModel> routes = new List<RouteViewModel>();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				SqlCommand command = new SqlCommand(sqlExpression, connection);

				var reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						RouteViewModel route = new RouteViewModel
						{
							RouteName = reader.GetString(0),
							DriverName = reader.GetString(1),
							BusNumber = reader.GetInt32(2)
						};

						routes.Add(route);
					}
				}
				reader.Close();
			}

			return View(routes);
		}

		public IActionResult ShowSchedule()
		{
			string sqlExpression = "SELECT * FROM ScheduleView";

			List<ScheduleViewModel> schedules = new List<ScheduleViewModel>();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				SqlCommand command = new SqlCommand(sqlExpression, connection);

				var reader = command.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						ScheduleViewModel schedule = new ScheduleViewModel
						{
							StopName = reader.GetString(0),
							DepartureTime = reader.GetTimeSpan(1),
							ArrivalTime = reader.GetTimeSpan(2)
						};

						schedules.Add(schedule);
					}
				}
				reader.Close();
			}

			return View(schedules);
		}
		public IActionResult ShowBusesUpdate()
		{
			string selectSql = "SELECT * FROM BusesUpdate";
			string updateSql = "UPDATE BusesUpdate SET CompanyName = 'Good Company' WHERE NumberOfSeats = 30";

			List<BusesUpdateViewModel> buses = new List<BusesUpdateViewModel>();

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();

				SqlCommand updateCommand = new SqlCommand(updateSql, connection);
				updateCommand.ExecuteNonQuery();

				SqlCommand selectCommand = new SqlCommand(selectSql, connection);
				var reader = selectCommand.ExecuteReader();

				if (reader.HasRows)
				{
					while (reader.Read())
					{
						BusesUpdateViewModel bus = new BusesUpdateViewModel
						{
							NumberOfSeats = reader.GetInt32(0),
							CompanyName = reader.GetString(1),
							Model = reader.GetString(2),
							ReleaseDate = reader.GetDateTime(3)
						};

						buses.Add(bus);
					}
				}
				reader.Close();
			}

			return View(buses);
		}
	}
}