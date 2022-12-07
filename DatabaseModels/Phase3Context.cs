using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Phase3Databases.DatabaseModels;

public partial class Phase3Context : DbContext
{
    public Phase3Context()
    {
    }

    public Phase3Context(DbContextOptions<Phase3Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Courier> Couriers { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemsOrdered> ItemsOrdereds { get; set; }

    public virtual DbSet<JobRole> JobRoles { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<PickList> PickLists { get; set; }

    public virtual DbSet<PickWalk> PickWalks { get; set; }

    public virtual DbSet<RegisteredCar> RegisteredCars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=129.146.191.238;uid=phase3;pwd=Test!Password123;database=PHASE3", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.30-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Courier>(entity =>
        {
            entity.HasKey(e => e.CourierId).HasName("PRIMARY");

            entity.ToTable("COURIER");

            entity.Property(e => e.CourierId)
                .ValueGeneratedNever()
                .HasColumnName("Courier_ID");
            entity.Property(e => e.CurrentLocation)
                .HasMaxLength(32)
                .HasColumnName("Current_Location");
            entity.Property(e => e.FirstName)
                .HasMaxLength(32)
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(32)
                .HasColumnName("Last_Name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("Phone_Number");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PRIMARY");

            entity.ToTable("CUSTOMER");

            entity.Property(e => e.CustomerId)
                .ValueGeneratedNever()
                .HasColumnName("Customer_ID");
            entity.Property(e => e.BirthDate).HasColumnName("Birth_Date");
            entity.Property(e => e.DeliveryLocation)
                .HasMaxLength(32)
                .HasColumnName("Delivery_Location");
            entity.Property(e => e.FirstName)
                .HasMaxLength(32)
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(32)
                .HasColumnName("Last_Name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("Phone_Number");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PRIMARY");

            entity.ToTable("EMPLOYEE");

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedNever()
                .HasColumnName("Employee_ID");
            entity.Property(e => e.CumulativePickrate).HasColumnName("Cumulative_Pickrate");
            entity.Property(e => e.FirstName)
                .HasMaxLength(32)
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(32)
                .HasColumnName("Last_Name");
            entity.Property(e => e.Salary).HasPrecision(13, 4);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PRIMARY");

            entity.ToTable("ITEM");

            entity.Property(e => e.ItemId)
                .ValueGeneratedNever()
                .HasColumnName("Item_ID");
            entity.Property(e => e.AgeRequirement).HasColumnName("Age_Requirement");
            entity.Property(e => e.DepartmentNumber).HasColumnName("Department_Number");
            entity.Property(e => e.Price)
                .HasPrecision(13, 4)
                .HasColumnName("PRICE");
            entity.Property(e => e.QuantityAvailable).HasColumnName("Quantity_Available");
            entity.Property(e => e.ShelfLocation).HasColumnName("Shelf_Location");
        });

        modelBuilder.Entity<ItemsOrdered>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ItemId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("ITEMS_ORDERED");

            entity.HasIndex(e => e.ItemId, "Item_ID");

            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.ItemId).HasColumnName("Item_ID");
            entity.Property(e => e.QuantityRequested).HasColumnName("Quantity_Requested");

            entity.HasOne(d => d.Item).WithMany(p => p.ItemsOrdereds)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ITEMS_ORDERED_ibfk_2");

            entity.HasOne(d => d.Order).WithMany(p => p.ItemsOrdereds)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ITEMS_ORDERED_ibfk_1");
        });

        modelBuilder.Entity<JobRole>(entity =>
        {
            entity.HasKey(e => new { e.EmployeeId, e.RoleName, e.DepartmentNumber })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("JOB_ROLES");

            entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(32)
                .HasColumnName("Role_Name");
            entity.Property(e => e.DepartmentNumber).HasColumnName("Department_Number");

            entity.HasOne(d => d.Employee).WithMany(p => p.JobRoles)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("JOB_ROLES_ibfk_1");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("ORDERS");

            entity.HasIndex(e => e.CourierId, "Courier_ID");

            entity.HasIndex(e => e.CustomerId, "Customer_ID");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("Order_ID");
            entity.Property(e => e.CourierId).HasColumnName("Courier_ID");
            entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");
            entity.Property(e => e.DeliveryTime)
                .HasColumnType("datetime")
                .HasColumnName("Delivery_Time");
            entity.Property(e => e.DistanceRemaining).HasColumnName("Distance_Remaining");
            entity.Property(e => e.EstimatedDeliveryTime)
                .HasColumnType("datetime")
                .HasColumnName("Estimated_Delivery_Time");
            entity.Property(e => e.HoursElapsed).HasColumnName("Hours_Elapsed");
            entity.Property(e => e.OrderStatus).HasColumnName("Order_Status");
            entity.Property(e => e.OrderTimestamp)
                .HasColumnType("datetime")
                .HasColumnName("Order_Timestamp");
            entity.Property(e => e.OrderTotal)
                .HasPrecision(13, 4)
                .HasColumnName("Order_Total");
            entity.Property(e => e.PickupTime)
                .HasColumnType("datetime")
                .HasColumnName("Pickup_Time");

            entity.HasOne(d => d.Courier).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CourierId)
                .HasConstraintName("ORDERS_ibfk_1");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ORDERS_ibfk_2");
        });

        modelBuilder.Entity<PickList>(entity =>
        {
            entity.HasKey(e => new { e.ItemId, e.OrderId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("PICK_LIST");

            entity.HasIndex(e => e.OrderId, "Order_ID");

            entity.HasIndex(e => new { e.StartTimestamp, e.EmployeeId }, "Start_Timestamp");

            entity.Property(e => e.ItemId).HasColumnName("Item_ID");
            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");
            entity.Property(e => e.QuantityNeeded).HasColumnName("Quantity_Needed");
            entity.Property(e => e.StartTimestamp)
                .HasColumnType("datetime")
                .HasColumnName("Start_Timestamp");

            entity.HasOne(d => d.Item).WithMany(p => p.PickLists)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PICK_LIST_ibfk_2");

            entity.HasOne(d => d.Order).WithMany(p => p.PickLists)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PICK_LIST_ibfk_3");

            entity.HasOne(d => d.PickWalk).WithMany(p => p.PickLists)
                .HasForeignKey(d => new { d.StartTimestamp, d.EmployeeId })
                .HasConstraintName("PICK_LIST_ibfk_1");
        });

        modelBuilder.Entity<PickWalk>(entity =>
        {
            entity.HasKey(e => new { e.StartTimestamp, e.EmployeeId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("PICK_WALK");

            entity.HasIndex(e => e.EmployeeId, "Employee_ID");

            entity.Property(e => e.StartTimestamp)
                .HasColumnType("datetime")
                .HasColumnName("Start_Timestamp");
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");
            entity.Property(e => e.EndTimestamp)
                .HasColumnType("datetime")
                .HasColumnName("End_Timestamp");
            entity.Property(e => e.PickRate).HasColumnName("Pick_Rate");
            entity.Property(e => e.TotalQuantityPicked).HasColumnName("Total_Quantity_Picked");
            entity.Property(e => e.WalkDuration).HasColumnName("Walk_Duration");

            entity.HasOne(d => d.Employee).WithMany(p => p.PickWalks)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PICK_WALK_ibfk_1");
        });

        modelBuilder.Entity<RegisteredCar>(entity =>
        {
            entity.HasKey(e => e.LicensePlateNumber).HasName("PRIMARY");

            entity.ToTable("REGISTERED_CARS");

            entity.HasIndex(e => e.CourierId, "Courier_ID");

            entity.Property(e => e.LicensePlateNumber)
                .HasMaxLength(8)
                .HasColumnName("LicensePlate_Number");
            entity.Property(e => e.Color)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.CourierId).HasColumnName("Courier_ID");
            entity.Property(e => e.Model).HasMaxLength(32);

            entity.HasOne(d => d.Courier).WithMany(p => p.RegisteredCars)
                .HasForeignKey(d => d.CourierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("REGISTERED_CARS_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
