using PiecesAutoYoussefApp.UI.Base;
using SharedORMAppsUI.Controls;

namespace PiecesAutoYoussefApp.UI.Impl
{
    public interface IClientsView : IAppModelView
    {
        LabeledProp TxtFirstName { get; }
        LabeledProp TxtLastName { get; }
        LabeledProp TxtPhoneNumber { get; }
        LabeledProp TxtAddress { get; }
        EditableRefPropCombo EditableCategory { get; }


        string FirstName { get => TxtFirstName.Text; set => TxtFirstName.Text = value; }
        string LastName { get => TxtLastName.Text; set => TxtLastName.Text = value; }
        string PhoneNumber { get => TxtPhoneNumber.Text; set => TxtPhoneNumber.Text = value; }
        string Address { get => TxtAddress.Text; set => TxtAddress.Text = value; }

        object[] Categories
        {
            get => ((ComboBox)EditableCategory).Items.Cast<object>().ToArray();
            set
            {
                var items = ((ComboBox)EditableCategory).Items;
                if (items.Cast<object>().SequenceEqual(value))
                {
                    return;
                }

                items.Clear();
                items.AddRange(value);
            }
        }
        object? Category { get => EditableCategory.CurrentItem; set => EditableCategory.CurrentItem = value; } // string or ClientCategory
    }
}
