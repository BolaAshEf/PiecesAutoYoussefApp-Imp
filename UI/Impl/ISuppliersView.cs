using PiecesAutoYoussefApp.UI.Base;
using SharedORMAppsUI.Controls;

namespace PiecesAutoYoussefApp.UI.Impl
{
    public interface ISuppliersView : IAppModelView
    {
        LabeledProp TxtFirstName { get; }
        LabeledProp TxtLastName { get; }
        LabeledProp TxtPhoneNumber { get; }
        LabeledProp TxtAddress { get; }
        LabeledProp NumProductQuantity { get; }
        EditableRefPropCombo ListProduct { get; }


        string FirstName { get => TxtFirstName.Text; set => TxtFirstName.Text = value; }
        string LastName { get => TxtLastName.Text; set => TxtLastName.Text = value; }
        string PhoneNumber { get => TxtPhoneNumber.Text; set => TxtPhoneNumber.Text = value; }
        string Address { get => TxtAddress.Text; set => TxtAddress.Text = value; }
        int? ProductQuantity
        {
            get => int.TryParse(NumProductQuantity.Text, out int qte) ? qte : null;
            set => NumProductQuantity.Text = value?.ToString() ?? "";
        }
        object[] SupplierProducts
        {
            get => ((ComboBox)ListProduct).Items.Cast<object>().ToArray();
            set
            {
                var items = ((ComboBox)ListProduct).Items;
                if (items.Cast<object>().SequenceEqual(value))
                {
                    return;
                }

                items.Clear();
                items.AddRange(value);
            }
        }
        object? SupplierProduct { get => ListProduct.CurrentItem; set => ListProduct.CurrentItem = value; } // string or Model
    }
}
