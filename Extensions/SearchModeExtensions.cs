using SharedORMAppsModels.DB.Impl.Sql.Search;
using SharedORMAppsUI.Base;

namespace PiecesAutoYoussefApp.Extensions
{
    public static class SearchModeExtensions
    {
        public static DBSearchMode ToDBSearchMode(this ButtonSearchMode buttonSearchMode)
        {
            return (DBSearchMode)buttonSearchMode;
        }

        public static ButtonSearchMode ToButtonSearchMode(this DBSearchMode dbSearchMode)
        {
            return (ButtonSearchMode)dbSearchMode;
        }
    }
}
