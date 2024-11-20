using PiecesAutoYoussefApp.UI.Base;
using PiecesAutoYoussefApp.UI.Impl;
using SharedORMAppsUI.Base;
using SharedORMAppsUI.Controls;

namespace PiecesAutoYoussefApp.UI.Screens
{
    public partial class ClientOrderCollectionDataPanel : AutoHeightUserControl, IClientOrderCollectionsView, IPartialView
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

        EditableRefPropCombo IClientOrderCollectionsView.ListCollectionClient => ListCollectionClient;
        public OrderDataPanel OrderDataPanel => PanelOrderData;



        Control ISavable.BtnSave => BtnSaveCollection;
        Control IDeletableView.BtnDelete => BtnDeleteCollection;
        Control IViewableView.BtnView => BtnCancelCollection;
        Control IAddableView.BtnAdd => BtnAddCollection;
        Control IUpdatableView.BtnUpdate => BtnUpdateCollection;



        public ClientOrderCollectionDataPanel()
        {
            InitializeComponent();
        }
    }
}
