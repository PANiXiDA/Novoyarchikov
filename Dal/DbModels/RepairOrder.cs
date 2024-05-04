using System;
using System.Collections.Generic;

namespace Dal.DbModels;

public partial class RepairOrder
{
    public int OrderId { get; set; }

    public int BusId { get; set; }

    public int? RepairPrice { get; set; }

    public DateTime? RepairDate { get; set; }

    public virtual Bus Bus { get; set; }
}
