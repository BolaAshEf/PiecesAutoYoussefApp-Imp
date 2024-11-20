using PiecesAutoYoussefApp.DB.Api.Procs;
using PiecesAutoYoussefApp.Extensions;
using PiecesAutoYoussefApp.Models;
using PiecesAutoYoussefApp.NotificationCenter;
using PiecesAutoYoussefApp.Parsers;
using PiecesAutoYoussefApp.Presenters.Base;
using PiecesAutoYoussefApp.UI.Base;
using PiecesAutoYoussefApp.UI.Impl;
using PiecesAutoYoussefApp.UI.Screens;
using SharedORMAppsModels.DB.Base;
using SharedORMAppsModels.Extensions;
using SharedORMAppsUI.Base;
using SharedORMAppsUI.States;
using System.Data;
using static PiecesAutoYoussefApp.Extensions.SearchModeExtensions;

namespace PiecesAutoYoussefApp.Presenters.Impl
{
    public class OrderPresenter : BaseFullPresenter<Order>
    {
        public OrderCollection Collection { get; init; }
        private IFullView ThisView { get; init; }
        new private IOrdersView ModelView => (IOrdersView)base.ModelView;

        public OrderPresenter(string connectionString, IOrdersView modelView, IFullView fullView, OrderCollection collection) :
            base(connectionString, modelView, fullView)
        {
            Collection = collection;
            ThisView = fullView;
        }

        private void OnPrint(object? sender, EventArgs e)
        {
            if (State != ScreenState.View || ThisView.GetGVDataSource() is not DataTable || Collection.Id == null)
            {
                return;
            }

            OrdersBillForm ordersBillForm = new OrdersBillForm(
                Collection.Id ?? -1,
                Collection.Client,
                DateTime.Now,
                ThisView.SelectedGVIDs.Cast<int>()
                .Select(
                    (id) => Models.FirstOrDefault((order) => order?.Id == id, null)
                )
                .Where((order) => order != null)
                .Cast<Order>()
                .ToList()
            );

            ordersBillForm.ShowDialog();
        }

        public override void Start()
        {
            base.Start();

            ModelView.BtnPrint.Click += OnPrint;
        }

        public override void Dispose()
        {
            ModelView.BtnPrint.Click -= OnPrint;

            base.Dispose();
        }

        public override DataTable FromModels(IEnumerable<Order> models) =>
            GridViewTableParser.FromOrders(models);


        // control values and reset modes in search(when clear).
        protected override void PopulateView(Order? model)
        {
            ModelView.NoTaxPrice = model?.NoTaxTotalPrice;
            ModelView.TotalPrice = model?.TotalPrice;
            ModelView.OrderProduct = model?.Product.ToNamedDecorator();
            ModelView.Quantity = model?.Quantity;
            ModelView.UnitPrice = model?.UnitPrice;
            ModelView.VatRate = model?.VatRate;
            ModelView.OrderDate = model?.OrderDate ?? DateTime.Now;
            //UpdateStock = model?.UpdateProductsThen;
            if (model == null)
            {
                if (ModelView.NumQuantity.SearchMode != null)
                {
                    ModelView.NumQuantity.SearchMode = ButtonSearchMode.NotCare;
                }
                if (ModelView.NumUnitPrice.SearchMode != null)
                {
                    ModelView.NumUnitPrice.SearchMode = ButtonSearchMode.NotCare;
                }
                if (ModelView.NumVatRate.SearchMode != null)
                {
                    ModelView.NumVatRate.SearchMode = ButtonSearchMode.NotCare;
                }
                if (ModelView.DateOrder.SearchMode != null)
                {
                    ModelView.DateOrder.SearchMode = ButtonSearchMode.NotCare;
                }

                ModelView.ListProduct.SetToSearchExcludeIfInSearch();
            }

            if (model == null)
            {
                ModelView.UpdateStock = ModelView.ChkUpdateStock.ThreeState ? null : true;
            }
            else
            {
                ModelView.UpdateStock = model?.UpdateProductsThen;
            }
        }

