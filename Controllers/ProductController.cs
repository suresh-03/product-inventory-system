using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using product_inventory_system.Models;
using product_inventory_system.Data;



namespace product_inventory_system.Controllers;

public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;
    private readonly AppDbContext _context;

    public ProductController(ILogger<ProductController> logger,AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var products = _context.Product.ToList();
        return View(products);
    }

    public IActionResult AddProductForm(){
        return View();
    }

    [HttpPost]
    public JsonResult AddProduct([FromBody] Product product){
        try{
            if(product == null){
                Response.StatusCode = 400;
                return Json(new {success = false,message = "Product should not be null!"});
            }

            var result = ValidateProduct(product);
            if(result != null){
                Response.StatusCode = 400;
                return result;
            }

            bool exists = CheckProductExists(product);
            if(exists){
                Response.StatusCode = 409;
                return Json(new {success = false,message = "Product Already Exists!"});
            }
            _context.Product.Add(product);
            _context.SaveChanges();
            Response.StatusCode = 200;
            return Json(new {success = true,redirectUrl = Url.Action("Index","Product"), message = "Product Added Successfully"});
        }
        catch(Exception e){
            Response.StatusCode = 500;
            _logger.LogError(e,"Unexpected error occurred while adding product to the database");
            return Json(new {success = false,message = "Internal Server Error"});
        }
        
    }

    public IActionResult EditProductForm(int Id){
        var product = _context.Product.Find(Id);
        if(product == null){
            Response.StatusCode = 404;
            return Json(new {success = false,message = "Product not Found!"});
        }

        return View("AddProductForm",product);
    }

    [HttpPost]
    public JsonResult EditProduct([FromBody] Product product,int Id){
        try{
            if(product == null){
                Response.StatusCode = 400;
                return Json(new {success = false,message = "Product and Id should not be null!"});
            }

            var result = ValidateProduct(product);
            if(result != null){
                Response.StatusCode = 400;
                return result;
            }

            product.Id = Id;
            _context.Product.Update(product); 
            _context.SaveChanges();
            Response.StatusCode = 200;
            return Json(new {success = true,redirectUrl = Url.Action("Index","Product"),message = "Product Updated Successfully"});
        }
        catch(Exception e){
            Response.StatusCode = 500;
            _logger.LogError(e,"Unexpected error occurred while updating product in the database");
            return Json(new {success = false,message = "Internal Server Error"});
        }
    }

    public IActionResult ProductDetails(int Id){
        var product = _context.Product
        .FirstOrDefault(p => p.Id == Id);

        return View(product);
    }

    [HttpDelete]
    public JsonResult DeleteProduct(int Id){
        try{
        var product = _context.Product.Find(Id);

        if (product == null)
        {
            Response.StatusCode = 404;
            return Json(new { success = false, message = "Product not found!" });
        }

        _context.Product.Remove(product);
        _context.SaveChanges();
        Response.StatusCode = 200;
        return Json(new { success = true, redirectUrl = Url.Action("Index","Product"), message = "Product Deleted Successfully"});
        }
        catch(Exception e){
            Response.StatusCode = 500;
            _logger.LogError(e,"Unexpected error occurred while deleting product in the database");
            return Json(new {success = false,message = "Internal Server Error"});
        }
    }

    [HttpDelete]
    public JsonResult DeleteAll(){
        try{
            var allProducts = _context.Product.ToList();
            if(allProducts == null){
                Response.StatusCode = 404;
                return Json(new { success = false, message = "No Products in the Database" });
            }
            _context.Product.RemoveRange(allProducts);
            _context.SaveChanges();
             Response.StatusCode = 200;
            return Json(new { success = true, redirectUrl = Url.Action("Index","Product"), message = "All Products Deleted Successfully"});
        }
        catch(Exception e){
            Response.StatusCode = 500;
            _logger.LogError(e,"Unexpected error occurred while deleting all products in the database");
            return Json(new {success = false,message = "Internal Server Error"});
        }
        
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private bool CheckProductExists(Product product){
        return _context.Product.Any(p => p.ProductName == product.ProductName && p.Category == product.Category);
    }


    private bool IsValidCategory(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
            return false;

        var validCategories = new HashSet<string>
        {
            "electronics", "clothing", "books", "furniture", "toys",
            "beauty", "home", "sports", "automotive", "grocery",
            "office", "health", "jewelry", "garden", "pet"
        };
        
        return validCategories.Contains(category.ToLower());
    }

    
    private JsonResult ValidateProduct(Product product)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(product.ProductName))
            errors.Add("Product name is required.");

        if (product.Price == null || product.Price <= 0)
            errors.Add("Price must be greater than 0.");

        if (!IsValidCategory(product.Category))
            errors.Add("Category is invalid");

        if (product.Quantity == null || product.Quantity < 0)
            errors.Add("Quantity cannot be negative.");

        if (string.IsNullOrWhiteSpace(product.Description))
            errors.Add("Description is required");

        if (errors.Count > 0)
        {
            return Json(new
            {
                success = false,
                message = string.Join(", ",errors)
            });
        }

        return null;
    }

}
