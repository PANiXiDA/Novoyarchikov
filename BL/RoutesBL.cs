using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal;
using Common.Enums;
using Common.Search;
using Route = Entities.Route;

namespace BL
{
	public class RoutesBL
	{
		public async Task<int> AddOrUpdateAsync(Route entity)
		{
			entity.RouteId = await new RoutesDal().AddOrUpdateAsync(entity);
			return entity.RouteId;
		}

		public Task<bool> ExistsAsync(int id)
		{
			return new RoutesDal().ExistsAsync(id);
		}

		public Task<bool> ExistsAsync(RoutesSearchParams searchParams)
		{
			return new RoutesDal().ExistsAsync(searchParams);
		}

		public Task<Route> GetAsync(int id)
		{
			return new RoutesDal().GetAsync(id);
		}

		public Task<bool> DeleteAsync(int id)
		{
			return new RoutesDal().DeleteAsync(id);
		}

		public Task<SearchResult<Route>> GetAsync(RoutesSearchParams searchParams)
		{
			return new RoutesDal().GetAsync(searchParams);
		}
	}
}

