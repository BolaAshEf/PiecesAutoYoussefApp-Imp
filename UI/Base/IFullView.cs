using SharedORMAppsUI.Base;

namespace PiecesAutoYoussefApp.UI.Base
{
    public interface IFullView : IPartialView,
        ISearchableView, IWithCountView, IWithExport, IWithGridView
    {

    }
}
