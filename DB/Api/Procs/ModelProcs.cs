using Microsoft.Data.SqlClient;
using PiecesAutoYoussefApp.Models;
using PiecesAutoYoussefApp.Parsers;
using SharedORMAppsModels.DB.Impl.Sql.Procs;
using System.Data;

namespace PiecesAutoYoussefApp.DB.Api.Procs
{
    public class GetModelsProc : BaseGetProc<Model>
    {
        public override string ProcName => "spGetModels";
        public override Func<IEnumerator<DataTable>, IEnumerable<Model>> ParseOutputsCallback => DBObjResponseParser.ToModels;
        public GetModelsProc(string connectionString, IEnumerable<int>? ids = null) : base(connectionString, ids) { }
    }

    public class AddModelProc : BaseAddProc<Model>
    {
        public override string ProcName => "spAddModel";
        public override Func<IEnumerator<DataTable>, IEnumerable<Model>> ParseOutputCallback => DBObjResponseParser.ToModels;
        public override Func<Model, SqlParameter[]> ObjSqlParamsParserCallback => SqlParamsParser.FromModel;
        public AddModelProc(string connectionString, Model model) : base(connectionString, model) { }
    }

    public class UpdateModelProc : BaseUpdateProc<Model>
    {
        public override string ProcName => "spUpdateModel";
        public override Func<IEnumerator<DataTable>, IEnumerable<Model>> ParseOutputCallback => DBObjResponseParser.ToModels;
        public override Func<Model, SqlParameter[]> ObjSqlParamsParserCallback => SqlParamsParser.FromModel;
        public UpdateModelProc(string connectionString, Model model) : base(connectionString, model) { }
    }

    public class DeleteModelsProc : BaseDeleteProc
    {
        public override string ProcName => "spDeleteModels";
        public DeleteModelsProc(string connectionString, IEnumerable<int> ids) : base(connectionString, ids) { }
    }

}
