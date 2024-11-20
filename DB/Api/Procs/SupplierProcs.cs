using Microsoft.Data.SqlClient;
using PiecesAutoYoussefApp.Models;
using PiecesAutoYoussefApp.Parsers;
using SharedORMAppsModels.DB.Impl.Sql.Procs;
using SharedORMAppsModels.DB.Impl.Sql.Search;
using SharedORMAppsModels.Extensions;
using System.Data;

namespace PiecesAutoYoussefApp.DB.Api.Procs
{
    public class GetSuppliersProc : BaseGetProc<Supplier>
    {
        public override string ProcName => "spGetSuppliers";
        public override Func<IEnumerator<DataTable>, IEnumerable<Supplier>> ParseOutputsCallback => DBObjResponseParser.ToSuppliers;
        public GetSuppliersProc(string connectionString, IEnumerable<int>? ids = null) : base(connectionString, ids) { }
    }

    public class SuppliersSearchModes : BaseModelsSearchModes
    {
        public DBSearchMode ProductQuantityMode { get; init; }
        public bool IncludeProductProps { get; init; }

        public SuppliersSearchModes(
            DBSearchMode? productQuantityMode,
            bool includeProductProps
        ) : base(
            new DBSearchModeUnit("@m_productQuantityMode", productQuantityMode ?? DBSearchMode.NotCare),
            new DBSearchModeUnit(null, includeProductProps ? DBSearchMode.Equal : DBSearchMode.NotCare)
        )
        {
            ProductQuantityMode = productQuantityMode ?? DBSearchMode.NotCare;
            IncludeProductProps = includeProductProps;
        }
    }

    public class SearchSuppliersProc : BaseDBProc<IEnumerable<Supplier>>
    {
        public override string ProcName => "spSearchSuppliers";
        public override Supplier[] ParseOutput(IEnumerable<DataTable> tables) => DBObjResponseParser.ToSuppliers(tables.GetTablesEnumerator());

        public SearchSuppliersProc(
            string connectionString,
            string firstName,
            string lastName,
            string phoneNumber,
            string supplierAddress,
            string? productProp,
            int? productQuantity,
            SuppliersSearchModes modes
        ) : base(
            connectionString,
            new SqlParameter("@m_firstName", SqlDbType.NVarChar, 30)
            {
                Value = firstName
            },
            new SqlParameter("@m_lastName", SqlDbType.NVarChar, 30)
            {
                Value = lastName
            },
            new SqlParameter("@m_phoneNumber", SqlDbType.VarChar, 15)
            {
                Value = phoneNumber
            },
            new SqlParameter("@m_supplierAddress", SqlDbType.NVarChar, 50)
            {
                Value = supplierAddress
            },
            new SqlParameter("@m_productRefIDModelPieceName", SqlDbType.NVarChar, 100)
            {
                Value = productProp
            }.WithMode(modes.IncludeProductProps),
            new SqlParameter("@m_productQuantity", SqlDbType.Int)
            {
                Value = productQuantity
            }.WithMode(modes.ProductQuantityMode)
        )
        {
            AddMoreParams(modes.SqlParams);
        }
    }

    public class AddSupplierProc : BaseAddProc<Supplier>
    {
        public override string ProcName => "spAddSupplier";
        public override Func<IEnumerator<DataTable>, IEnumerable<Supplier>> ParseOutputCallback => DBObjResponseParser.ToSuppliers;
        public override Func<Supplier, SqlParameter[]> ObjSqlParamsParserCallback => SqlParamsParser.FromSupplier;
        public AddSupplierProc(string connectionString, Supplier supplier) : base(connectionString, supplier) { }
    }

    public class UpdateSupplierProc : BaseUpdateProc<Supplier>
    {
        public override string ProcName => "spUpdateSupplier";
        public override Func<IEnumerator<DataTable>, IEnumerable<Supplier>> ParseOutputCallback => DBObjResponseParser.ToSuppliers;
        public override Func<Supplier, SqlParameter[]> ObjSqlParamsParserCallback => SqlParamsParser.FromSupplier;
        public UpdateSupplierProc(string connectionString, Supplier supplier) : base(connectionString, supplier) { }
    }

    public class DeleteSuppliersProc : BaseDeleteProc
    {
        public override string ProcName => "spDeleteSuppliers";
        public DeleteSuppliersProc(string connectionString, IEnumerable<int> ids) : base(connectionString, ids) { }
    }
}
