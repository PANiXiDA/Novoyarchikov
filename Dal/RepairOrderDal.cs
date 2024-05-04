using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Common.Enums;
using Common.Search;
using Dal.DbModels;

namespace Dal
{
	public class RepairOrderDal : BaseDal<DefaultDbContext, RepairOrder, Entities.RepairOrder, int, RepairOrderSearchParams, object>
	{
		protected override bool RequiresUpdatesAfterObjectSaving => false;

		public RepairOrderDal()
		{
		}

		protected internal RepairOrderDal(DefaultDbContext context) : base(context)
		{
		}

		protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Entities.RepairOrder entity, RepairOrder dbObject, bool exists)
		{
			dbObject.OrderId = entity.OrderId;
			dbObject.BusId = entity.BusId;
			dbObject.RepairPrice = entity.RepairPrice;
			dbObject.RepairDate = entity.RepairDate;
			return Task.CompletedTask;
		}
	
		protected override Task<IQueryable<RepairOrder>> BuildDbQueryAsync(DefaultDbContext context, IQueryable<RepairOrder> dbObjects, RepairOrderSearchParams searchParams)
		{
			return Task.FromResult(dbObjects);
		}

		protected override async Task<IList<Entities.RepairOrder>> BuildEntitiesListAsync(DefaultDbContext context, IQueryable<RepairOrder> dbObjects, object convertParams, bool isFull)
		{
			return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
		}

		protected override Expression<Func<RepairOrder, int>> GetIdByDbObjectExpression()
		{
			return item => item.OrderId;
		}

		protected override Expression<Func<Entities.RepairOrder, int>> GetIdByEntityExpression()
		{
			return item => item.OrderId;
		}

		internal static Entities.RepairOrder ConvertDbObjectToEntity(RepairOrder dbObject)
		{
			return dbObject == null ? null : new Entities.RepairOrder(dbObject.OrderId, dbObject.BusId,
				dbObject.RepairPrice, dbObject.RepairDate);
		}
	}
}
