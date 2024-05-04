using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal;
using Common.Enums;
using Common.Search;
using Company = Entities.Company;

namespace BL
{
	public class CompaniesBL
	{
		public async Task<int> AddOrUpdateAsync(Company entity)
		{
			entity.CompanyId = await new CompaniesDal().AddOrUpdateAsync(entity);
			return entity.CompanyId;
		}

		public Task<bool> ExistsAsync(int id)
		{
			return new CompaniesDal().ExistsAsync(id);
		}

		public Task<bool> ExistsAsync(CompaniesSearchParams searchParams)
		{
			return new CompaniesDal().ExistsAsync(searchParams);
		}

		public Task<Company> GetAsync(int id)
		{
			return new CompaniesDal().GetAsync(id);
		}

		public Task<bool> DeleteAsync(int id)
		{
			return new CompaniesDal().DeleteAsync(id);
		}

		public Task<SearchResult<Company>> GetAsync(CompaniesSearchParams searchParams)
		{
			return new CompaniesDal().GetAsync(searchParams);
		}
	}
}

