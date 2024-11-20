using SharedORMAppsModels.DB.Base;

namespace PiecesAutoYoussefApp.Models
{
    public class Piece : DBObjIntKeyed, IEquatable<Piece?>
    {
        public Piece(string pieceName)
        {
            PieceName = pieceName;
        }

        public string PieceName { get; init; }



        public override bool Equals(object? obj)
        {
            return Equals(obj as Piece);
        }

        public bool Equals(Piece? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   PieceName == other.PieceName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), PieceName);
        }

        public static bool operator ==(Piece? left, Piece? right)
        {
            return EqualityComparer<Piece>.Default.Equals(left, right);
        }

        public static bool operator !=(Piece? left, Piece? right)
        {
            return !(left == right);
        }
    }
}
