using PiecesAutoYoussefApp.Models; // TODO
using PiecesAutoYoussefApp.NotificationCenter;
using PiecesAutoYoussefApp.Parsers;
using PiecesAutoYoussefApp.Utils;
using SharedAppsUtils.ThirdParty;
using System.Data;
using System.Text;

namespace PiecesAutoYoussefApp.UI.Screens
{
    public partial class OrdersBillForm : Form
    {
        public OrdersBillForm(int collectionID, Client client, DateTime date, List<Order> orders)
        {
            InitializeComponent();

            TxtClientName.Text = client.FullName;
            TxtClientAddress.Text = client.ClientAddress;
            TxtClientCategory.Text = client.ClientCategory.CategoryName;
            TxtClientPhone.Text = client.PhoneNumber;

            TxtAF1.Text = TxtAF4.Text = collectionID.ToString();
            TxtAF2.Text = date.ToString();

            GridView.DataSource = GridViewTableParser.FromOrdersForPrinting(orders);
            SharedAppsUtils.General.Utils.FormatDecimalColumns(GridView);

            decimal tht = 0, tva = 0, ttc = 0;
            foreach (Order order in orders)
            {
                tht += order.NoTaxTotalPrice;
                tva += order.VatRate;
                ttc += order.TotalPrice;
            }
            TxtHT.Text = SharedAppsUtils.General.Utils.FormatDecimalText(tht.ToString());
            TxtTVA.Text = SharedAppsUtils.General.Utils.FormatDecimalText(tva.ToString());
            TxtTTC.Text = SharedAppsUtils.General.Utils.FormatDecimalText(ttc.ToString());
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(Constants.PRINT_PDF_CMD_CACHE_PATH)) { Directory.Delete(Constants.PRINT_PDF_CMD_CACHE_PATH, true); }
                if (File.Exists(Constants.PRINT_TEMP_HTML_PATH)) { File.Delete(Constants.PRINT_TEMP_HTML_PATH); }
                if (File.Exists(Constants.PRINT_OUT_PDF_FILE_PATH)) { File.Delete(Constants.PRINT_OUT_PDF_FILE_PATH); }

                string billHTML = GetResultBillAsHTML(Constants.PRINT_HTML_TEMPLATE_PATH, GetBillTemplatesMap());

                File.WriteAllText(Constants.PRINT_TEMP_HTML_PATH, billHTML);

                PrintingUtils.HTML2PDF(
                    Constants.PRINT_HTML2PDF_CMD_PATH,
                    Constants.PRINT_TEMP_HTML_PATH,
                    Constants.PRINT_OUT_PDF_FILE_PATH
                );

                PrintingUtils.PrintPDF(
                    Constants.PRINT_PDF_CMD_PATH,
                    Constants.PRINT_OUT_PDF_FILE_PATH
                );

                if (Directory.Exists(Constants.PRINT_PDF_CMD_CACHE_PATH)) { Directory.Delete(Constants.PRINT_PDF_CMD_CACHE_PATH, true); }
                if (File.Exists(Constants.PRINT_TEMP_HTML_PATH)) { File.Delete(Constants.PRINT_TEMP_HTML_PATH); }
                if (File.Exists(Constants.PRINT_OUT_PDF_FILE_PATH)) { File.Delete(Constants.PRINT_OUT_PDF_FILE_PATH); }
            }
            catch (Exception ex)
            {
                ErrorHandler.NotifyErrorUnknown();
            }

            Close();
        }

        private string GetResultBillAsHTML(string htmlTemplatePath, Dictionary<string, string> billTemplatesMap)
        {
            StringBuilder sb = new StringBuilder(File.ReadAllText(htmlTemplatePath));
            foreach (var template in billTemplatesMap)
            {
                sb.Replace(template.Key, template.Value);
            }

            return sb.ToString();
        }

        private Dictionary<string, string> GetBillTemplatesMap()
        {
            GetBillDataTableAsHTML(out string headers, out string rows);
            return new Dictionary<string, string>
            {
                { "{{client-name}}", TxtClientName.Text },
                { "{{client-address}}", TxtClientAddress.Text },
                { "{{client-phone}}",  TxtClientPhone.Text },
                { "{{client-category}}", TxtClientCategory.Text },
                { "{{bill-id1}}", TxtAF1.Text },
                { "{{bill-date}}", TxtAF2.Text },
                { "{{bill-id2}}", TxtAF4.Text },
                { "{{bill-representative}}", TxtAF7.Text },
                { "{{bill-payment-method}}", ChkChangePatmentControl.Checked ? TxtPayment.Text : (ListPayment.SelectedItem?.ToString() ?? "") },
                { "{{data-headers}}", headers },
                { "{{data-rows}}", rows },
                { "{{bill-controller}}", TxtController.Text },
                { "{{bill-tht}}", TxtHT.Text },
                { "{{bill-tva}}", TxtTVA.Text },
                { "{{bill-ttc}}", TxtTTC.Text }
            };
        }

        private void GetBillDataTableAsHTML(out string headers, out string rows)
        {
            DataTable tbl = (DataTable)GridView.DataSource;
            StringBuilder headerBuilder = new StringBuilder();
            foreach (DataColumn column in tbl.Columns)
            {
                headerBuilder.Append($"<th>{column.ColumnName}</th>");
            }
            headers = headerBuilder.ToString();

            StringBuilder rowsBuilder = new StringBuilder();
            foreach (DataRow row in tbl.Rows)
            {
                rowsBuilder.Append("<tr>");
                foreach (var cell in row.ItemArray)
                {
                    rowsBuilder.Append($"<td>{cell}</td>");
                }
                rowsBuilder.Append("</tr>");
            }
            rows = rowsBuilder.ToString();
        }

        private void ChkAddProp_CheckedChanged(object sender, EventArgs e)
        {
            TxtPayment.Visible = ChkChangePatmentControl.Checked;
            ListPayment.Visible = !ChkChangePatmentControl.Checked;
        }
    }
}
