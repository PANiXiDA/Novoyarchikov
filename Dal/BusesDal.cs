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
	public class BusesDal : BaseDal<DefaultDbContext, Bus, Entities.Bus, int, BusesSearchParams, object>
	{
		protected override bool RequiresUpdatesAfterObjectSaving => false;

		public BusesDal()
		{
		}

		protected internal BusesDal(DefaultDbContext context) : base(context)
		{
		}

		protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Entities.Bus entity, Bus dbObject, bool exists)
		{
			dbObject.BusId = entity.BusId;
			dbObject.BusNumber = entity.BusNumber;
			dbObject.CompanyId = entity.CompanyId;
			dbObject.NumberOfSeats = entity.NumberOfSeats;
			dbObject.Model = entity.Model;
			dbObject.ReleaseDate = entity.ReleaseDate;
			return Task.CompletedTask;
		}
	
		protected override Task<IQueryable<Bus>> BuildDbQueryAsync(DefaultDbContext context, IQueryable<Bus> dbObjects, BusesSearchParams searchParams)
		{
			return Task.FromResult(dbObjects);
		}

		protected override async Task<IList<Entities.Bus>> BuildEntitiesListAsync(DefaultDbContext context, IQueryable<Bus> dbObjects, object convertParams, bool isFull)
		{
			return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
		}

		protected override Expression<Func<Bus, int>> GetIdByDbObjectExpression()
		{
			return item => item.BusId;
		}

		protected override Expression<Func<Entities.Bus, int>> GetIdByEntityExpression()
		{
			return item => item.BusId;
		}

		internal static Entities.Bus ConvertDbObjectToEntity(Bus dbObject)
		{
			return dbObject == null ? null : new Entities.Bus(dbObject.BusId, dbObject.BusNumber, dbObject.CompanyId,
				dbObject.NumberOfSeats, dbObject.Model, dbObject.ReleaseDate);
		}
	}
}
