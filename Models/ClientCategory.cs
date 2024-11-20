using SharedORMAppsModels.DB.Base;

namespace PiecesAutoYoussefApp.Models
{
    public class ClientCategory : DBObjIntKeyed, IEquatable<ClientCategory?>
    {
        public ClientCategory(string categoryName)
        {
            CategoryName = categoryName;
        }

        public string CategoryName { get; init; }


        public override bool Equals(object? obj)
        {
            return Equals(obj as ClientCategory);
        }

        public bool Equals(ClientCategory? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   CategoryName == other.CategoryName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), CategoryName);
        }

        public static bool operator ==(ClientCategory? left, ClientCategory? right)
        {
            return EqualityComparer<ClientCategory>.Default.Equals(left, right);
        }

        public static bool operator !=(ClientCategory? left, ClientCategory? right)
        {
            return !(left == right);
        }
    }
}
