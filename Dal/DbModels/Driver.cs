using System;
using System.Collections.Generic;

namespace Dal.DbModels;

public partial class Driver
{
    public int DriverId { get; set; }

    public int CompanyId { get; set; }

    public string DriverName { get; set; }

    public int? Experience { get; set; }

    public int? NumberOfAccidents { get; set; }

    public virtual Company Company { get; set; }

    public virtual ICollection<RouteList> RouteLists { get; set; } = new List<RouteList>();
}
