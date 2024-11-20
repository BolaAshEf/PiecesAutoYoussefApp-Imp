using SharedORMAppsModels.DB.Base;

namespace PiecesAutoYoussefApp.Models
{
    public class Order : DBObjIntKeyed, IEquatable<Order?>
    {
        public Order(Product product, int quantity, decimal unitPrice, decimal vatRate, DateTime orderDate, bool updateProductsThen)
        {
            Product = product;
            Quantity = quantity;
            UnitPrice = unitPrice;
            VatRate = vatRate;
            OrderDate = orderDate;
            UpdateProductsThen = updateProductsThen;
        }

        public Product Product { get; init; }
        public int Quantity { get; init; }
        public decimal UnitPrice { get; init; }
        public decimal VatRate { get; init; }
        public decimal NoTaxTotalPrice => UnitPrice * Quantity;
        public decimal TotalPrice => NoTaxTotalPrice * (1 + VatRate);
        public DateTime OrderDate { get; init; }
        public bool UpdateProductsThen { get; init; }



        public override bool Equals(object? obj)
        {
            return Equals(obj as Order);
        }

        public bool Equals(Order? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   EqualityComparer<Product>.Default.Equals(Product, other.Product) &&
                   Quantity == other.Quantity &&
                   UnitPrice == other.UnitPrice &&
                   VatRate == other.VatRate &&
                   NoTaxTotalPrice == other.NoTaxTotalPrice &&
                   TotalPrice == other.TotalPrice &&
                   OrderDate == other.OrderDate &&
                   UpdateProductsThen == other.UpdateProductsThen;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(base.GetHashCode());
            hash.Add(Product);
            hash.Add(Quantity);
            hash.Add(UnitPrice);
            hash.Add(VatRate);
            hash.Add(NoTaxTotalPrice);
            hash.Add(TotalPrice);
            hash.Add(OrderDate);
            hash.Add(UpdateProductsThen);
            return hash.ToHashCode();
        }

        public static bool operator ==(Order? left, Order? right)
        {
            return EqualityComparer<Order>.Default.Equals(left, right);
        }

        public static bool operator !=(Order? left, Order? right)
        {
            return !(left == right);
        }
    }
}
