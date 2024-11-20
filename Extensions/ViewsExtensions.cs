using PiecesAutoYoussefApp.UI.Base;

namespace PiecesAutoYoussefApp.Extensions
{
    public static class ViewsExtensions
    {
        public static void SetAllVisibility(this IPartialView view, Func<bool, bool> visibilityCallback)
        {
            view.BtnAdd.Visible = visibilityCallback(view.BtnAdd.Visible);
            view.BtnDelete.Visible = visibilityCallback(view.BtnDelete.Visible);
            view.BtnSave.Visible = visibilityCallback(view.BtnSave.Visible);
            view.BtnUpdate.Visible = visibilityCallback(view.BtnUpdate.Visible);
            view.BtnView.Visible = visibilityCallback(view.BtnView.Visible);
        }

        public static void SetAllVisibility(this IFullView view, Func<bool, bool> visibilityCallback)
        {
            SetAllVisibility((IPartialView)view, visibilityCallback);

            view.BtnClear.Visible = visibilityCallback(view.BtnClear.Visible);
            view.BtnExportCSV.Visible = visibilityCallback(view.BtnExportCSV.Visible);
            view.BtnSearch.Visible = visibilityCallback(view.BtnSearch.Visible);
            view.BtnView.Visible = visibilityCallback(view.BtnView.Visible);
            view.CountView.Visible = visibilityCallback(view.CountView.Visible);
            view.GridView.Visible = visibilityCallback(view.GridView.Visible);
        }
    }
}
