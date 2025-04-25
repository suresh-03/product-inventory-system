using product_inventory_system.Models;
using Microsoft.EntityFrameworkCore;


namespace product_inventory_system.Data;


public class AppDbContext : DbContext{

	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){

	}

	public DbSet<Product> Product {get; set;}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
    	modelBuilder.Entity<Product>()
        	.HasIndex(p => new { p.ProductName, p.Category })
        	.IsUnique()
        	.HasDatabaseName("UQ_ProductName_Category");

    	base.OnModelCreating(modelBuilder); // Optional but good practice
	}

}