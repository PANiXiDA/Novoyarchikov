using System;
using System.Collections.Generic;
using Common.Enums;

namespace Common.Search
{
	public class ScheduleSearchParams : BaseSearchParams
	{
		public ScheduleSearchParams(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
		{
		}
	}
}
