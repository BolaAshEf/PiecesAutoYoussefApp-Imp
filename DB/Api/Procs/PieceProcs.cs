using Microsoft.Data.SqlClient;
using PiecesAutoYoussefApp.Models;
using PiecesAutoYoussefApp.Parsers;
using SharedORMAppsModels.DB.Impl.Sql.Procs;
using System.Data;

namespace PiecesAutoYoussefApp.DB.Api.Procs
{
    public class GetPiecesProc : BaseGetProc<Piece>
    {
        public override string ProcName => "spGetPieces";
        public override Func<IEnumerator<DataTable>, IEnumerable<Piece>> ParseOutputsCallback => DBObjResponseParser.ToPieces;
        public GetPiecesProc(string connectionString, IEnumerable<int>? ids = null) : base(connectionString, ids) { }
    }

    public class AddPieceProc : BaseAddProc<Piece>
    {
        public override string ProcName => "spAddPiece";
        public override Func<IEnumerator<DataTable>, IEnumerable<Piece>> ParseOutputCallback => DBObjResponseParser.ToPieces;
        public override Func<Piece, SqlParameter[]> ObjSqlParamsParserCallback => SqlParamsParser.FromPiece;
        public AddPieceProc(string connectionString, Piece piece) : base(connectionString, piece) { }
    }

    public class UpdatePieceProc : BaseUpdateProc<Piece>
    {
        public override string ProcName => "spUpdatePiece";
        public override Func<IEnumerator<DataTable>, IEnumerable<Piece>> ParseOutputCallback => DBObjResponseParser.ToPieces;
        public override Func<Piece, SqlParameter[]> ObjSqlParamsParserCallback => SqlParamsParser.FromPiece;
        public UpdatePieceProc(string connectionString, Piece piece) : base(connectionString, piece) { }
    }

    public class DeletePiecesProc : BaseDeleteProc
    {
        public override string ProcName => "spDeletePieces";
        public DeletePiecesProc(string connectionString, IEnumerable<int> ids) : base(connectionString, ids) { }
    }
}
