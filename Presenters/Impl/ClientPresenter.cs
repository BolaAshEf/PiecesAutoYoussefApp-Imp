using PiecesAutoYoussefApp.DB.Api.Procs;
using PiecesAutoYoussefApp.Extensions;
using PiecesAutoYoussefApp.Models;
using PiecesAutoYoussefApp.Parsers;
using PiecesAutoYoussefApp.Presenters.Base;
using PiecesAutoYoussefApp.UI.Base;
using PiecesAutoYoussefApp.UI.Impl;
using SharedORMAppsModels.DB.Base;
using SharedORMAppsModels.Extensions;
using System.Data;
using static PiecesAutoYoussefApp.Extensions.PresentersExtensions;

namespace PiecesAutoYoussefApp.Presenters.Impl
{
    public class ClientPresenter : BaseFullPresenter<Client>
    {
        new private IClientsView ModelView => (IClientsView)base.ModelView;

        public ClientPresenter(string connectionString, IClientsView modelView, IFullView fullView) :
            base(connectionString, modelView, fullView)
        {

        }


        protected ClientCategory CalcClientCategory() => (
            ModelView.Category is ClientCategory category
            ? category
            : new ClientCategory(categoryName: ModelView.Category?.ToString() ?? "")
        ).ToNamedDecorator();

        protected IDBInvokable<IEnumerable<ClientCategory>> LoadCategoriesProc() =>
            new GetClientCategoriesProc(ConnectionString);


        public override DataTable FromModels(IEnumerable<Client> models) =>
            GridViewTableParser.FromClients(models);

        public override ClientsSearchModes CalcModes() => new ClientsSearchModes(
            ModelView.EditableCategory.SearchInclude == true
        );





        protected override IDBInvokable<IEnumerable<Client>> LoadModelsFromDB() => new DBInvokableResHandlerDecorator<IEnumerable<Client>>(
            new GetClientsProc(ConnectionString),
            resCallback =>
            {
                DBResponse categoriesRes = LoadCategoriesProc().Invoke(out var obj);
                if (obj == null)
                {
                    return categoriesRes;
                }
                else
                {
                    ModelView.Categories = obj.ToNamedDecorator().ToArray();

                    return resCallback();
                }
            }
        ).AlsoNotifyFailure();


        public override IDBInvokable<Client> AddProc() => new AddClientProc(ConnectionString, CalcModel(null)).AlsoNotifyAdd();
        public override IDBInvokable<Client> UpdateProc(object? id) => new UpdateClientProc(ConnectionString, CalcModel(id)).AlsoNotifyUpdate();
        public override IDBInvokable<IEnumerable<Client>> SearchProc() => new SearchClientsProc(
            connectionString: ConnectionString,
            firstName: ModelView.FirstName,
            lastName: ModelView.LastName,
            clientAddress: ModelView.Address,
            phoneNumber: ModelView.PhoneNumber,
            categoryName: CalcClientCategory().CategoryName,
            modes: CalcModes()
        ).AlsoNotifyFailure();
        public override IDBInvokable<NoObj> DeleteProc()
        {
            var willDelete = GetModelIDsToDelete();

            return new DeleteClientsProc(ConnectionString, willDelete)
            .AlsoNotifyDelete()
            .AlsoAssureDeletion(willDelete.Count());
        }



        protected override void PopulateView(Client? model)
        {
            ModelView.FirstName = model?.FirstName ?? "";
            ModelView.LastName = model?.LastName ?? "";
            ModelView.Address = model?.ClientAddress ?? "";
            ModelView.PhoneNumber = model?.PhoneNumber ?? "";
            ModelView.Category = model?.ClientCategory.ToNamedDecorator();

            if (model == null)
            {
                ModelView.EditableCategory.SetToSearchExcludeIfInSearch();
            }
        }

        protected override Client CalcModel(object? id) => new Client(
            firstName: ModelView.FirstName,
            lastName: ModelView.LastName,
            clientAddress: ModelView.Address,
            phoneNumber: ModelView.PhoneNumber,
            clientCategory: CalcClientCategory()
        )
        {
            Id = id as int?
        };

        protected override void ApplyOnModelSpecifics()
        {
            ModelView.ListID.Enabled = State.IsRecomenddedIDEnable();

            ModelView.TxtFirstName.ReadOnly
                = ModelView.TxtLastName.ReadOnly
                = ModelView.TxtAddress.ReadOnly
                = ModelView.TxtPhoneNumber.ReadOnly
                = State.IsTxtReadOnly();

            ModelView.EditableCategory.Mode = State.RefComboMode();
        }
    }
}
