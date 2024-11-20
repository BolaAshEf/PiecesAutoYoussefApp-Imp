using SharedORMAppsModels.DB.Base;

namespace PiecesAutoYoussefApp.Models
{
    public class Product : DBObjIntKeyed, IEquatable<Product?>
    {
        public Product(string referenceID, Piece piece, Model model, int stock, decimal unitPrice, DateTime additionDate)
        {
            ReferenceID = referenceID;
            Piece = piece;
            Model = model;
            Stock = stock;
            UnitPrice = unitPrice;
            AdditionDate = additionDate;
        }

        public string ReferenceID { get; init; }
        public Piece Piece { get; init; }
        public Model Model { get; init; }
        public int Stock { get; init; }
        public decimal UnitPrice { get; init; }
        public DateTime AdditionDate { get; init; }



        public override bool Equals(object? obj)
        {
            return Equals(obj as Product);
        }

        public bool Equals(Product? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ReferenceID == other.ReferenceID &&
                   EqualityComparer<Piece>.Default.Equals(Piece, other.Piece) &&
                   EqualityComparer<Model>.Default.Equals(Model, other.Model) &&
                   Stock == other.Stock &&
                   UnitPrice == other.UnitPrice &&
                   AdditionDate == other.AdditionDate;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), ReferenceID, Piece, Model, Stock, UnitPrice, AdditionDate);
        }

        public static bool operator ==(Product? left, Product? right)
        {
            return EqualityComparer<Product>.Default.Equals(left, right);
        }

        public static bool operator !=(Product? left, Product? right)
        {
            return !(left == right);
        }
    }
}
