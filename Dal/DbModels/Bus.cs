using System;
using System.Collections.Generic;

namespace Dal.DbModels;

public partial class Bus
{
    public int BusId { get; set; }

    public int? BusNumber { get; set; }

    public int CompanyId { get; set; }

    public int? NumberOfSeats { get; set; }

    public string Model { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public virtual Company Company { get; set; }

    public virtual ICollection<RepairOrder> RepairOrders { get; set; } = new List<RepairOrder>();

    public virtual ICollection<RouteList> RouteLists { get; set; } = new List<RouteList>();
}
