using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using product_inventory_system.Models;
using product_inventory_system.Data;

namespace product_inventory_system.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly AppDbContext _context;

        public ProductController(ILogger<ProductController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Displays the list of products on the index page
        public IActionResult Index()
        {
            var products = _context.Product.ToList();
            return View(products);
        }

        // Returns the form view to add a new product
        public IActionResult AddProductForm()
        {
            return View();
        }

        // Handles POST request to add a new product
        [HttpPost]
        public JsonResult AddProduct([FromBody] Product product)
        {
            try
            {
                if (product == null)
                    return BadRequestJson("Product should not be null!");

                var validationResult = ValidateProduct(product);
                if (validationResult != null)
                    return validationResult;

                if (ProductAlreadyExists(product))
                    return ConflictJson("Product Already Exists!");

                _context.Product.Add(product);
                _context.SaveChanges();

                return SuccessJson("Product Added Successfully", Url.Action("Index", "Product"));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error adding product");
                return ServerErrorJson();
            }
        }

        // Loads the edit form with existing product data
        public IActionResult EditProductForm(int Id)
        {
            var product = _context.Product.Find(Id);
            if (product == null)
                return NotFoundJson("Product not found!");

            return View("AddProductForm", product);
        }

        // Handles POST request to update an existing product
        [HttpPost]
        public JsonResult EditProduct([FromBody] Product product, int Id)
        {
            try
            {
                if (product == null)
                    return BadRequestJson("Product and Id should not be null!");

                var validationResult = ValidateProduct(product);
                if (validationResult != null)
                    return validationResult;

                product.Id = Id;
                _context.Product.Update(product);
                _context.SaveChanges();

                return SuccessJson("Product Updated Successfully", Url.Action("Index", "Product"));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating product");
                return ServerErrorJson();
            }
        }

        // Returns details view of a specific product
        public IActionResult ProductDetails(int Id)
        {
            var product = _context.Product.FirstOrDefault(p => p.Id == Id);
            return View(product);
        }

        // Deletes a specific product from the database
        [HttpDelete]
        public JsonResult DeleteProduct(int Id)
        {
            try
            {
                var product = _context.Product.Find(Id);
                if (product == null)
                    return NotFoundJson("Product not found!");

                _context.Product.Remove(product);
                _context.SaveChanges();

                return SuccessJson("Product Deleted Successfully", Url.Action("Index", "Product"));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting product");
                return ServerErrorJson();
            }
        }

        // Deletes all products in the database
        [HttpDelete]
        public JsonResult DeleteAll()
        {
            try
            {
                var allProducts = _context.Product.ToList();
                if (!allProducts.Any())
                    return NotFoundJson("No Products in the Database");

                _context.Product.RemoveRange(allProducts);
                _context.SaveChanges();

                return SuccessJson("All Products Deleted Successfully", Url.Action("Index", "Product"));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting all products");
                return ServerErrorJson();
            }
        }

        // Returns generic error view
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Validates the given product object
        private JsonResult ValidateProduct(Product product)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(product.ProductName))
                errors.Add("Product name is required.");

            if (product.Price <= 0)
                errors.Add("Price must be greater than 0.");

            if (!IsValidCategory(product.Category))
                errors.Add("Category is invalid");

            if (product.Quantity < 0)
                errors.Add("Quantity cannot be negative.");

            if (string.IsNullOrWhiteSpace(product.Description))
                errors.Add("Description is required");

            return errors.Count > 0 ? Json(new { success = false, message = string.Join(", ", errors) }) : null;
        }

        // Checks whether a product already exists in the database
        private bool ProductAlreadyExists(Product product)
        {
            return _context.Product.Any(p => p.ProductName == product.ProductName && p.Category == product.Category);
        }

        // Validates if a category string is within the allowed set
        private bool IsValidCategory(string? category)
        {
            var validCategories = new HashSet<string>
            {
                "electronics", "clothing", "books", "furniture", "toys",
                "beauty", "home", "sports", "automotive", "grocery",
                "office", "health", "jewelry", "garden", "pet"
            };

            return !string.IsNullOrWhiteSpace(category) && validCategories.Contains(category.ToLower());
        }

        // Utility methods for generating consistent JSON responses
        private JsonResult SuccessJson(string message, string? redirectUrl)
        {   
            if(string.IsNullOrWhiteSpace(redirectUrl)){
                return NotFoundJson("Url not Found");
            }
            Response.StatusCode = 200;
            return Json(new { success = true, redirectUrl, message });
        }

        private JsonResult BadRequestJson(string message)
        {
            Response.StatusCode = 400;
            return Json(new { success = false, message });
        }

        private JsonResult ConflictJson(string message)
        {
            Response.StatusCode = 409;
            return Json(new { success = false, message });
        }

        private JsonResult NotFoundJson(string message)
        {
            Response.StatusCode = 404;
            return Json(new { success = false, message });
        }

        private JsonResult ServerErrorJson()
        {
            Response.StatusCode = 500;
            return Json(new { success = false, message = "Internal Server Error" });
        }
    }
}
