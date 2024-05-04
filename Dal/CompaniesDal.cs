using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Common.Enums;
using Common.Search;
using Dal.DbModels;
using Org.BouncyCastle.Utilities;

namespace Dal
{
	public class CompaniesDal : BaseDal<DefaultDbContext, Company, Entities.Company, int, CompaniesSearchParams, object>
	{
		protected override bool RequiresUpdatesAfterObjectSaving => false;
		private readonly DefaultDbContext defaultDbContext;

		public CompaniesDal()
		{
		}

		protected internal CompaniesDal(DefaultDbContext context) : base(context)
		{
		}

		protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Entities.Company entity, Company dbObject, bool exists)
		{
			dbObject.CompanyId = entity.CompanyId;
			dbObject.CompanyName = entity.CompanyName;
			dbObject.Owner = entity.Owner;
			return Task.CompletedTask;
		}
	
		protected override Task<IQueryable<Company>> BuildDbQueryAsync(DefaultDbContext context, IQueryable<Company> dbObjects, CompaniesSearchParams searchParams)
		{
			return Task.FromResult(dbObjects);
		}

		protected override async Task<IList<Entities.Company>> BuildEntitiesListAsync(DefaultDbContext context, IQueryable<Company> dbObjects, object convertParams, bool isFull)
		{
			return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
		}

		protected override Expression<Func<Company, int>> GetIdByDbObjectExpression()
		{
			return item => item.CompanyId;
		}

		protected override Expression<Func<Entities.Company, int>> GetIdByEntityExpression()
		{
			return item => item.CompanyId;
		}

		internal static Entities.Company ConvertDbObjectToEntity(Company dbObject)
		{
			return dbObject == null ? null : new Entities.Company(dbObject.CompanyId, dbObject.CompanyName,
				dbObject.Owner);
		}
	}
}
