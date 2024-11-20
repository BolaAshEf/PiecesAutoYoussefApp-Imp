using SharedORMAppsModels.DB.Base;

namespace PiecesAutoYoussefApp.Models
{
    public class Client : DBObjIntKeyed, IEquatable<Client?>
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string FullName => FirstName + " " + LastName;
        public string PhoneNumber { get; init; }
        public string ClientAddress { get; init; }
        public ClientCategory ClientCategory { get; init; }

        public Client(string firstName, string lastName, string phoneNumber, string clientAddress, ClientCategory clientCategory)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            ClientAddress = clientAddress;
            ClientCategory = clientCategory;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Client);
        }

        public bool Equals(Client? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   FirstName == other.FirstName &&
                   LastName == other.LastName &&
                   PhoneNumber == other.PhoneNumber &&
                   ClientAddress == other.ClientAddress &&
                   EqualityComparer<ClientCategory>.Default.Equals(ClientCategory, other.ClientCategory);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), FirstName, LastName, PhoneNumber, ClientAddress, ClientCategory);
        }

        public static bool operator ==(Client? left, Client? right)
        {
            return EqualityComparer<Client>.Default.Equals(left, right);
        }

        public static bool operator !=(Client? left, Client? right)
        {
            return !(left == right);
        }
    }
}
