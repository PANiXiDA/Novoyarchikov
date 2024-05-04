using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal;
using Common.Enums;
using Common.Search;
using RepairOrder = Entities.RepairOrder;

namespace BL
{
	public class RepairOrderBL
	{
		public async Task<int> AddOrUpdateAsync(RepairOrder entity)
		{
			entity.OrderId = await new RepairOrderDal().AddOrUpdateAsync(entity);
			return entity.OrderId;
		}

		public Task<bool> ExistsAsync(int id)
		{
			return new RepairOrderDal().ExistsAsync(id);
		}

		public Task<bool> ExistsAsync(RepairOrderSearchParams searchParams)
		{
			return new RepairOrderDal().ExistsAsync(searchParams);
		}

		public Task<RepairOrder> GetAsync(int id)
		{
			return new RepairOrderDal().GetAsync(id);
		}

		public Task<bool> DeleteAsync(int id)
		{
			return new RepairOrderDal().DeleteAsync(id);
		}

		public Task<SearchResult<RepairOrder>> GetAsync(RepairOrderSearchParams searchParams)
		{
			return new RepairOrderDal().GetAsync(searchParams);
		}
	}
}

