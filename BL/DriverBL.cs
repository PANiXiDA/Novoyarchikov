using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal;
using Common.Enums;
using Common.Search;
using Driver = Entities.Driver;

namespace BL
{
	public class DriverBL
	{
		public async Task<int> AddOrUpdateAsync(Driver entity)
		{
			entity.DriverId = await new DriverDal().AddOrUpdateAsync(entity);
			return entity.DriverId;
		}

		public Task<bool> ExistsAsync(int id)
		{
			return new DriverDal().ExistsAsync(id);
		}

		public Task<bool> ExistsAsync(DriverSearchParams searchParams)
		{
			return new DriverDal().ExistsAsync(searchParams);
		}

		public Task<Driver> GetAsync(int id)
		{
			return new DriverDal().GetAsync(id);
		}

		public Task<bool> DeleteAsync(int id)
		{
			return new DriverDal().DeleteAsync(id);
		}

		public Task<SearchResult<Driver>> GetAsync(DriverSearchParams searchParams)
		{
			return new DriverDal().GetAsync(searchParams);
		}
	}
}

