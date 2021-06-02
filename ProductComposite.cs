
namespace dotnet_multiple_dataresources
{
    public class ProductComposite
    {
        // don't set ? after any id
        public int ProductId { get; set; }
        // string is always nullable
        public string ProductName { get; set; }
        public string Category { get; set; }
        // the default of int and decimal are always 0
        // if int doesn't exsit, return null 
        public int? Price { get; set; }
        // if decimal doesn't exsit, return null 
        public decimal? Discount { get; set; }
    }

}
