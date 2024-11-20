using PiecesAutoYoussefApp.UI.Base;
using SharedORMAppsModels.DB.Base;
using SharedORMAppsPresenters.Base;
using SharedORMAppsPresenters.Impl.Essentials;
using SharedORMAppsPresenters.Impl.Parts;
using SharedORMAppsUI.Base;
using SharedORMAppsUI.States;

namespace PiecesAutoYoussefApp.Presenters.Base
{
    public abstract class BasePartialPresenter<OBJ> : BaseModelPresenter<OBJ>,
        IAddPresenterEssentials<OBJ>, IUpdatePresenterEssentials<OBJ>, IDeletePresenterEssentials,
        IStateEssentials, ISaveEssentials<OBJ>
        where OBJ : BaseDBObj
    {
        protected string ConnectionString { get; init; }

        protected AddablePresenter<OBJ> Addable { get; init; }
        protected UpdateblePresenter<OBJ> Updatable { get; init; }
        protected ViewablePresenter<OBJ> Viewable { get; init; }
        protected DeletablePresenter<OBJ> Deletable { get; init; }

        private IEnumerable<BaseViewPresenter<OBJ>> _views;
        public override IEnumerable<BaseViewPresenter<OBJ>> Views => _views;

        public BasePartialPresenter(string connectionString, IModelView modelView, IPartialView partialView) : base(modelView)
        {
            ConnectionString = connectionString;

            Addable = new(this, partialView, this, this, this);
            Updatable = new(this, partialView, this, this, this);
            Viewable = new(this, partialView, this);
            Deletable = new(this, partialView, this);

            _views = [Addable, Updatable, Viewable, Deletable];
        }


        protected virtual IEnumerable<int> GetModelIDsToDelete() =>
            CurrentModel?.Key is int id ? [id] : [];

        public void SetStateButtonBorder(Control? control, int size = 2, Color? color = null)
        {
            if (control is Button button)
            {
                button.FlatAppearance.BorderSize = size;
                button.FlatAppearance.BorderColor = color ?? Color.Black;
            }
        }

        public virtual bool CanDelete() => State == ScreenState.View;
        public virtual void DoAfterDeletion()
        {
            if (State == ScreenState.View)
            {
                LoadModels();

                if (Models.Count() > 0)
                {
                    if (!Models.Contains(LastModel!))
                    {
                        LastModel = Models.First();
                    }

                    if (CurrentModel == null || !Models.Contains(CurrentModel!))
                    {
                        CurrentModel = Models.First();
                    }
                }
            }
        }

        public bool CanSave() =>
            State is AddScreenState || State == ScreenState.Update;

        public void ProcessSaveResult(OBJ? result)
        {
            LoadModels();

            LastModel = result;
            State = ScreenState.View;
        }

        public abstract IDBInvokable<OBJ> AddProc();
        public abstract IDBInvokable<NoObj> DeleteProc();
        public abstract IDBInvokable<OBJ> UpdateProc(object? id);
    }
}
