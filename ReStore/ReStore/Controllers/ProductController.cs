using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReStore.Data;
using ReStore.Entities;

namespace ReStore.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly StoreContext _context;
        public ProductController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> getProduct(int id)
        {
            var product = await _context.Products.Where(o => o.Id == id).FirstOrDefaultAsync();
            if (product == null) return NotFound();
            return Ok(product);
        }

    }
}