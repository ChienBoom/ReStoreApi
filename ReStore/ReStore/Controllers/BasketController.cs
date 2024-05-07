
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReStore.Data;
using ReStore.DTOs;
using ReStore.Entities;

namespace ReStore.Controllers
{
    public class BasketController : BaseApiController
    {
        public StoreContext _context;
        public BasketController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetBasket")]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var basket = await RetrieveBasket();
            Console.WriteLine("basket: " + basket);
            if (basket == null) return NotFound();
            return MapBasketToBasketDto(basket);

        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> AddBasket(int productId, int quantity)
        {
            var basket = await RetrieveBasket();
            if (basket == null) basket = CreateBasket();
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();
            basket.AddItem(product, quantity);
            var result = await _context.SaveChangesAsync() > 0;
            if (result) return CreatedAtRoute("GetBasket", MapBasketToBasketDto(basket));
            return BadRequest(new ProblemDetails { Title = "Problem saving item to basket" });
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveBasket(int productId, int quantity)
        {
            var basket = await RetrieveBasket();
            if (basket == null) return NotFound();
            basket.RemoveItem(productId, quantity);
            var result = await _context.SaveChangesAsync() > 0;
            if (result) return Ok();
            return BadRequest(new ProblemDetails { Title = "Problem removing item from the basket" });
        }

        private async Task<Basket> RetrieveBasket()
        {
            // var buyerId = Request.Cookies["buyerId"];
            var buyerId = "a79ff52e-abff-4fa7-b32f-67a5a3666304";
            Console.WriteLine("buyerId: " + buyerId);
            return await _context.Baskets
            .Include(item => item.Items)
            .ThenInclude(item => item.Product)
            .FirstOrDefaultAsync(item => item.BuyerId == buyerId);
        }
        private Basket CreateBasket()
        {
            var buyerId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions { IsEssential = true, Expires = DateTime.Now.AddDays(30) };
            Response.Cookies.Append("buyerId", buyerId, cookieOptions);
            var basket = new Basket { BuyerId = buyerId };
            _context.Baskets.Add(basket);
            return basket;
        }

        private BasketDto MapBasketToBasketDto(Basket basket)
        {
            return new BasketDto
            {
                Id = basket.Id,
                BuyerId = basket.BuyerId,
                Items = basket.Items.Select(item => new BasketItemDto
                {
                    ProductId = item.ProductId,
                    Name = item.Product.Name,
                    Price = item.Product.Price,
                    PictureUrl = item.Product.PictureUrl,
                    Type = item.Product.Type,
                    Brand = item.Product.Brand,
                    Quantity = item.Quantity
                }).ToList()
            };
        }

    }
}