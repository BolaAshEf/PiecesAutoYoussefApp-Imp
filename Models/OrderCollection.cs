using SharedORMAppsModels.DB.Base;

namespace PiecesAutoYoussefApp.Models
{
    public class OrderCollection : DBObjIntKeyed, IEquatable<OrderCollection?>
    {
        public OrderCollection(Client client)
        {
            Client = client;
        }

        public Client Client { get; init; }



        public override bool Equals(object? obj)
        {
            return Equals(obj as OrderCollection);
        }

        public bool Equals(OrderCollection? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   EqualityComparer<Client>.Default.Equals(Client, other.Client);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Client);
        }

        public static bool operator ==(OrderCollection? left, OrderCollection? right)
        {
            return EqualityComparer<OrderCollection>.Default.Equals(left, right);
        }

        public static bool operator !=(OrderCollection? left, OrderCollection? right)
        {
            return !(left == right);
        }
    }
}
