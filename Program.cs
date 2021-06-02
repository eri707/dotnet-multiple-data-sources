using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace dotnet_multiple_dataresources
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (StreamReader r = new StreamReader("./products.json"))
                {
                    var jsonString = r.ReadToEnd();
                    var productsList = JsonSerializer.Deserialize<List<Product>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    using (StreamReader r2 = new StreamReader("./productPrices.json"))
                    {
                        var jsonString2 = r2.ReadToEnd();
                        var priceList = JsonSerializer.Deserialize<List<ProductPrice>>(jsonString2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        using (StreamReader r3 = new StreamReader("./productDiscounts.json"))
                        {
                            var jsonString3 = r3.ReadToEnd();
                            var discountList = JsonSerializer.Deserialize<List<ProductDiscount>>(jsonString3, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            // creates productConmposites with the new formation(new ProductComposite) using Select 
                            var productConposites = productsList.Select(product => new ProductComposite
                            {   // when using Select, the contents{} is always only property
                                ProductId = product.ProductId,
                                ProductName = product.ProductName,
                                Category = product.Category,
                                // find the prices which correspond with ProductId
                                Price = priceList?.Find(price => price.ProductId == product.ProductId)?.Price,
                                // find the discounts which correspond with ProductId
                                Discount = discountList?.Find(discount => discount.ProductId == product.ProductId)?.Discount
                            });
                            var serialize = JsonSerializer.Serialize(productConposites, new JsonSerializerOptions { WriteIndented = true });
                            using (FileStream f = new FileStream("./productsComposites.json", FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                using (StreamWriter w = new StreamWriter(f))
                                {
                                    w.WriteLine(serialize);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
