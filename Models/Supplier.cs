using SharedORMAppsModels.DB.Base;

namespace PiecesAutoYoussefApp.Models
{
    public class Supplier : DBObjIntKeyed, IEquatable<Supplier?>
    {
        public Supplier(string firstName, string lastName, string phoneNumber, string supplierAddress, Product product, int productQuantity)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            SupplierAddress = supplierAddress;
            Product = product;
            ProductQuantity = productQuantity;
        }

        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string FullName => FirstName + " " + LastName;
        public string PhoneNumber { get; init; }
        public string SupplierAddress { get; init; }
        public Product Product { get; init; }
        public int ProductQuantity { get; init; }




        public override bool Equals(object? obj)
        {
            return Equals(obj as Supplier);
        }

        public bool Equals(Supplier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   FirstName == other.FirstName &&
                   LastName == other.LastName &&
                   FullName == other.FullName &&
                   PhoneNumber == other.PhoneNumber &&
                   SupplierAddress == other.SupplierAddress &&
                   EqualityComparer<Product>.Default.Equals(Product, other.Product) &&
                   ProductQuantity == other.ProductQuantity;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), FirstName, LastName, FullName, PhoneNumber, SupplierAddress, Product, ProductQuantity);
        }

        public static bool operator ==(Supplier? left, Supplier? right)
        {
            return EqualityComparer<Supplier>.Default.Equals(left, right);
        }

        public static bool operator !=(Supplier? left, Supplier? right)
        {
            return !(left == right);
        }
    }
}
