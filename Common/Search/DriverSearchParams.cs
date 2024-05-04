using System;
using System.Collections.Generic;
using Common.Enums;

namespace Common.Search
{
	public class DriverSearchParams : BaseSearchParams
	{
		public DriverSearchParams(int startIndex = 0, int? objectsCount = null) : base(startIndex, objectsCount)
		{
		}
	}
}
