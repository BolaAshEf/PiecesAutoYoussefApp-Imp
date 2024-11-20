using PiecesAutoYoussefApp.UI.Base;
using SharedORMAppsUI.Controls;
using System.Data;

namespace PiecesAutoYoussefApp.UI.Impl
{
    public interface IOrdersView : IAppModelView
    {
        LabeledProp NumNoTaxTotalPrice { get; }
        LabeledProp NumTotalPrice { get; }
        LabeledProp NumQuantity { get; }
        LabeledProp NumUnitPrice { get; }
        LabeledProp NumVatRate { get; }
        SysDateProp DateOrder { get; }
        EditableRefPropCombo ListProduct { get; }
        CheckBox ChkUpdateStock { get; }

        Button BtnPrint { get; }

        bool? UpdateStock
        {
            get => ChkUpdateStock.CheckState switch
            {
                CheckState.Checked => true,
                CheckState.Unchecked => false,
                CheckState.Indeterminate => null,
                _ => throw new NotImplementedException(),
            };
            set
            {
                switch (value)
                {
                    case true:
                        ChkUpdateStock.CheckState = CheckState.Checked;
                        break;
                    case false:
                        ChkUpdateStock.CheckState = CheckState.Unchecked;
                        break;
                    case null:
                        ChkUpdateStock.CheckState = CheckState.Indeterminate;
                        break;
                }
            }
        }
        decimal? NoTaxPrice
        {
            get => decimal.TryParse(NumNoTaxTotalPrice.Text, out decimal noTaxPrice) ? noTaxPrice : null;
            set => NumNoTaxTotalPrice.Text = value?.ToString() ?? "";
        }
        decimal? TotalPrice
        {
            get => decimal.TryParse(NumTotalPrice.Text, out decimal totalPrice) ? totalPrice : null;
            set => NumTotalPrice.Text = value?.ToString() ?? "";
        }
        int? Quantity
        {
            get => int.TryParse(NumQuantity.Text, out int stock) ? stock : null;
            set => NumQuantity.Text = value?.ToString() ?? "";
        }
        decimal? UnitPrice
        {
            get => decimal.TryParse(NumUnitPrice.Text, out decimal unitPrice) ? unitPrice : null;
            set => NumUnitPrice.Text = value?.ToString() ?? "";
        }
        decimal? VatRate
        {
            get => decimal.TryParse(NumVatRate.Text, out decimal vatRate) ? vatRate : null;
            set => NumVatRate.Text = value?.ToString() ?? "";
        }
        DateTime OrderDate
        {
            get => DateOrder.Value;
            set => DateOrder.Value = value;
        }

        object[] OrderProducts
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
        object? OrderProduct { get => ListProduct.CurrentItem; set => ListProduct.CurrentItem = value; }
    }
}