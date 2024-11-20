using PiecesAutoYoussefApp.UI.Base;
using PiecesAutoYoussefApp.UI.Impl;
using SharedORMAppsUI.Base;
using SharedORMAppsUI.Controls;

namespace PiecesAutoYoussefApp.UI.Screens
{
    public partial class ClientDataPanel : AutoHeightUserControl, IClientsView
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

        LabeledProp IClientsView.TxtFirstName => TxtFirstName;
        LabeledProp IClientsView.TxtLastName => TxtLastName;
        LabeledProp IClientsView.TxtPhoneNumber => TxtPhoneNumber;
        LabeledProp IClientsView.TxtAddress => TxtAddress;
        EditableRefPropCombo IClientsView.EditableCategory => EditableCategory;

        public ClientDataPanel()
        {
            InitializeComponent();
        }
    }
}
