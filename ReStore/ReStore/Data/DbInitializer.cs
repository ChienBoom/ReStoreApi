

using ReStore.Entities;

namespace ReStore.Data
{
    public static class DbInitializer
    {
        public static void Initialize(StoreContext context)
        {
            if (context.Products.Any()) return;

            var products = new List<Product>
            {
                new Product{
                    Name = "Angular Purple Boots",
                    Description = "Aenean nec lorem",
                    Price = 15000,
                    PictureUrl = "images/products/boot-ang2.png",
                    Brand = "Angular",
                    Type = "Boots",
                    QuantityInStock = 100
                },
                new Product{
                    Name = "Angular Blue Boots",
                    Description = "Aenean nec lorem",
                    Price = 18000,
                    PictureUrl = "images/products/boot-ang2.png",
                    Brand = "Angular",
                    Type = "Boots",
                    QuantityInStock = 100
                }
            };

            foreach (var product in products)
            {
                context.Products.Add(product);
            }

            // context.Products.AddRange(products);

            context.SaveChanges();
        }
    }
}