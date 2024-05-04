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
	public class DriverDal : BaseDal<DefaultDbContext, Driver, Entities.Driver, int, DriverSearchParams, object>
	{
		protected override bool RequiresUpdatesAfterObjectSaving => false;

		public DriverDal()
		{
		}

		protected internal DriverDal(DefaultDbContext context) : base(context)
		{
		}

		protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Entities.Driver entity, Driver dbObject, bool exists)
		{
			dbObject.DriverId = entity.DriverId;
			dbObject.CompanyId = entity.CompanyId;
			dbObject.DriverName = entity.DriverName;
			dbObject.Experience = entity.Experience;
			dbObject.NumberOfAccidents = entity.NumberOfAccidents;
			return Task.CompletedTask;
		}
	
		protected override Task<IQueryable<Driver>> BuildDbQueryAsync(DefaultDbContext context, IQueryable<Driver> dbObjects, DriverSearchParams searchParams)
		{
			return Task.FromResult(dbObjects);
		}

		protected override async Task<IList<Entities.Driver>> BuildEntitiesListAsync(DefaultDbContext context, IQueryable<Driver> dbObjects, object convertParams, bool isFull)
		{
			return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
		}

		protected override Expression<Func<Driver, int>> GetIdByDbObjectExpression()
		{
			return item => item.DriverId;
		}

		protected override Expression<Func<Entities.Driver, int>> GetIdByEntityExpression()
		{
			return item => item.DriverId;
		}

		internal static Entities.Driver ConvertDbObjectToEntity(Driver dbObject)
		{
			return dbObject == null ? null : new Entities.Driver(dbObject.DriverId, dbObject.CompanyId,
				dbObject.DriverName, dbObject.Experience, dbObject.NumberOfAccidents);
		}
	}
}
