namespace PiecesAutoYoussefApp.UI.Base
{
    public static class AppModelViewHelper
    {
        public static IEnumerable<object> GetModels(IAppModelView appModelView)
        {
            return ((ComboBox)appModelView.ListID).Items.Cast<object>().ToArray();
        }

        public static void SetModels(IAppModelView appModelView, IEnumerable<object> models)
        {
            var items = ((ComboBox)appModelView.ListID).Items;
            items.Clear();
            items.AddRange(models.ToArray());
        }

        public static object? GetModel(IAppModelView appModelView)
        {
            return appModelView.ListID.CurrentItem;
        }

        public static void SetModel(IAppModelView appModelView, object? model)
        {
            appModelView.ListID.CurrentItem = model;
        }

        public static void AddModelChanged(IAppModelView appModelView, EventHandler e) =>
            ((ComboBox)appModelView.ListID).SelectedIndexChanged += e;

        public static void RemoveModelChanged(IAppModelView appModelView, EventHandler e) =>
            ((ComboBox)appModelView.ListID).SelectedIndexChanged -= e;
    }
}
