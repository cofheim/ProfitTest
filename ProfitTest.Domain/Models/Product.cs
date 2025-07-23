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
                error = "�������� ������ �� ����� ���� ������";
                return (null, error);
            }

            if (price <= 0)
            {
                error = "���� ������ ���� ������ ����";
                return (null, error);
            }

            if (priceValidTo.HasValue && priceValidFrom >= priceValidTo.Value)
            {
                error = "���� ������ �������� ���� ������ ���� ������ ���� ���������";
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
                return (false, "�������� ������ �� ����� ���� ������");

            if (price <= 0)
                return (false, "���� ������ ���� ������ ����");

            if (priceValidTo.HasValue && priceValidFrom >= priceValidTo.Value)
                return (false, "���� ������ �������� ���� ������ ���� ������ ���� ���������");

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
