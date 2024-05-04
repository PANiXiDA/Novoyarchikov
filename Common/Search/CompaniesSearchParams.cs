using System;
using System.Collections.Generic;
using Common.Enums;

namespace Common.Search
{
	public class CompaniesSearchParams : BaseSearchParams
	{
		public CompaniesSearchParams(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
		{
		}
	}
}
