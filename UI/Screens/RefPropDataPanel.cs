using PiecesAutoYoussefApp.UI.Base;
using PiecesAutoYoussefApp.UI.Impl;
using SharedORMAppsUI.Base;
using SharedORMAppsUI.Controls;
using System.ComponentModel;

namespace PiecesAutoYoussefApp.UI.Screens
{
    public partial class RefPropDataPanel : AutoHeightUserControl, IPartialView
    {
        Control ISavable.BtnSave => BtnSaveCollection;
        Control IDeletableView.BtnDelete => BtnDeleteCollection;
        Control IViewableView.BtnView => BtnCancelCollection;
        Control IAddableView.BtnAdd => BtnAddCollection;
        Control IUpdatableView.BtnUpdate => BtnUpdateCollection;

        private IPartialView View => this;


        public RefPropDataPanel()
        {
            InitializeComponent();

            Load += (object? sender, EventArgs e) =>
            {
                View.BtnAdd.Click += (object? sender, EventArgs e) => ListID.Focus();
                AddEnterFocusNextEvent(ListID);
                View.BtnSave.Click += (object? sender, EventArgs e) => View.BtnAdd.Focus();
            };
        }

        private void AddEnterFocusNextEvent(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                control.KeyDown += (object? sender, KeyEventArgs e) =>
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        View.BtnSave.Focus();
                    }
                };
                if (control.Controls.Count > 0)
                {
                    AddEnterFocusNextEvent(control);
                }
            }
        }

        [Category("Custom")]
        [Browsable(true)]
        public string Label
        {
            get => ListID.Label;
            set => ListID.Label = value;
        }
    }


    public class ClientCategoryDataPanel : RefPropDataPanel, IRefPropView
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
    }
    public class ModelDataPanel : RefPropDataPanel, IRefPropView
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
    }
    public class PieceDataPanel : RefPropDataPanel, IRefPropView
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
    }
}
