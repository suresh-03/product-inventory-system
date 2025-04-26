using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace product_inventory_system.Models;

// Product class mapped with Product table in DB using DBContext
public class Product{

	[Key]
	[Column("PRODUCT_ID")]
	public int Id {get; set;}

	[Required]
	[Column("PRODUCT_NAME")]
	public string ProductName {get; set;}

	[Required]
	[Column("CATEGORY")]
	public string Category {get; set;}

	[Required]
	[Column("QUANTITY")]
	public int Quantity {get; set;}

	[Required]
	[Column("PRICE")]
	public double Price {get; set;} 

	[Required]
	[Column("DESCRIPTION")]
	public string Description {get; set;}


	public override string ToString(){
		return "ProductId="+Id+",ProductName="+ProductName+", Category="+Category+", Quantity="+Quantity+", Price="+Price+", Description="+Description;
	}
}