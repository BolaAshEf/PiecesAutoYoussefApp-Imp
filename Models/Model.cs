using SharedORMAppsModels.DB.Base;

namespace PiecesAutoYoussefApp.Models
{
    public class Model : DBObjIntKeyed, IEquatable<Model?>
    {
        public Model(string modelName)
        {
            ModelName = modelName;
        }

        public string ModelName { get; init; }



        public override bool Equals(object? obj)
        {
            return Equals(obj as Model);
        }

        public bool Equals(Model? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ModelName == other.ModelName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), ModelName);
        }

        public static bool operator ==(Model? left, Model? right)
        {
            return EqualityComparer<Model>.Default.Equals(left, right);
        }

        public static bool operator !=(Model? left, Model? right)
        {
            return !(left == right);
        }
    }
}
