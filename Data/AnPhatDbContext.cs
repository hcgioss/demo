using demo.Models;
using System.Collections.Generic;
using System.Data.Entity;

public class AnPhatDbContext : DbContext
{
    public AnPhatDbContext() : base("AnPhatDbContext") { }

    public DbSet<ProductItem> ProductItems { get; set; }
    public DbSet<WarrantyTicket> WarrantyTickets { get; set; }
    public DbSet<User> Users { get; set; }
}