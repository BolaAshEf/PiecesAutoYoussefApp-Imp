using PiecesAutoYoussefApp.UI.Base;
using SharedAppsUtils.Parsers;
using SharedORMAppsModels.DB.Base;
using SharedORMAppsModels.DB.Impl.Sql.Search;
using SharedORMAppsPresenters.Base;
using SharedORMAppsPresenters.Impl.Essentials;
using SharedORMAppsPresenters.Impl.Parts;
using SharedORMAppsPresenters.Impl.Sql.Parts;
using SharedORMAppsUI.Base;
using SharedORMAppsUI.States;
using System.Data;

namespace PiecesAutoYoussefApp.Presenters.Base
{
    public abstract class BaseFullPresenter<OBJ> : BasePartialPresenter<OBJ>,
        ISqlSearchPresenterEssentials<OBJ>,
        IWithCountPresenterEssentials, IWithExportPresenterEssentials, IWithGridViewPresenterEssentials<OBJ>
        where OBJ : BaseDBObj
    {
        protected SqlSearchablePresenter<OBJ> Searchable { get; init; }
        protected WithCountPresenter<OBJ> Countable { get; init; }
        protected WithExportPresenter<OBJ> Exportable { get; init; }
        protected WithGridViewPresenter<OBJ> WithGrid { get; init; }


        private IEnumerable<BaseViewPresenter<OBJ>> _views;
        public override IEnumerable<BaseViewPresenter<OBJ>> Views => _views;

        private IFullView ThisView { get; init; }

        public BaseFullPresenter(string connectionString, IModelView modelView, IFullView fullView) : base(connectionString, modelView, fullView)
        {
            ThisView = fullView;

            Searchable = new(this, fullView, this, this, new SqlSearchState<OBJ>(), 500); // default 500ms
            Countable = new(this, fullView, this);
            Exportable = new(this, fullView, this);
            WithGrid = new(this, fullView, this);

            _views = [.. base.Views, Searchable, Countable, Exportable, WithGrid];
        }

        public int IntervalCheckMS
        {
            get => Searchable.IntervalCheckMS;
            set => Searchable.IntervalCheckMS = value;
        }

        public string? CalcCountText() =>
            State == ScreenState.View ? Models.Count().ToString() : null;


        public override void Start()
        {
            base.Start();

            StateChanged += BasePartialPresenter_StateChanged;
        }

        public override void Dispose()
        {
            StateChanged -= BasePartialPresenter_StateChanged;

            base.Dispose();
        }

        private void BasePartialPresenter_StateChanged(object? sender, ObjectChangedEventArgs<ScreenState> e)
        {
            if (State is AddScreenState || State == ScreenState.Update)
            {
                ThisView.SetGVDataSource([], new DataTable());
            }
        }



        protected override IEnumerable<int> GetModelIDsToDelete() => ThisView.SelectedGVIDs.OfType<int>();


        public bool CanExport() =>
            State == ScreenState.View || State == ScreenState.Search;

        public void OnExportCSV(object? sender, EventArgs e)
        {
            if (State != ScreenState.View && State != ScreenState.Search) { return; }

            object? gridDataSource = ThisView.GetGVDataSource();
            if (gridDataSource != null && gridDataSource is DataTable gridDataTable)
            {
                CSVParser.Export(gridDataTable);
            }
        }

        public override bool CanDelete() =>
            base.CanDelete() || State == ScreenState.Search;
        public override void DoAfterDeletion()
        {
            base.DoAfterDeletion();

            if (State == ScreenState.Search)
            {
                Searchable.OnSearch();
            }
        }

        public void LoadSearchResults(IEnumerable<OBJ> result) =>
            ThisView.SetGVDataSource(result.Select(obj => obj.Key).OfType<object>(), FromModels(result));

        public abstract DataTable FromModels(IEnumerable<OBJ> models);

        public abstract IDBInvokable<IEnumerable<OBJ>> SearchProc();
        public virtual BaseModelsSearchModes CalcModes() => new BaseModelsSearchModes();
    }
}
