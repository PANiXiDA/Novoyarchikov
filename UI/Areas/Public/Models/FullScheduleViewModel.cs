using BL;
using Common.Search;
using UI.Areas.Admin.Models;
using UI.Areas.Admin.Models.ViewModels;

namespace UI.Areas.Public.Models
{
	public class FullScheduleViewModel
	{
		public SearchResultViewModel<CompanyModel> companyModel { get; set; }
		public SearchResultViewModel<BusModel> busModel { get; set; }
		public SearchResultViewModel<StopModel> stopModel { get; set; }
		public SearchResultViewModel<RepairOrderModel> repairOrderModel { get; set; }
		public SearchResultViewModel<RouteListModel> routeListModel { get; set; }
		public SearchResultViewModel<DriverModel> driverModel { get; set; }
		public SearchResultViewModel<ScheduleModel> scheduleModel { get; set; }
		public SearchResultViewModel<RouteModel> routeModel { get; set; }
	}
}