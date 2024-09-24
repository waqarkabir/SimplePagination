namespace WebApp.Models
{
    // Models/Product.cs
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }

    public class ProductViewModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }

    // Models/Category.cs
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }

    public class CategoryViewModel
    {
        public string Name { get; set; }

    }

}
