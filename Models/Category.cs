namespace NewWebShopApp.Models
{
    public class Category
    {
        public int id { get; set; }
        public string? name { get; set; }

        public List<Product> Products { get; set; } = new();
    }
}
