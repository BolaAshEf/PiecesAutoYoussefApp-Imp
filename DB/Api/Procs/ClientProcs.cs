using Microsoft.Data.SqlClient;
using PiecesAutoYoussefApp.Models;
using PiecesAutoYoussefApp.Parsers;
using SharedORMAppsModels.DB.Impl.Sql.Procs;
using SharedORMAppsModels.DB.Impl.Sql.Search;
using SharedORMAppsModels.Extensions;
using System.Data;

namespace PiecesAutoYoussefApp.DB.Api.Procs
{
    public class GetClientsProc : BaseGetProc<Client>
    {
        public override string ProcName => "spGetClients";
        public override Func<IEnumerator<DataTable>, IEnumerable<Client>> ParseOutputsCallback => DBObjResponseParser.ToClients;
        public GetClientsProc(string connectionString, IEnumerable<int>? ids = null) : base(connectionString, ids) { }
    }

    public class ClientsSearchModes : BaseModelsSearchModes
    {
        public bool IncludeCategory { get; init; }
        public ClientsSearchModes(
            bool includeCategory
        ) : base(
            new DBSearchModeUnit(null, includeCategory ? DBSearchMode.Equal : DBSearchMode.NotCare)
        )
        {
            IncludeCategory = includeCategory;
        }
    }

    public class SearchClientsProc : BaseDBProc<IEnumerable<Client>>
    {
        public override string ProcName => "spSearchClients";

        public override IEnumerable<Client> ParseOutput(IEnumerable<DataTable> tables) =>
            DBObjResponseParser.ToClients(tables.GetTablesEnumerator());

        public SearchClientsProc(
            string connectionString,
            string firstName,
            string lastName,
            string phoneNumber,
            string clientAddress,
            string categoryName,
            ClientsSearchModes modes
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
            new SqlParameter("@m_clientAddress", SqlDbType.NVarChar, 50)
            {
                Value = clientAddress
            },
            new SqlParameter("@m_categoryName", SqlDbType.NVarChar, 60)
            {
                Value = categoryName
            }.WithMode(modes.IncludeCategory)
        )
        { }
    }

    public class AddClientProc : BaseAddProc<Client>
    {
        public override string ProcName => "spAddClient";
        public override Func<IEnumerator<DataTable>, IEnumerable<Client>> ParseOutputCallback => DBObjResponseParser.ToClients;
        public override Func<Client, SqlParameter[]> ObjSqlParamsParserCallback => SqlParamsParser.FromClient;
        public AddClientProc(string connectionString, Client client) : base(connectionString, client) { }
    }

    public class UpdateClientProc : BaseUpdateProc<Client>
    {
        public override string ProcName => "spUpdateClient";
        public override Func<IEnumerator<DataTable>, IEnumerable<Client>> ParseOutputCallback => DBObjResponseParser.ToClients;
        public override Func<Client, SqlParameter[]> ObjSqlParamsParserCallback => SqlParamsParser.FromClient;
        public UpdateClientProc(string connectionString, Client client) : base(connectionString, client) { }
    }

    public class DeleteClientsProc : BaseDeleteProc
    {
        public override string ProcName => "spDeleteClients";
        public DeleteClientsProc(string connectionString, IEnumerable<int> ids) : base(connectionString, ids) { }
    }

}
