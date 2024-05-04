using System;
using System.Collections.Generic;

namespace Dal.DbModels;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public int RouteId { get; set; }

    public int StopId { get; set; }

    public TimeSpan? DepartureTime { get; set; }

    public TimeSpan? ArrivalTime { get; set; }

    public virtual Route Route { get; set; }

    public virtual Stop Stop { get; set; }
}
