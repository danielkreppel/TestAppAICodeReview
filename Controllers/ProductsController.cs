using Microsoft.AspNetCore.Mvc;
using TestAppAICodeReview.Data;
using TestAppAICodeReview.Models;
using System.Collections.Generic;
using System.Linq;

namespace TestAppAICodeReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return _context.Products.ToList();
        }

        [HttpPost]
        public ActionResult<Product> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
        }
    }
}
