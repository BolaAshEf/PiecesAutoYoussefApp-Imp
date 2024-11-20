using Microsoft.Data.SqlClient;
using PiecesAutoYoussefApp.Models;
using PiecesAutoYoussefApp.Parsers;
using SharedORMAppsModels.DB.Impl.Sql.Procs;
using SharedORMAppsModels.DB.Impl.Sql.Search;
using SharedORMAppsModels.Extensions;
using System.Data;

namespace PiecesAutoYoussefApp.DB.Api.Procs
{
    public class GetClientOrderCollectionsProc : BaseGetProc<OrderCollection>
    {
        public override string ProcName => "spGetClientOrderCollections";
        public override Func<IEnumerator<DataTable>, IEnumerable<OrderCollection>> ParseOutputsCallback => DBObjResponseParser.ToOrderCollections;
        public GetClientOrderCollectionsProc(string connectionString, IEnumerable<int>? ids = null) : base(connectionString, ids) { }
    }

    public class AddClientOrderCollectionProc : BaseAddProc<OrderCollection>
    {
        public override string ProcName => "spAddClientOrderCollection";
        public override Func<IEnumerator<DataTable>, IEnumerable<OrderCollection>> ParseOutputCallback => DBObjResponseParser.ToOrderCollections;
        public override Func<OrderCollection, SqlParameter[]> ObjSqlParamsParserCallback => SqlParamsParser.FromOrderCollection;
        public AddClientOrderCollectionProc(string connectionString, OrderCollection orderCollection) : base(connectionString, orderCollection) { }
    }

    public class UpdateClientOrderCollectionProc : BaseUpdateProc<OrderCollection>
    {
        public override string ProcName => "spUpdateClientOrderCollection";
        public override Func<IEnumerator<DataTable>, IEnumerable<OrderCollection>> ParseOutputCallback => DBObjResponseParser.ToOrderCollections;
        public override Func<OrderCollection, SqlParameter[]> ObjSqlParamsParserCallback => SqlParamsParser.FromOrderCollection;
        public UpdateClientOrderCollectionProc(string connectionString, OrderCollection orderCollection) : base(connectionString, orderCollection) { }
    }

    public class DeleteClientOrderCollectionsProc : BaseDeleteProc
    {
        public override string ProcName => "spDeleteClientOrderCollections";
        public DeleteClientOrderCollectionsProc(string connectionString, IEnumerable<int> ids) : base(connectionString, ids) { }
    }



    public class GetOrdersProc : BaseGetProc<Order>
    {
        public override string ProcName => "spGetOrders";
        public override Func<IEnumerator<DataTable>, IEnumerable<Order>> ParseOutputsCallback => DBObjResponseParser.ToOrders;
        public GetOrdersProc(string connectionString, OrderCollection collection, IEnumerable<int>? ids = null) : base(
            connectionString,
            ids,
            new SqlParameter("@m_collectionID", SqlDbType.Int)
            {
                Value = collection.Id
            }
        )
        { }
    }

    public class AddOrderProc : BaseAddProc<Order>
    {
        public override string ProcName => "spAddOrder";
        public override Func<IEnumerator<DataTable>, IEnumerable<Order>> ParseOutputCallback => DBObjResponseParser.ToOrders;
        public override Func<Order, SqlParameter[]> ObjSqlParamsParserCallback => SqlParamsParser.FromOrder;
        public AddOrderProc(string connectionString, OrderCollection collection, Order order) : base(
            connectionString,
            order,
            new SqlParameter("@m_collectionID", SqlDbType.Int)
            {
                Value = collection.Id
            }
        )
        { }
    }


    public class OrdersSearchModes : BaseModelsSearchModes
    {
        public bool IncludeProductProps { get; init; }
        public DBSearchMode QuantityMode { get; init; }
        public DBSearchMode UnitPriceMode { get; init; }
        public DBSearchMode VatRateMode { get; init; }
        public DBSearchMode OrderDateMode { get; init; }
        public DBSearchMode UpdateProductsThenMode { get; init; }

