using PiecesAutoYoussefApp.UI.Base;
using PiecesAutoYoussefApp.UI.Impl;
using SharedORMAppsUI.Base;
using SharedORMAppsUI.Controls;


namespace PiecesAutoYoussefApp.UI.Screens
{
    public partial class ProductDataPanel : AutoHeightUserControl, IProductsView
    {
        IDCombo IAppModelView.ListID => ListID;
        public IEnumerable<object> Models
        {
            get => AppModelViewHelper.GetModels(this);
            set => AppModelViewHelper.SetModels(this, value);
        }
        public object? Model
        {
            get => AppModelViewHelper.GetModel(this);
            set => AppModelViewHelper.SetModel(this, value);
        }
        public event EventHandler ModelChanged
        {
            add => AppModelViewHelper.AddModelChanged(this, value);
            remove => AppModelViewHelper.RemoveModelChanged(this, value);
        }


        LabeledProp IProductsView.NumStock => NumStock;
        LabeledProp IProductsView.NumUnitPrice => NumUnitPrice;
        SysDateProp IProductsView.DateAddition => DateAddition;
        EditableRefPropCombo IProductsView.ListModel => ListModel;
        EditableRefPropCombo IProductsView.ListPiece => ListPiece;


        public ProductDataPanel()
        {
            InitializeComponent();
        }
    }
}
