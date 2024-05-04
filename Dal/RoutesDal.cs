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
	public class RoutesDal : BaseDal<DefaultDbContext, Route, Entities.Route, int, RoutesSearchParams, object>
	{
		protected override bool RequiresUpdatesAfterObjectSaving => false;

		public RoutesDal()
		{
		}

		protected internal RoutesDal(DefaultDbContext context) : base(context)
		{
		}

		protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Entities.Route entity, Route dbObject, bool exists)
		{
			dbObject.RouteId = entity.RouteId;
			dbObject.CompanyId = entity.CompanyId;
			dbObject.RouteName = entity.RouteName;
			dbObject.RouteDuration = entity.RouteDuration;
			return Task.CompletedTask;
		}
	
		protected override Task<IQueryable<Route>> BuildDbQueryAsync(DefaultDbContext context, IQueryable<Route> dbObjects, RoutesSearchParams searchParams)
		{
			return Task.FromResult(dbObjects);
		}

		protected override async Task<IList<Entities.Route>> BuildEntitiesListAsync(DefaultDbContext context, IQueryable<Route> dbObjects, object convertParams, bool isFull)
		{
			return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
		}

		protected override Expression<Func<Route, int>> GetIdByDbObjectExpression()
		{
			return item => item.RouteId;
		}

		protected override Expression<Func<Entities.Route, int>> GetIdByEntityExpression()
		{
			return item => item.RouteId;
		}

		internal static Entities.Route ConvertDbObjectToEntity(Route dbObject)
		{
			return dbObject == null ? null : new Entities.Route(dbObject.RouteId, dbObject.CompanyId,
				dbObject.RouteName, dbObject.RouteDuration);
		}
	}
}
