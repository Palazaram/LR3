namespace NewWebShopApp.Models
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public string img { get; set; }
        public ushort price { get; set; }
        public ushort box { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
