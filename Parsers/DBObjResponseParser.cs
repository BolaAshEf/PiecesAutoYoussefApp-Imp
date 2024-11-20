using PiecesAutoYoussefApp.Models;
using SharedORMAppsModels.Extensions;
using System.Data;

namespace PiecesAutoYoussefApp.Parsers
{
    /// <summary>
    /// Order matters with tblsEnumerator.
    /// </summary>
    public static class DBObjResponseParser
    {
        public static Client[] ToClients(IEnumerator<DataTable> tblsEnumerator)
        {
            DataTable table = tblsEnumerator.GetNextTable();
            ClientCategory[] clientCategories = ToClientCategories(tblsEnumerator);

            var clients = new Client[table.Rows.Count];
            for (int i = 0; i < clients.Length; i++)
            {
                DataRow row = table.Rows[i];
                clients[i] = new Client(
                    firstName: row.GetField<string>("firstName"),
                    lastName: row.GetField<string>("lastName"),
                    phoneNumber: row.GetField<string>("phoneNumber"),
                    clientAddress: row.GetField<string>("clientAddress"),
                    clientCategory: clientCategories[i]
                )
                {
                    Id = row.GetField<int>("id")
                };
            }
            return clients;
        }

        public static ClientCategory[] ToClientCategories(IEnumerator<DataTable> tblsEnumerator)
        {
            DataTable table = tblsEnumerator.GetNextTable();
            var clientCategories = new ClientCategory[table.Rows.Count];
            for (int i = 0; i < clientCategories.Length; i++)
            {
                DataRow row = table.Rows[i];
                clientCategories[i] = new ClientCategory(
                    categoryName: row.GetField<string>("categoryName")
                )
                {
                    Id = row.GetField<int>("id")
                };
            }
            return clientCategories;
        }

        public static Model[] ToModels(IEnumerator<DataTable> tblsEnumerator)
        {
            DataTable table = tblsEnumerator.GetNextTable();
            var models = new Model[table.Rows.Count];
            for (int i = 0; i < models.Length; i++)
            {
                DataRow row = table.Rows[i];
                models[i] = new Model(
                    modelName: row.GetField<string>("modelName")
                )
                {
                    Id = row.GetField<int>("id")
                };
            }
            return models;
        }

        public static OrderCollection[] ToOrderCollections(IEnumerator<DataTable> tblsEnumerator)
        {
            DataTable table = tblsEnumerator.GetNextTable();
            Client[] clients = ToClients(tblsEnumerator);
            var orderCollections = new OrderCollection[table.Rows.Count];
            for (int i = 0; i < orderCollections.Length; i++)
            {
                DataRow row = table.Rows[i];
                orderCollections[i] = new OrderCollection(
                    client: clients[i]
                )
                {
                    Id = row.GetField<int>("id")
                };
            }
            return orderCollections;
        }

        public static Order[] ToOrders(IEnumerator<DataTable> tblsEnumerator)
        {
            DataTable table = tblsEnumerator.GetNextTable();
            Product[] products = ToProducts(tblsEnumerator);
            var orders = new Order[table.Rows.Count];
            for (int i = 0; i < orders.Length; i++)
            {
                DataRow row = table.Rows[i];
                orders[i] = new Order(
                    product: products[i],
                    quantity: row.GetField<int>("quantity"),
                    unitPrice: row.GetField<decimal>("unitPrice"),
                    vatRate: row.GetField<decimal>("vatRate"),
                    orderDate: row.GetField<DateTime>("orderDate"),
                    updateProductsThen: row.GetField<bool>("updateProductsThen")
                )
                {
                    Id = row.GetField<int>("id")
                };
            }
            return orders;
        }

        public static Piece[] ToPieces(IEnumerator<DataTable> tblsEnumerator)
        {
            DataTable table = tblsEnumerator.GetNextTable();
            var pieces = new Piece[table.Rows.Count];
            for (int i = 0; i < pieces.Length; i++)
            {
                DataRow row = table.Rows[i];
                pieces[i] = new Piece(
                    pieceName: row.GetField<string>("pieceName")
                )
                {
                    Id = row.GetField<int>("id")
                };
            }
            return pieces;
        }

        public static Product[] ToProducts(IEnumerator<DataTable> tblsEnumerator)
        {
            DataTable table = tblsEnumerator.GetNextTable();
            Model[] models = ToModels(tblsEnumerator);
            Piece[] pieces = ToPieces(tblsEnumerator);
            var products = new Product[table.Rows.Count];
            for (int i = 0; i < products.Length; i++)
            {
                DataRow row = table.Rows[i];
                products[i] = new Product(
                    referenceID: row.GetField<string>("referenceID"),
                    model: models[i],
                    piece: pieces[i],
                    stock: row.GetField<int>("stock"),
                    unitPrice: row.GetField<decimal>("unitPrice"),
                    additionDate: row.GetField<DateTime>("additionDate")
                )
                {
                    Id = row.GetField<int>("id")
                };
            }
            return products;
        }

        public static Supplier[] ToSuppliers(IEnumerator<DataTable> tblsEnumerator)
        {
            DataTable table = tblsEnumerator.GetNextTable();
            Product[] products = ToProducts(tblsEnumerator);
            var suppliers = new Supplier[table.Rows.Count];
            for (int i = 0; i < suppliers.Length; i++)
            {
                DataRow row = table.Rows[i];
                suppliers[i] = new Supplier(
                    firstName: row.GetField<string>("firstName"),
                    lastName: row.GetField<string>("lastName"),
                    phoneNumber: row.GetField<string>("phoneNumber"),
                    supplierAddress: row.GetField<string>("supplierAddress"),
                    product: products[i],
                    productQuantity: row.GetField<int>("productQuantity")
                )
                {
                    Id = row.GetField<int>("id")
                };
            }
            return suppliers;
        }
    }
}