        public OrdersSearchModes(
            DBSearchMode? quantityMode,
            DBSearchMode? unitPriceMode,
            DBSearchMode? vatRateMode,
            DBSearchMode? orderDateMode,
            bool? updateProducts,
            bool includeProductProps
        ) : base(
            new DBSearchModeUnit("@m_quantityMode", quantityMode ?? DBSearchMode.NotCare),
            new DBSearchModeUnit("@m_unitPriceMode", unitPriceMode ?? DBSearchMode.NotCare),
            new DBSearchModeUnit("@m_vatRateMode", vatRateMode ?? DBSearchMode.NotCare),
            new DBSearchModeUnit("@m_orderDateMode", orderDateMode ?? DBSearchMode.NotCare),
            new DBSearchModeUnit("@m_updateProductsThenMode", updateProducts == null ? DBSearchMode.NotCare : DBSearchMode.Equal),
            new DBSearchModeUnit(null, includeProductProps ? DBSearchMode.Equal : DBSearchMode.NotCare)
        )
        {
            QuantityMode = quantityMode ?? DBSearchMode.NotCare;
            UnitPriceMode = unitPriceMode ?? DBSearchMode.NotCare;
            VatRateMode = vatRateMode ?? DBSearchMode.NotCare;
            OrderDateMode = orderDateMode ?? DBSearchMode.NotCare;
            UpdateProductsThenMode = updateProducts == null ? DBSearchMode.NotCare : DBSearchMode.Equal;
            IncludeProductProps = includeProductProps;
        }
    }

    public class SearchOrdersProc : BaseDBProc<IEnumerable<Order>>
    {
        public override string ProcName => "spSearchOrders";
        public override IEnumerable<Order> ParseOutput(IEnumerable<DataTable> tables) => DBObjResponseParser.ToOrders(tables.GetTablesEnumerator());

        public SearchOrdersProc(
            string connectionString,
            OrderCollection collection,
            string? productProps,
            int? quantity,
            decimal? unitPrice,
            decimal? vatRate,
            DateTime orderDate,
            bool? updateProductsThen,
            OrdersSearchModes modes
        ) : base(
            connectionString,
            new SqlParameter("@m_collectionID", SqlDbType.Int)
            {
                Value = collection.Id
            },
            new SqlParameter("@m_productRefIDModelPieceName", SqlDbType.NVarChar, 100)
            {
                Value = productProps
            }.WithMode(modes.IncludeProductProps),
            new SqlParameter("@m_quantity", SqlDbType.Int)
            {
                Value = quantity
            }.WithMode(modes.QuantityMode),
            new SqlParameter("@m_unitPrice", SqlDbType.Money)
            {
                Value = unitPrice
            }.WithMode(modes.UnitPriceMode),
            new SqlParameter("@m_vatRate", SqlDbType.Decimal)
            {
                Value = vatRate
            }.WithMode(modes.VatRateMode),
            new SqlParameter("@m_orderDate", SqlDbType.DateTime)
            {
                Value = orderDate
            }.WithMode(modes.OrderDateMode),
            new SqlParameter("@m_updateProductsThen", SqlDbType.Bit)
            {
                Value = updateProductsThen
            }.WithMode(modes.UpdateProductsThenMode)
        )
        {
            AddMoreParams(modes.SqlParams);
        }
    }


    public class UpdateOrderProc : BaseUpdateProc<Order>
    {
        public override string ProcName => "spUpdateOrder";
        public override Func<IEnumerator<DataTable>, IEnumerable<Order>> ParseOutputCallback => DBObjResponseParser.ToOrders;
        public override Func<Order, SqlParameter[]> ObjSqlParamsParserCallback => SqlParamsParser.FromOrder;
        public UpdateOrderProc(string connectionString, OrderCollection collection, Order order) : base(
            connectionString,
            order,
            new SqlParameter("@m_collectionID", SqlDbType.Int)
            {
                Value = collection.Id
            }
        )
        {
        }
    }

    public class DeleteOrdersProc : BaseDeleteProc
    {
        public override string ProcName => "spDeleteOrders";
        public DeleteOrdersProc(string connectionString, IEnumerable<int> ids) : base(connectionString, ids) { }
    }
}
