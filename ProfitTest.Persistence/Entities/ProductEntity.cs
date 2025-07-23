namespace ProfitTest.Persistence.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime PriceValidFrom { get; set; }
        public DateTime? PriceValidTo { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
