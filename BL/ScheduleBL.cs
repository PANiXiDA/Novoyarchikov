using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal;
using Common.Enums;
using Common.Search;
using Schedule = Entities.Schedule;

namespace BL
{
	public class ScheduleBL
	{
		public async Task<int> AddOrUpdateAsync(Schedule entity)
		{
			entity.ScheduleId = await new ScheduleDal().AddOrUpdateAsync(entity);
			return entity.ScheduleId;
		}

		public Task<bool> ExistsAsync(int id)
		{
			return new ScheduleDal().ExistsAsync(id);
		}

		public Task<bool> ExistsAsync(ScheduleSearchParams searchParams)
		{
			return new ScheduleDal().ExistsAsync(searchParams);
		}

		public Task<Schedule> GetAsync(int id)
		{
			return new ScheduleDal().GetAsync(id);
		}

		public Task<bool> DeleteAsync(int id)
		{
			return new ScheduleDal().DeleteAsync(id);
		}

		public Task<SearchResult<Schedule>> GetAsync(ScheduleSearchParams searchParams)
		{
			return new ScheduleDal().GetAsync(searchParams);
		}
	}
}

