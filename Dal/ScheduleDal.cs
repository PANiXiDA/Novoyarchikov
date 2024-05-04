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
	public class ScheduleDal : BaseDal<DefaultDbContext, Schedule, Entities.Schedule, int, ScheduleSearchParams, object>
	{
		protected override bool RequiresUpdatesAfterObjectSaving => false;

		public ScheduleDal()
		{
		}

		protected internal ScheduleDal(DefaultDbContext context) : base(context)
		{
		}

		protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Entities.Schedule entity, Schedule dbObject, bool exists)
		{
			dbObject.ScheduleId = entity.ScheduleId;
			dbObject.RouteId = entity.RouteId;
			dbObject.StopId = entity.StopId;
			dbObject.DepartureTime = entity.DepartureTime;
			dbObject.ArrivalTime = entity.ArrivalTime;
			return Task.CompletedTask;
		}
	
		protected override Task<IQueryable<Schedule>> BuildDbQueryAsync(DefaultDbContext context, IQueryable<Schedule> dbObjects, ScheduleSearchParams searchParams)
		{
			return Task.FromResult(dbObjects);
		}

		protected override async Task<IList<Entities.Schedule>> BuildEntitiesListAsync(DefaultDbContext context, IQueryable<Schedule> dbObjects, object convertParams, bool isFull)
		{
			return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
		}

		protected override Expression<Func<Schedule, int>> GetIdByDbObjectExpression()
		{
			return item => item.ScheduleId;
		}

		protected override Expression<Func<Entities.Schedule, int>> GetIdByEntityExpression()
		{
			return item => item.ScheduleId;
		}

		internal static Entities.Schedule ConvertDbObjectToEntity(Schedule dbObject)
		{
			return dbObject == null ? null : new Entities.Schedule(dbObject.ScheduleId, dbObject.RouteId,
				dbObject.StopId, dbObject.DepartureTime, dbObject.ArrivalTime);
		}
	}
}
