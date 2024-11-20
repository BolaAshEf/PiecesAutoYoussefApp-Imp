using Microsoft.Data.SqlClient;
using PiecesAutoYoussefApp.Models;
using System.Data;

namespace PiecesAutoYoussefApp.Parsers
{
    public static class SqlParamsParser
    {
        public static SqlParameter[] FromClient(Client client) => [
            new SqlParameter("@m_firstName", SqlDbType.NVarChar, 30)
            {
                Value = client.FirstName
            },
            new SqlParameter("@m_lastName", SqlDbType.NVarChar, 30)
            {
                Value = client.LastName
            },
            new SqlParameter("@m_phoneNumber", SqlDbType.VarChar, 15)
            {
                Value = client.PhoneNumber
            },
            new SqlParameter("@m_clientAddress", SqlDbType.NVarChar, 50)
            {
                Value = client.ClientAddress
            },
            new SqlParameter("@m_categoryName", SqlDbType.NVarChar, 60)
            {
                Value = client.ClientCategory.CategoryName
            }
        ];

        public static SqlParameter[] FromClientCategory(ClientCategory clientCategory) => [
             new SqlParameter("@m_categoryName", SqlDbType.NVarChar, 60)
             {
                 Value = clientCategory.CategoryName,
             }
        ];

        public static SqlParameter[] FromModel(Model model) => [
            new SqlParameter("@m_modelName", SqlDbType.NVarChar, 60)
            {
                Value = model.ModelName
            }
        ];

        public static SqlParameter[] FromOrder(Order order) => [
            new SqlParameter("@m_productID", SqlDbType.Int)
            {
                Value = order.Product.Id
            },
            new SqlParameter("@m_quantity", SqlDbType.Int)
            {
                Value = order.Quantity
            },
            new SqlParameter("@m_unitPrice", SqlDbType.Money)
            {
                Value = order.UnitPrice
            },
            new SqlParameter("@m_vatRate", SqlDbType.Decimal)
            {
                Value = order.VatRate
            },
            new SqlParameter("@m_orderDate", SqlDbType.DateTime)
            {
                Value = order.OrderDate
            },
            new SqlParameter("@m_updateProductsThen", SqlDbType.Bit)
            {
                Value = order.UpdateProductsThen
            }
        ];

        public static SqlParameter[] FromPiece(Piece piece) => [
            new SqlParameter("@m_pieceName", SqlDbType.NVarChar, 60)
            {
                Value = piece.PieceName
            }
        ];

        public static SqlParameter[] FromProduct(Product product) => [
            new SqlParameter("@m_referenceID", SqlDbType.VarChar, 100)
            {
                Value = product.ReferenceID
            },
            new SqlParameter("@m_modelName", SqlDbType.NVarChar, 60)
            {
                Value = product.Model.ModelName
            },
            new SqlParameter("@m_pieceName", SqlDbType.NVarChar, 60)
            {
                Value = product.Piece.PieceName
            },
            new SqlParameter("@m_stock", SqlDbType.Int)
            {
                Value = product.Stock
            },
            new SqlParameter("@m_unitPrice", SqlDbType.Money)
            {
                Value = product.UnitPrice
            },
            new SqlParameter("@m_additionDate", SqlDbType.DateTime)
            {
                Value = product.AdditionDate
            }
        ];

        public static SqlParameter[] FromSupplier(Supplier supplier) => [
            new SqlParameter("@m_firstName", SqlDbType.NVarChar, 30)
            {
                Value = supplier.FirstName
            },
            new SqlParameter("@m_lastName", SqlDbType.NVarChar, 30)
            {
                Value = supplier.LastName
            },
            new SqlParameter("@m_phoneNumber", SqlDbType.VarChar, 15)
            {
                Value = supplier.PhoneNumber
            },
            new SqlParameter("@m_supplierAddress", SqlDbType.NVarChar, 50)
            {
                Value = supplier.SupplierAddress
            },
            new SqlParameter("@m_productID", SqlDbType.Int)
            {
                Value = supplier.Product.Id
            },
            new SqlParameter("@m_productQuantity", SqlDbType.Int)
            {
                Value = supplier.ProductQuantity
            }
        ];

        public static SqlParameter[] FromOrderCollection(OrderCollection orderCollection) => [
            new SqlParameter("@m_clientID", SqlDbType.Int)
            {
                Value = orderCollection.Client.Id,
            }
        ];
    }
}
