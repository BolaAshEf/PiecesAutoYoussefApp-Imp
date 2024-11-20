using PiecesAutoYoussefApp.UI.Base;
using SharedORMAppsUI.Controls;

namespace PiecesAutoYoussefApp.UI.Impl
{
    public interface IProductsView : IAppModelView
    {
        LabeledProp NumStock { get; }
        LabeledProp NumUnitPrice { get; }
        SysDateProp DateAddition { get; }
        EditableRefPropCombo ListModel { get; }
        EditableRefPropCombo ListPiece { get; }


        object? ID
        {
            get => ListID.CurrentItem;
            set => ListID.CurrentItem = value;
        } // string or Model
        int? Stock
        {
            get => int.TryParse(NumStock.Text, out int stock) ? stock : null;
            set => NumStock.Text = value?.ToString() ?? "";
        }
        decimal? UnitPrice
        {
            get => decimal.TryParse(NumUnitPrice.Text, out decimal unitPrice) ? unitPrice : null;
            set => NumUnitPrice.Text = value?.ToString() ?? "";
        }
        DateTime AdditionDate
        {
            get => DateAddition.Value;
            set => DateAddition.Value = value;
        }

        object[] ProductModels
        {
            get => ((ComboBox)ListModel).Items.Cast<object>().ToArray();
            set
            {
                var items = ((ComboBox)ListModel).Items;
                if (items.Cast<object>().SequenceEqual(value))
                {
                    return;
                }

                items.Clear();
                items.AddRange(value);
            }
        }
        object? ProductModel { get => ListModel.CurrentItem; set => ListModel.CurrentItem = value; } // string or Model

        object[] ProductPieces
        {
            get => ((ComboBox)ListPiece).Items.Cast<object>().ToArray();
            set
            {
                var items = ((ComboBox)ListPiece).Items;
                if (items.Cast<object>().SequenceEqual(value))
                {
                    return;
                }

                items.Clear();
                items.AddRange(value);
            }
        }
        object? ProductPiece { get => ListPiece.CurrentItem; set => ListPiece.CurrentItem = value; } // string or Model
    }
}
