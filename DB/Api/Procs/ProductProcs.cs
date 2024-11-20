using Microsoft.Data.SqlClient;
using PiecesAutoYoussefApp.Models;
using PiecesAutoYoussefApp.Parsers;
using SharedORMAppsModels.DB.Impl.Sql.Procs;
using SharedORMAppsModels.DB.Impl.Sql.Search;
using SharedORMAppsModels.Extensions;
using System.Data;

namespace PiecesAutoYoussefApp.DB.Api.Procs
{
    public class GetProductsProc : BaseGetProc<Product>
    {
        public override string ProcName => "spGetProducts";
        public override Func<IEnumerator<DataTable>, IEnumerable<Product>> ParseOutputsCallback => DBObjResponseParser.ToProducts;
        public GetProductsProc(string connectionString, IEnumerable<int>? ids = null) : base(connectionString, ids) { }
    }

    public class ProductsSearchModes : BaseModelsSearchModes
    {
        public DBSearchMode StockMode { get; init; }
        public DBSearchMode UnitPriceMode { get; init; }
        public DBSearchMode AdditionDateMode { get; init; }
        public bool IncludeModelName { get; init; }
        public bool IncludePieceName { get; init; }

        public ProductsSearchModes(
            DBSearchMode? stockMode,
            DBSearchMode? unitPriceMode,
            DBSearchMode? additionDateMode,
            bool includeModelName,
            bool includePieceName
        ) : base(
            new DBSearchModeUnit("@m_stockMode", stockMode ?? DBSearchMode.NotCare),
            new DBSearchModeUnit("@m_unitPriceMode", unitPriceMode ?? DBSearchMode.NotCare),
            new DBSearchModeUnit("@m_additionDateMode", additionDateMode ?? DBSearchMode.NotCare),
            new DBSearchModeUnit(null, includeModelName ? DBSearchMode.Equal : DBSearchMode.NotCare),
            new DBSearchModeUnit(null, includePieceName ? DBSearchMode.Equal : DBSearchMode.NotCare)
        )
        {
            StockMode = stockMode ?? DBSearchMode.NotCare;
            UnitPriceMode = unitPriceMode ?? DBSearchMode.NotCare;
            AdditionDateMode = additionDateMode ?? DBSearchMode.NotCare;
            IncludeModelName = includeModelName;
            IncludePieceName = includePieceName;
        }
    }

    public class SearchProductsProc : BaseDBProc<IEnumerable<Product>>
    {
        public override string ProcName => "spSearchProducts";
        public override Product[] ParseOutput(IEnumerable<DataTable> tables) => DBObjResponseParser.ToProducts(tables.GetTablesEnumerator());

        public SearchProductsProc(
            string connectionString,
            string modelName,
            string pieceName,
            int? stock,
            decimal? unitPrice,
            DateTime additionDate,
            ProductsSearchModes modes
        ) : base(
            connectionString,
            new SqlParameter("@m_modelName", SqlDbType.NVarChar, 60)
            {
                Value = modelName
            }.WithMode(modes.IncludeModelName),
            new SqlParameter("@m_pieceName", SqlDbType.NVarChar, 60)
            {
                Value = pieceName
            }.WithMode(modes.IncludePieceName),
            new SqlParameter("@m_stock", SqlDbType.Int)
            {
                Value = stock
            }.WithMode(modes.StockMode),
            new SqlParameter("@m_unitPrice", SqlDbType.Money)
            {
                Value = unitPrice
            }.WithMode(modes.UnitPriceMode),
            new SqlParameter("@m_additionDate", SqlDbType.DateTime)
            {
                Value = additionDate
            }.WithMode(modes.AdditionDateMode)
        )
        {
            AddMoreParams(modes.SqlParams);
        }
    }

    public class AddProductProc : BaseAddProc<Product>
    {
        public override string ProcName => "spAddProduct";
        public override Func<IEnumerator<DataTable>, IEnumerable<Product>> ParseOutputCallback => DBObjResponseParser.ToProducts;
        public override Func<Product, SqlParameter[]> ObjSqlParamsParserCallback => SqlParamsParser.FromProduct;
        public AddProductProc(string connectionString, Product product) : base(connectionString, product) { }
    }

    public class UpdateProductProc : BaseUpdateProc<Product>
    {
        public override string ProcName => "spUpdateProduct";
        public override Func<IEnumerator<DataTable>, IEnumerable<Product>> ParseOutputCallback => DBObjResponseParser.ToProducts;
        public override Func<Product, SqlParameter[]> ObjSqlParamsParserCallback => SqlParamsParser.FromProduct;
        public UpdateProductProc(string connectionString, Product product) : base(connectionString, product) { }
    }

    public class DeleteProductsProc : BaseDeleteProc
    {
        public override string ProcName => "spDeleteProducts";
        public DeleteProductsProc(string connectionString, IEnumerable<int> ids) : base(connectionString, ids) { }
    }
}
