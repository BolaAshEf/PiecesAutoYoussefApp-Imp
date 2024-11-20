using PiecesAutoYoussefApp.UI.Base;
using PiecesAutoYoussefApp.UI.Impl;
using SharedORMAppsUI.Base;
using SharedORMAppsUI.Controls;

namespace PiecesAutoYoussefApp.UI.Screens
{
    public partial class SupplierDataPanel : AutoHeightUserControl, ISuppliersView
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


        LabeledProp ISuppliersView.TxtFirstName => TxtFirstName;
        LabeledProp ISuppliersView.TxtLastName => TxtLastName;
        LabeledProp ISuppliersView.TxtPhoneNumber => TxtPhoneNumber;
        LabeledProp ISuppliersView.TxtAddress => TxtAddress;
        LabeledProp ISuppliersView.NumProductQuantity => NumQuantity;
        EditableRefPropCombo ISuppliersView.ListProduct => ListProduct;

        public SupplierDataPanel()
        {
            InitializeComponent();
        }
    }
}
