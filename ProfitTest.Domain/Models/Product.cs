namespace ProfitTest.Domain.Models
{
    public class Product
    {
        private Product(string name, decimal price, DateTime priceValidFrom, DateTime? priceValidTo = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            PriceValidFrom = priceValidFrom;
            PriceValidTo = priceValidTo;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public DateTime PriceValidFrom { get; private set; }
        public DateTime? PriceValidTo { get; private set; }
        public DateTime CreatedAt { get; }

        public static (Product? Product, string Error) Create(
            string name,
            decimal price,
            DateTime priceValidFrom,
            DateTime? priceValidTo = null)
        {
            var error = string.Empty;

            if (string.IsNullOrWhiteSpace(name))
            {
                error = "Ќазвание товара не может быть пустым";
                return (null, error);
            }

            if (price <= 0)
            {
                error = "÷ена должна быть больше нул€";
                return (null, error);
            }

            if (priceValidTo.HasValue && priceValidFrom >= priceValidTo.Value)
            {
                error = "ƒата начала действи€ цены должна быть раньше даты окончани€";
                return (null, error);
            }

            var product = new Product(name, price, priceValidFrom, priceValidTo);
            return (product, error);
        }

        public (bool Success, string Error) Update(
            string name,
            decimal price,
            DateTime priceValidFrom,
            DateTime? priceValidTo = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Ќазвание товара не может быть пустым");

            if (price <= 0)
                return (false, "÷ена должна быть больше нул€");

            if (priceValidTo.HasValue && priceValidFrom >= priceValidTo.Value)
                return (false, "ƒата начала действи€ цены должна быть раньше даты окончани€");

            Name = name;
            Price = price;
            PriceValidFrom = priceValidFrom;
            PriceValidTo = priceValidTo;

            return (true, string.Empty);
        }

        public bool IsPriceActiveAt(DateTime date)
        {
            var isAfterStart = date >= PriceValidFrom;
            var isBeforeEnd = !PriceValidTo.HasValue || date <= PriceValidTo.Value;

            return isAfterStart && isBeforeEnd;
        }
    }
}
