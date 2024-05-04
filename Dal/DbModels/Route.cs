using System;
using System.Collections.Generic;

namespace Dal.DbModels;

public partial class Route
{
    public int RouteId { get; set; }

    public int CompanyId { get; set; }

    public string RouteName { get; set; }

    public int? RouteDuration { get; set; }

    public virtual Company Company { get; set; }

    public virtual ICollection<RouteList> RouteLists { get; set; } = new List<RouteList>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
