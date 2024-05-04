using System;
using System.Collections.Generic;

namespace Dal.DbModels;

public partial class Company
{
    public int CompanyId { get; set; }

    public string CompanyName { get; set; }

    public string Owner { get; set; }

    public virtual ICollection<Bus> Buses { get; set; } = new List<Bus>();

    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();

    public virtual ICollection<Route> Routes { get; set; } = new List<Route>();
}
