using PiecesAutoYoussefApp.DB.Api.Procs;
using PiecesAutoYoussefApp.Extensions;
using PiecesAutoYoussefApp.Models;
using PiecesAutoYoussefApp.NotificationCenter;
using PiecesAutoYoussefApp.Parsers;
using PiecesAutoYoussefApp.Presenters.Base;
using PiecesAutoYoussefApp.UI.Base;
using PiecesAutoYoussefApp.UI.Impl;
using SharedORMAppsModels.DB.Base;
using SharedORMAppsModels.Extensions;
using SharedORMAppsUI.Base;
using SharedORMAppsUI.States;
using System.Data;

namespace PiecesAutoYoussefApp.Presenters.Impl
{
    public class ProductPresenter : BaseFullPresenter<Product>
    {
        new private IProductsView ModelView => (IProductsView)base.ModelView;

        public ProductPresenter(string connectionString, IProductsView modelView, IFullView fullView) :
            base(connectionString, modelView, fullView)
        {

        }

        protected string CalcProductRefID()
        {
            if (ModelView.ID is Product product)
            {
                return product.ReferenceID;
            }

            return ModelView.ID?.ToString() ?? "";
        }

        protected Model CalcProductModel() => (ModelView.ProductModel is Model model
            ? model
            : new Model(modelName: ModelView.ProductModel?.ToString() ?? "")).ToNamedDecorator();

        protected Piece CalcProductPiece() => (ModelView.ProductPiece is Piece piece
            ? piece
            : new Piece(pieceName: ModelView.ProductPiece?.ToString() ?? "")).ToNamedDecorator();


        public override DataTable FromModels(IEnumerable<Product> models) =>
            GridViewTableParser.FromProducts(models);



        protected IDBInvokable<IEnumerable<Model>> LoadProductModelsProc() => new GetModelsProc(ConnectionString);
        protected IDBInvokable<IEnumerable<Piece>> LoadProductPiecesProc() => new GetPiecesProc(ConnectionString);





        protected override IDBInvokable<IEnumerable<Product>> LoadModelsFromDB() => new DBInvokableResHandlerDecorator<IEnumerable<Product>>(
            new GetProductsProc(ConnectionString),
            resCallback =>
            {
                DBResponse resProductModels = LoadProductModelsProc().Invoke(out var objProductModels);
                DBResponse resProductPieces = LoadProductPiecesProc().Invoke(out var objProductPieces);
                if (objProductModels == null)
                {
                    return resProductModels;
                }
                else if (objProductPieces == null)
                {
                    return resProductPieces;
                }
                else
                {
                    ModelView.ProductModels = objProductModels.ToNamedDecorator().ToArray();
                    ModelView.ProductPieces = objProductPieces.ToNamedDecorator().ToArray();

                    return resCallback();
                }
            }
        ).AlsoApply(obj => obj?.ToNamedDecorator()).AlsoNotifyFailure();

        public override IDBInvokable<Product> AddProc() => new AddProductProc(ConnectionString, CalcModel(null)).AlsoNotifyAdd();
        public override IDBInvokable<Product> UpdateProc(object? id) => new UpdateProductProc(ConnectionString, CalcModel(id)).AlsoNotifyUpdate();
        public override IDBInvokable<IEnumerable<Product>> SearchProc() => new SearchProductsProc(
            connectionString: ConnectionString,
            stock: ModelView.Stock,
            unitPrice: ModelView.UnitPrice,
            additionDate: ModelView.AdditionDate,
            modelName: CalcProductModel().ModelName,
            pieceName: CalcProductPiece().PieceName,
            modes: CalcModes()
        ).AlsoApply(obj => obj?.ToNamedDecorator()).AlsoNotifyFailure();
        public override IDBInvokable<NoObj> DeleteProc()
        {
            var willDelete = GetModelIDsToDelete();

            return new DeleteProductsProc(ConnectionString, willDelete)
            .AlsoNotifyDelete()
            .AlsoAssureDeletion(willDelete.Count());
        }


        public override ProductsSearchModes CalcModes() => new ProductsSearchModes(
            ModelView.NumStock.SearchMode?.ToDBSearchMode(),
            ModelView.NumUnitPrice.SearchMode?.ToDBSearchMode(),
            ModelView.DateAddition.SearchMode?.ToDBSearchMode(),
            ModelView.ListModel.SearchInclude == true,
            ModelView.ListPiece.SearchInclude == true
        );

        protected override void PopulateView(Product? model)
        {
            ModelView.ID = model?.ReferenceID;
            ModelView.Stock = model?.Stock;
            ModelView.UnitPrice = model?.UnitPrice;
            ModelView.AdditionDate = model?.AdditionDate ?? DateTime.Now;
            ModelView.ProductModel = model?.Model.ToNamedDecorator();
            ModelView.ProductPiece = model?.Piece.ToNamedDecorator();

            if (model == null)
            {
                if (ModelView.NumStock.SearchMode != null)
                {
                    ModelView.NumStock.SearchMode = ButtonSearchMode.NotCare;
                }
                if (ModelView.NumUnitPrice.SearchMode != null)
                {
                    ModelView.NumUnitPrice.SearchMode = ButtonSearchMode.NotCare;
                }
                if (ModelView.DateAddition.SearchMode != null)
                {
                    ModelView.DateAddition.SearchMode = ButtonSearchMode.NotCare;
                }

                ModelView.ListModel.SetToSearchExcludeIfInSearch();
                ModelView.ListPiece.SetToSearchExcludeIfInSearch();
            }
        }

        protected override Product CalcModel(object? id) => new Product(
            referenceID: CalcProductRefID(),
            stock: ModelView.Stock ?? throw ConstraintKey.ProductStockNonNegative.NotifyErr(),
            unitPrice: ModelView.UnitPrice ?? throw ConstraintKey.ProductUnitPriceNonNegative.NotifyErr(),
            additionDate: ModelView.AdditionDate,
            model: CalcProductModel().ToNamedDecorator(),
            piece: CalcProductPiece().ToNamedDecorator()
        )
        {
            Id = id as int?
        };

        protected override void ApplyOnModelSpecifics()
        {
            ModelView.ListID.Enabled = State is AddScreenState || State == ScreenState.Update || State == ScreenState.View;
            ModelView.ListID.InEdit = State is AddScreenState || State == ScreenState.Update;

            ModelView.NumStock.ReadOnly
                = ModelView.NumUnitPrice.ReadOnly
                = ModelView.DateAddition.ReadOnly
                = State.IsTxtReadOnly();

            ModelView.NumStock.SearchMode
                = ModelView.NumUnitPrice.SearchMode
                = ModelView.DateAddition.SearchMode
                = State.SearchMode()?.ToButtonSearchMode();

            ModelView.ListModel.Mode = ModelView.ListPiece.Mode = State.RefComboMode();
        }
    }
}
