﻿using System;
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
	public class RouteListDal : BaseDal<DefaultDbContext, RouteList, Entities.RouteList, int, RouteListSearchParams, object>
	{
		protected override bool RequiresUpdatesAfterObjectSaving => false;

		public RouteListDal()
		{
		}

		protected internal RouteListDal(DefaultDbContext context) : base(context)
		{
		}

		protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Entities.RouteList entity, RouteList dbObject, bool exists)
		{
			dbObject.SheetId = entity.SheetId;
			dbObject.BusId = entity.BusId;
			dbObject.DriverId = entity.DriverId;
			dbObject.RouteId = entity.RouteId;
			dbObject.DataRoute = entity.DataRoute;
			return Task.CompletedTask;
		}
	
		protected override Task<IQueryable<RouteList>> BuildDbQueryAsync(DefaultDbContext context, IQueryable<RouteList> dbObjects, RouteListSearchParams searchParams)
		{
			return Task.FromResult(dbObjects);
		}

		protected override async Task<IList<Entities.RouteList>> BuildEntitiesListAsync(DefaultDbContext context, IQueryable<RouteList> dbObjects, object convertParams, bool isFull)
		{
			return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
		}

		protected override Expression<Func<RouteList, int>> GetIdByDbObjectExpression()
		{
			return item => item.SheetId;
		}

		protected override Expression<Func<Entities.RouteList, int>> GetIdByEntityExpression()
		{
			return item => item.SheetId;
		}

		internal static Entities.RouteList ConvertDbObjectToEntity(RouteList dbObject)
		{
			return dbObject == null ? null : new Entities.RouteList(dbObject.SheetId, dbObject.BusId,
				dbObject.DriverId, dbObject.RouteId, dbObject.DataRoute);
		}
	}
}
