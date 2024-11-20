using PiecesAutoYoussefApp.DB.Api.Procs;
using PiecesAutoYoussefApp.Extensions;
using PiecesAutoYoussefApp.Models;
using PiecesAutoYoussefApp.NotificationCenter;
using PiecesAutoYoussefApp.Presenters.Base;
using PiecesAutoYoussefApp.UI.Base;
using PiecesAutoYoussefApp.UI.Impl;
using SharedORMAppsModels.DB.Base;
using SharedORMAppsModels.Extensions;
using SharedORMAppsUI.Controls;
using SharedORMAppsUI.States;

namespace PiecesAutoYoussefApp.Presenters.Impl
{
    public class ClientOrderCollectionPresenter : BasePartialPresenter<OrderCollection>
    {
        public IPartialView ThisView { get; init; }
        new private IClientOrderCollectionsView ModelView => (IClientOrderCollectionsView)base.ModelView;

        public IFullView MainFormCRUDView { get; init; }
        private OrderPresenter? _orderPresenter;

        public ClientOrderCollectionPresenter(
            string connectionString,
            IClientOrderCollectionsView modelView,
            IPartialView partialView,
            IFullView mainFormCRUDView
        ) : base(connectionString, modelView, partialView)
        {
            ThisView = partialView;
            MainFormCRUDView = mainFormCRUDView;
        }

        public override void Dispose()
        {
            MainFormCRUDView.SetAllVisibility(_ => true);

            _orderPresenter?.Dispose();

            base.Dispose();
        }

        protected override OrderCollection CalcModel(object? id) => new OrderCollection(
            client: (ModelView.CollectionClient as Client
                ?? throw ConstraintKey.ClientFirstNameNotEmpty.NotifyErr()).ToNamedDecorator()
        )
        {
            Id = id as int?
        };

        protected override void PopulateView(OrderCollection? model)
        {
            ModelView.CollectionClient = model?.Client.ToNamedDecorator();

            if (model != _orderPresenter?.Collection)
            {
                _orderPresenter?.Dispose();
                if (model == null)
                {
                    _orderPresenter = null;
                }
                else
                {
                    _orderPresenter = new OrderPresenter(ConnectionString, ModelView.OrderDataPanel, MainFormCRUDView, model);
                    _orderPresenter.Start();
                }
            }
        }


        protected override void ApplyOnModelSpecifics()
        {
            ModelView.ListID.Enabled = State.IsRecomenddedIDEnable();


            if (State is AddScreenState || State == ScreenState.Update)
            {
                ModelView.ListCollectionClient.Mode = EditableRefPropCombo.ComboMode.ChooseFromList;

                ModelView.OrderDataPanel.Visible = false;
                MainFormCRUDView.SetAllVisibility(_ => false); // ok
            }
            else if (State == ScreenState.View)
            {
                ModelView.ListCollectionClient.Mode = EditableRefPropCombo.ComboMode.ReadOnly;

                ModelView.OrderDataPanel.Visible = true;
            }
        }

        public override void ApplyOnViewSpecifics()
        {
            base.ApplyOnViewSpecifics();

            // Custom Update For Buttons
            bool inView = State == ScreenState.View;
            ThisView.BtnSave.Visible
                = ThisView.BtnView.Visible // cancel button
                = !inView;
            ThisView.BtnAdd.Visible
                = ThisView.BtnUpdate.Visible
                = ThisView.BtnDelete.Visible
                = inView;
        }


        protected IDBInvokable<IEnumerable<Client>> LoadCollectionClientsProc() => new GetClientsProc(ConnectionString);
        protected override IDBInvokable<IEnumerable<OrderCollection>> LoadModelsFromDB() =>
            new DBInvokableResHandlerDecorator<IEnumerable<OrderCollection>>(
            new GetClientOrderCollectionsProc(ConnectionString),
            resCallback =>
            {
                DBResponse res = LoadCollectionClientsProc().Invoke(out var obj);
                if (obj == null)
                {
                    return res;
                }
                else
                {
                    ModelView.CollectionClients = obj.ToNamedDecorator().ToArray();
                    return resCallback();
                }
            }
        ).AlsoNotifyFailure();

        public override IDBInvokable<OrderCollection> AddProc() =>
            new AddClientOrderCollectionProc(ConnectionString, CalcModel(null)).AlsoNotifyAdd();
        public override IDBInvokable<OrderCollection> UpdateProc(object? id) =>
            new UpdateClientOrderCollectionProc(ConnectionString, CalcModel(id)).AlsoNotifyUpdate();
        public override IDBInvokable<NoObj> DeleteProc()
        {
            var willDelete = GetModelIDsToDelete();

            return new DeleteClientOrderCollectionsProc(ConnectionString, willDelete)
            .AlsoNotifyDelete()
            .AlsoAssureDeletion(willDelete.Count());
        }
    }
}