        protected override Order CalcModel(object? id) => new Order(
            product: (ModelView.OrderProduct as Product)?.ToNamedDecorator()
                ?? throw ConstraintKey.ProductReferenceIDNotEmpty.NotifyErr(),
            quantity: ModelView.Quantity ?? throw ConstraintKey.OrderQuantityPositive.NotifyErr(),
            unitPrice: ModelView.UnitPrice ?? throw ConstraintKey.OrderUnitPriceNonNegative.NotifyErr(),
            vatRate: ModelView.VatRate ?? throw ConstraintKey.OrderVatRateNonNegative.NotifyErr(),
            orderDate: ModelView.OrderDate,
            updateProductsThen: ModelView.UpdateStock ?? false
        )
        {
            Id = id as int?
        };

        // control other properties other than value for each control.
        protected override void ApplyOnModelSpecifics()
        {
            ModelView.ListID.Enabled = State.IsRecomenddedIDEnable();

            ModelView.NumQuantity.ReadOnly
                = ModelView.NumUnitPrice.ReadOnly
                = ModelView.NumVatRate.ReadOnly
                = ModelView.DateOrder.ReadOnly
                = State.IsTxtReadOnly();

            ModelView.NumQuantity.SearchMode
                = ModelView.NumUnitPrice.SearchMode
                = ModelView.NumVatRate.SearchMode
                = ModelView.DateOrder.SearchMode
                = State.SearchMode()?.ToButtonSearchMode();

            ModelView.NumNoTaxTotalPrice.Visible
                = ModelView.NumTotalPrice.Visible
                = State == ScreenState.View;

            ModelView.ChkUpdateStock.Enabled = State != ScreenState.View;
            ModelView.ChkUpdateStock.ThreeState = State == ScreenState.Search;

            ModelView.ListProduct.Mode = State.RefComboNoEditMode();

            ModelView.BtnPrint.Visible = State == ScreenState.View;
        }



        protected IDBInvokable<IEnumerable<Product>> LoadOrderProductsProc() => new GetProductsProc(ConnectionString);
        protected override IDBInvokable<IEnumerable<Order>> LoadModelsFromDB() => new DBInvokableResHandlerDecorator<IEnumerable<Order>>(
            new GetOrdersProc(ConnectionString, Collection),
            resCallback =>
            {
                DBResponse resOrderProducts = LoadOrderProductsProc().Invoke(out var objOrderProducts);
                if (objOrderProducts == null)
                {
                    return resOrderProducts;
                }
                else
                {
                    ModelView.OrderProducts = objOrderProducts.ToNamedDecorator().ToArray();

                    return resCallback();
                }
            }
        ).AlsoNotifyFailure();
        public override IDBInvokable<Order> AddProc() => new AddOrderProc(ConnectionString, Collection, CalcModel(null)).AlsoNotifyAdd();
        public override IDBInvokable<Order> UpdateProc(object? id) => new UpdateOrderProc(ConnectionString, Collection, CalcModel(id)).AlsoNotifyUpdate();
        public override IDBInvokable<NoObj> DeleteProc()
        {
            var willDelete = GetModelIDsToDelete();

            return new DeleteOrdersProc(ConnectionString, willDelete)
            .AlsoNotifyDelete()
            .AlsoAssureDeletion(willDelete.Count());
        }

        public override IDBInvokable<IEnumerable<Order>> SearchProc() => new SearchOrdersProc(
            connectionString: ConnectionString,
            collection: Collection,
            productProps: ModelView.OrderProduct is Product productProp
                ? productProp.ReferenceID : ModelView.OrderProduct as string,
            quantity: ModelView.Quantity,
            unitPrice: ModelView.UnitPrice,
            vatRate: ModelView.VatRate,
            orderDate: ModelView.OrderDate,
            updateProductsThen: ModelView.UpdateStock,
            modes: CalcModes()
        ).AlsoNotifyFailure();

        public override OrdersSearchModes CalcModes() => new OrdersSearchModes(
            ModelView.NumQuantity.SearchMode?.ToDBSearchMode(),
            ModelView.NumUnitPrice.SearchMode?.ToDBSearchMode(),
            ModelView.NumVatRate.SearchMode?.ToDBSearchMode(),
            ModelView.DateOrder.SearchMode?.ToDBSearchMode(),
            ModelView.UpdateStock,
            ModelView.ListProduct.SearchInclude == true
        );
    }
}
