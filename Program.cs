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
                using (StreamReader productReader = new StreamReader("./products.json"))
                using (StreamReader priceReader = new StreamReader("./productPrices.json"))
                using (StreamReader discountReader = new StreamReader("./productDiscounts.json"))
                using (FileStream fileStream = new FileStream("./productsComposites.json", FileMode.Create, FileAccess.Write, FileShare.None))
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    var jsonString = productReader.ReadToEnd();
                    var productsList = JsonSerializer.Deserialize<List<Product>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    var jsonString2 = priceReader.ReadToEnd();
                    var priceList = JsonSerializer.Deserialize<List<ProductPrice>>(jsonString2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    var jsonString3 = discountReader.ReadToEnd();
                    var discountList = JsonSerializer.Deserialize<List<ProductDiscount>>(jsonString3, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    // creates productConmposites with the new formation(new ProductComposite) using Select 
                    var productConposites = productsList.Select(product => new ProductComposite
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        Category = product.Category,
                        // find the prices which correspond with ProductId
                        Price = priceList?.Find(price => price.ProductId == product.ProductId)?.Price,
                        // find the discounts which correspond with ProductId
                        Discount = discountList?.Find(discount => discount.ProductId == product.ProductId)?.Discount
                    });
                    var serialize = JsonSerializer.Serialize(productConposites, new JsonSerializerOptions { WriteIndented = true });
                    streamWriter.WriteLine(serialize);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
