using PiecesAutoYoussefApp.UI.Base;
using PiecesAutoYoussefApp.UI.Impl;
using SharedORMAppsUI.Base;
using SharedORMAppsUI.Controls;

namespace PiecesAutoYoussefApp.UI.Screens
{
    public partial class OrderDataPanel : AutoHeightUserControl, IOrdersView
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

        LabeledProp IOrdersView.NumNoTaxTotalPrice => NumNoTaxTotalPrice;
        LabeledProp IOrdersView.NumTotalPrice => NumTotalPrice;
        LabeledProp IOrdersView.NumQuantity => NumQuantity;
        LabeledProp IOrdersView.NumUnitPrice => NumUnitPrice;
        LabeledProp IOrdersView.NumVatRate => NumVatRate;
        SysDateProp IOrdersView.DateOrder => DateOrder;
        EditableRefPropCombo IOrdersView.ListProduct => ListProduct;
        CheckBox IOrdersView.ChkUpdateStock => ChkUpdateStock;

        Button IOrdersView.BtnPrint => BtnPrint;

        public OrderDataPanel()
        {
            InitializeComponent();
        }
    }
}
