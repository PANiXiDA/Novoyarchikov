using System;
using System.Collections.Generic;

namespace Dal.DbModels;

public partial class RouteList
{
    public int SheetId { get; set; }

    public int BusId { get; set; }

    public int DriverId { get; set; }

    public int RouteId { get; set; }

    public DateTime? DataRoute { get; set; }

    public virtual Bus Bus { get; set; }

    public virtual Driver Driver { get; set; }

    public virtual Route Route { get; set; }
}
