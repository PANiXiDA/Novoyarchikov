using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dal.DbModels;

public partial class DefaultDbContext : DbContext
{
    public DefaultDbContext()
    {
    }

    public DefaultDbContext(DbContextOptions<DefaultDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bus> Buses { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<RepairOrder> RepairOrders { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<RouteList> RouteLists { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Stop> Stops { get; set; }

	public virtual DbSet<User> Users { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-BJFC27S\\SQLEXPRESS;Initial Catalog=RouteTaxi;Integrated security=true;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Cyrillic_General_100_CI_AI");
		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK_Users");

			entity.ToTable("Users");

			entity.Property(e => e.Id).HasColumnName("Id");
			entity.Property(e => e.IsBlocked).HasColumnName("IsBlocked");
			entity.Property(e => e.Login).HasColumnName("Login");
			entity.Property(e => e.Password).HasColumnName("Password");
			entity.Property(e => e.RegistrationDate).HasColumnName("RegistrationDate");
			entity.Property(e => e.RoleId).HasColumnName("Role");
		});

		modelBuilder.Entity<Bus>(entity =>
        {
            entity.HasKey(e => e.BusId).HasName("PK_buses");

            entity.Property(e => e.BusId).HasColumnName("BusID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.Model).HasMaxLength(255);
            entity.Property(e => e.ReleaseDate).HasColumnType("date");

            entity.HasOne(d => d.Company).WithMany(p => p.Buses)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("companies_buses");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK_companies");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CompanyName).HasMaxLength(255);
            entity.Property(e => e.Owner).HasMaxLength(255);
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.DriverId).HasName("PK_driver");

            entity.ToTable("Driver");

            entity.Property(e => e.DriverId).HasColumnName("DriverID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.DriverName).HasMaxLength(255);

            entity.HasOne(d => d.Company).WithMany(p => p.Drivers)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("companies_driver");
        });

        modelBuilder.Entity<RepairOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK_repairOrder");

            entity.ToTable("RepairOrder");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.BusId).HasColumnName("BusID");
            entity.Property(e => e.RepairDate).HasColumnType("date");

            entity.HasOne(d => d.Bus).WithMany(p => p.RepairOrders)
                .HasForeignKey(d => d.BusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("buses_repairOrder");
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.RouteId).HasName("PK_routes");

            entity.Property(e => e.RouteId).HasColumnName("RouteID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.RouteName).HasMaxLength(255);

            entity.HasOne(d => d.Company).WithMany(p => p.Routes)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("companies_routes");
        });

        modelBuilder.Entity<RouteList>(entity =>
        {
            entity.HasKey(e => e.SheetId).HasName("PK_routeList");

            entity.ToTable("RouteList");

            entity.Property(e => e.SheetId).HasColumnName("SheetID");
            entity.Property(e => e.BusId).HasColumnName("BusID");
            entity.Property(e => e.DataRoute).HasColumnType("datetime");
            entity.Property(e => e.DriverId).HasColumnName("DriverID");
            entity.Property(e => e.RouteId).HasColumnName("RouteID");

            entity.HasOne(d => d.Bus).WithMany(p => p.RouteLists)
                .HasForeignKey(d => d.BusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("buses_route");

            entity.HasOne(d => d.Driver).WithMany(p => p.RouteLists)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("drivers_routeList");

            entity.HasOne(d => d.Route).WithMany(p => p.RouteLists)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("routes_routeList");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK_schedule");

            entity.ToTable("Schedule");

            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.RouteId).HasColumnName("RouteID");
            entity.Property(e => e.StopId).HasColumnName("StopID");

            entity.HasOne(d => d.Route).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("routes_schedule");

            entity.HasOne(d => d.Stop).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.StopId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stops_schedule");
        });

        modelBuilder.Entity<Stop>(entity =>
        {
            entity.HasKey(e => e.StopId).HasName("PK_stops");

            entity.Property(e => e.StopId).HasColumnName("StopID");
            entity.Property(e => e.StopName).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
