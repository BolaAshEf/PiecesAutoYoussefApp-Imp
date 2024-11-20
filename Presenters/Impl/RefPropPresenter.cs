using PiecesAutoYoussefApp.DB.Api.Procs;
using PiecesAutoYoussefApp.Extensions;
using PiecesAutoYoussefApp.Models;
using PiecesAutoYoussefApp.Presenters.Base;
using PiecesAutoYoussefApp.UI.Base;
using PiecesAutoYoussefApp.UI.Impl;
using PiecesAutoYoussefApp.UI.Screens;
using SharedORMAppsModels.DB.Base;
using SharedORMAppsPresenters.Base;
using SharedORMAppsUI.States;

namespace PiecesAutoYoussefApp.Presenters.Impl
{
    public abstract class RefPropPresenter<OBJ> : BasePartialPresenter<OBJ> where OBJ : BaseDBObj
    {
        public IPartialView ThisView { get; init; }
        protected IRefPropView MyModelView => (IRefPropView)base.ModelView;

        public RefPropPresenter(
            string connectionString,
            IRefPropView modelView,
            IPartialView partialView
        ) : base(connectionString, modelView, partialView)
        {
            ThisView = partialView;
        }

        protected override void PopulateView(OBJ? model) => MyModelView.PropName = model?.ToString();

        protected override void ApplyOnModelSpecifics()
        {
            MyModelView.ListID.Enabled = true;
            MyModelView.ListID.InEdit = State is AddScreenState || State == ScreenState.Update;
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
    }


    public class ClientCategoryPropPresenter : RefPropPresenter<ClientCategory>
    {
        public ClientCategoryPropPresenter(string connectionString, ClientCategoryDataPanel dataPanel) : base(connectionString, dataPanel, dataPanel)
        {
        }

        protected override ClientCategory CalcModel(object? id) => new ClientCategory(
            categoryName: MyModelView.PropName ?? ""
        )
        {
            Id = id as int?
        };


        protected override IDBInvokable<IEnumerable<ClientCategory>> LoadModelsFromDB() =>
            new GetClientCategoriesProc(ConnectionString)
            .AlsoApply(obj => obj?.ToNamedDecorator())
            .AlsoNotifyFailure();
        public override IDBInvokable<ClientCategory> AddProc() =>
            new AddClientCategoryProc(ConnectionString, CalcModel(null)).AlsoNotifyAdd();
        public override IDBInvokable<ClientCategory> UpdateProc(object? id) =>
            new UpdateClientCategoryProc(ConnectionString, CalcModel(id)).AlsoNotifyUpdate();
        public override IDBInvokable<NoObj> DeleteProc()
        {
            var willDelete = GetModelIDsToDelete();

            return new DeleteClientCategoriesProc(ConnectionString, willDelete)
            .AlsoNotifyDelete()
            .AlsoAssureDeletion(willDelete.Count());
        }
    }

    public class ModelPropPresenter : RefPropPresenter<Model>
    {
        public ModelPropPresenter(string connectionString, ModelDataPanel dataPanel) : base(connectionString, dataPanel, dataPanel)
        {
        }

        protected override Model CalcModel(object? id) => new Model(
            modelName: MyModelView.PropName ?? ""
        )
        {
            Id = id as int?
        };


        protected override IDBInvokable<IEnumerable<Model>> LoadModelsFromDB() =>
            new GetModelsProc(ConnectionString)
            .AlsoApply(obj => obj?.ToNamedDecorator())
            .AlsoNotifyFailure();
        public override IDBInvokable<Model> AddProc() =>
            new AddModelProc(ConnectionString, CalcModel(null)).AlsoNotifyAdd();
        public override IDBInvokable<Model> UpdateProc(object? id) =>
            new UpdateModelProc(ConnectionString, CalcModel(id)).AlsoNotifyUpdate();
        public override IDBInvokable<NoObj> DeleteProc()
        {
            var willDelete = GetModelIDsToDelete();

            return new DeleteModelsProc(ConnectionString, willDelete)
            .AlsoNotifyDelete()
            .AlsoAssureDeletion(willDelete.Count());
        }
    }

    public class PiecePropPresenter : RefPropPresenter<Piece>
    {
        public PiecePropPresenter(string connectionString, PieceDataPanel dataPanel) : base(connectionString, dataPanel, dataPanel)
        {
        }

        protected override Piece CalcModel(object? id) => new Piece(
            pieceName: MyModelView.PropName ?? ""
        )
        {
            Id = id as int?
        };


        protected override IDBInvokable<IEnumerable<Piece>> LoadModelsFromDB() =>
            new GetPiecesProc(ConnectionString)
            .AlsoApply(obj => obj?.ToNamedDecorator())
            .AlsoNotifyFailure();
        public override IDBInvokable<Piece> AddProc() =>
            new AddPieceProc(ConnectionString, CalcModel(null)).AlsoNotifyAdd();
        public override IDBInvokable<Piece> UpdateProc(object? id) =>
            new UpdatePieceProc(ConnectionString, CalcModel(id)).AlsoNotifyUpdate();
        public override IDBInvokable<NoObj> DeleteProc()
        {
            var willDelete = GetModelIDsToDelete();

            return new DeletePiecesProc(ConnectionString, willDelete)
            .AlsoNotifyDelete()
            .AlsoAssureDeletion(willDelete.Count());
        }
    }


    public class AllRefPropsPresenter : ILifeCycle
    {
        private IFullView MainPageCRUDView { get; init; }

        private ClientCategoryPropPresenter ClientCategoryPropPresenter { get; init; }
        private ModelPropPresenter ModelPropPresenter { get; init; }
        private PiecePropPresenter PiecePropPresenter { get; init; }
        public AllRefPropsPresenter(string connectionString, IAllRefPropsView view, IFullView mainPageCRUDView)
        {
            MainPageCRUDView = mainPageCRUDView;

            ClientCategoryPropPresenter = new ClientCategoryPropPresenter(connectionString, view.ClientCategoryDataPanel);
            ModelPropPresenter = new ModelPropPresenter(connectionString, view.ModelDataPanel);
            PiecePropPresenter = new PiecePropPresenter(connectionString, view.PieceDataPanel);
        }


        public void Start()
        {
            MainPageCRUDView.SetAllVisibility(_ => false);

            ClientCategoryPropPresenter.Start();
            ModelPropPresenter.Start();
            PiecePropPresenter.Start();
        }

        public void Dispose()
        {
            ClientCategoryPropPresenter.Dispose();
            ModelPropPresenter.Dispose();
            PiecePropPresenter.Dispose();

            MainPageCRUDView.SetAllVisibility(_ => true);
        }
    }
}
