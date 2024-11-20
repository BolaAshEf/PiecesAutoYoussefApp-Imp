using PiecesAutoYoussefApp.Extensions;
using PiecesAutoYoussefApp.Models;
using System.Data;

namespace PiecesAutoYoussefApp.Parsers
{
    public static class GridViewTableParser
    {
        public static DataTable FromClients(IEnumerable<Client> clients)
        {
            DataTable table = new DataTable();

            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Full Name", typeof(string));
            table.Columns.Add("Phone Number", typeof(string));
            table.Columns.Add("Address", typeof(string));
            table.Columns.Add("Category", typeof(string));

            foreach (var client in clients)
            {
                table.Rows.Add(
                    client.Id,
                    client.FullName,
                    client.PhoneNumber,
                    client.ClientAddress,
                    client.ClientCategory.CategoryName
                );
            }

            return table;
        }

        public static DataTable FromClientCategories(IEnumerable<ClientCategory> categories)
        {
            DataTable table = new DataTable();

            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Name", typeof(string));

            foreach (var category in categories)
            {
                table.Rows.Add(category.Id, category.CategoryName);
            }

            return table;
        }

        public static DataTable FromModels(IEnumerable<Model> models)
        {
            DataTable table = new DataTable();

            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Name", typeof(string));

            foreach (var model in models)
            {
                table.Rows.Add(model.Id, model.ModelName);
            }

            return table;
        }

        public static DataTable FromOrders(IEnumerable<Order> orders)
        {
            DataTable table = new DataTable();

            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Product", typeof(string));
            table.Columns.Add("Quantity", typeof(int));
            table.Columns.Add("Unit Price", typeof(decimal));
            table.Columns.Add("VAT Rate", typeof(decimal));
            table.Columns.Add("Purchases", typeof(decimal));
            table.Columns.Add("Total Price", typeof(decimal));
            table.Columns.Add("Order Date", typeof(DateTime));
            table.Columns.Add("Update Products", typeof(bool));

            foreach (var order in orders)
            {
                table.Rows.Add(
                    order.Id,
                    order.Product.ToNamedDecorator().ToString(),
                    order.Quantity,
                    order.UnitPrice,
                    order.VatRate,
                    order.NoTaxTotalPrice,
                    order.TotalPrice,
                    order.OrderDate,
                    order.UpdateProductsThen
                );
            }

            return table;
        }

        public static DataTable FromOrdersForPrinting(IEnumerable<Order> orders)
        {
            DataTable table = new DataTable();

            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("Product", typeof(string));
            table.Columns.Add("Quantity", typeof(string));
            table.Columns.Add("Unit Price", typeof(string));
            table.Columns.Add("VAT Rate", typeof(string));
            table.Columns.Add("Purchases", typeof(string));
            table.Columns.Add("Total Price", typeof(string));

            foreach (var order in orders)
            {
                table.Rows.Add(
                    order.Id.ToString(),
                    order.Product.ToNamedDecorator().ToString(),
                    order.Quantity.ToString(),
                    SharedAppsUtils.General.Utils.FormatDecimalText(order.UnitPrice),
                    SharedAppsUtils.General.Utils.FormatDecimalText(order.VatRate),
                    SharedAppsUtils.General.Utils.FormatDecimalText(order.NoTaxTotalPrice),
                    SharedAppsUtils.General.Utils.FormatDecimalText(order.TotalPrice)
                );
            }

            return table;
        }

        public static DataTable FromPieces(IEnumerable<Piece> pieces)
        {
            DataTable table = new DataTable();

            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Name", typeof(string));

            foreach (var piece in pieces)
            {
                table.Rows.Add(piece.Id, piece.PieceName);
            }

            return table;
        }

        public static DataTable FromProducts(IEnumerable<Product> products)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Reference ID", typeof(string));
            table.Columns.Add("Piece", typeof(string));
            table.Columns.Add("Model", typeof(string));
            table.Columns.Add("Stock", typeof(int));
            table.Columns.Add("Unit Price", typeof(decimal));
            table.Columns.Add("Addition Date", typeof(DateTime));

            foreach (var product in products)
            {
                table.Rows.Add(
                    product.ReferenceID,
                    product.Piece.PieceName,
                    product.Model.ModelName,
                    product.Stock,
                    product.UnitPrice,
                    product.AdditionDate
                );
            }

            return table;
        }

        public static DataTable FromSuppliers(IEnumerable<Supplier> suppliers)
        {
            DataTable table = new DataTable();

            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Full Name", typeof(string));
            table.Columns.Add("Phone Number", typeof(string));
            table.Columns.Add("Supplier Address", typeof(string));
            table.Columns.Add("Product", typeof(string));
            table.Columns.Add("Product Quantity", typeof(int));

            foreach (var supplier in suppliers)
            {
                table.Rows.Add(
                    supplier.Id,
                    supplier.FullName,
                    supplier.PhoneNumber,
                    supplier.SupplierAddress,
                    supplier.Product.ToNamedDecorator().ToString(),
                    supplier.ProductQuantity
                );
            }

            return table;
        }
    }
}
