using System;
using System.Collections.Generic;
using Common.Enums;

namespace Common.Search
{
	public class RoutesSearchParams : BaseSearchParams
	{
		public RoutesSearchParams(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
		{
		}
	}
}
