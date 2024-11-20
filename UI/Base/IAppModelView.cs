using SharedORMAppsUI.Base;
using SharedORMAppsUI.Controls;


namespace PiecesAutoYoussefApp.UI.Base
{
    public interface IAppModelView : IModelView
    {
        IDCombo ListID { get; }
    }
}
