namespace ProfitTest.Persistence.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; } // цена товара
        public DateTime PriceValidFrom { get; set; } // период действия цены ОТ
        public DateTime? PriceValidTo { get; set; } // период действия цены ДО
        public DateTime CreatedAt { get; set; } // дата создания товара
    }
}
