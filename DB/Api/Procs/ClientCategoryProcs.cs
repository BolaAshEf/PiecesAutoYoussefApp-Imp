using Microsoft.Data.SqlClient;
using PiecesAutoYoussefApp.Models;
using PiecesAutoYoussefApp.Parsers;
using SharedORMAppsModels.DB.Impl.Sql.Procs;
using System.Data;

namespace PiecesAutoYoussefApp.DB.Api.Procs
{
    public class GetClientCategoriesProc : BaseGetProc<ClientCategory>
    {
        public override string ProcName => "spGetClientCategories";
        public override Func<IEnumerator<DataTable>, IEnumerable<ClientCategory>> ParseOutputsCallback => DBObjResponseParser.ToClientCategories;
        public GetClientCategoriesProc(string connectionString, IEnumerable<int>? ids = null) : base(connectionString, ids) { }
    }

    public class AddClientCategoryProc : BaseAddProc<ClientCategory>
    {
        public override string ProcName => "spAddClientCategory";
        public override Func<IEnumerator<DataTable>, IEnumerable<ClientCategory>> ParseOutputCallback => DBObjResponseParser.ToClientCategories;
        public override Func<ClientCategory, SqlParameter[]> ObjSqlParamsParserCallback => SqlParamsParser.FromClientCategory;
        public AddClientCategoryProc(string connectionString, ClientCategory clientCategory) : base(connectionString, clientCategory) { }
    }

    public class UpdateClientCategoryProc : BaseUpdateProc<ClientCategory>
    {
        public override string ProcName => "spUpdateClientCategory";
        public override Func<IEnumerator<DataTable>, IEnumerable<ClientCategory>> ParseOutputCallback => DBObjResponseParser.ToClientCategories;
        public override Func<ClientCategory, SqlParameter[]> ObjSqlParamsParserCallback => SqlParamsParser.FromClientCategory;
        public UpdateClientCategoryProc(string connectionString, ClientCategory clientCategory) : base(connectionString, clientCategory) { }
    }

    public class DeleteClientCategoriesProc : BaseDeleteProc
    {
        public override string ProcName => "spDeleteClientCategories";
        public DeleteClientCategoriesProc(string connectionString, IEnumerable<int> ids,
            params SqlParameter[] _params) : base(connectionString, ids, _params) { }
    }
}
