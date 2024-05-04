using System;
using System.Collections.Generic;

namespace Dal.DbModels;

public partial class Stop
{
    public int StopId { get; set; }

    public string StopName { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
