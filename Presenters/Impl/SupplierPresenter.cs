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
using System.Data;
using static PiecesAutoYoussefApp.Extensions.PresentersExtensions;

namespace PiecesAutoYoussefApp.Presenters.Impl
{
    public class SupplierPresenter : BaseFullPresenter<Supplier>
    {
        new private ISuppliersView ModelView => (ISuppliersView)base.ModelView;

        public SupplierPresenter(string connectionString, ISuppliersView modelView, IFullView fullView) :
            base(connectionString, modelView, fullView)
        {

        }


        public override DataTable FromModels(IEnumerable<Supplier> models) =>
            GridViewTableParser.FromSuppliers(models);

        public override SuppliersSearchModes CalcModes() => new SuppliersSearchModes(
            ModelView.NumProductQuantity.SearchMode?.ToDBSearchMode(),
            ModelView.ListProduct.SearchInclude == true
        );


        protected IDBInvokable<IEnumerable<Product>> LoadOrderProductsProc() => new GetProductsProc(ConnectionString);

        protected override IDBInvokable<IEnumerable<Supplier>> LoadModelsFromDB() => new DBInvokableResHandlerDecorator<IEnumerable<Supplier>>(
            new GetSuppliersProc(ConnectionString),
            resCallback =>
            {
                DBResponse productsRes = LoadOrderProductsProc().Invoke(out var obj);
                if (obj == null)
                {
                    return productsRes;
                }
                else
                {
                    ModelView.SupplierProducts = obj.ToNamedDecorator().ToArray();

                    return resCallback();
                }
            }
        ).AlsoNotifyFailure();
        public override IDBInvokable<Supplier> AddProc() => new AddSupplierProc(ConnectionString, CalcModel(null)).AlsoNotifyAdd();
        public override IDBInvokable<Supplier> UpdateProc(object? id) => new UpdateSupplierProc(ConnectionString, CalcModel(id)).AlsoNotifyUpdate();
        public override IDBInvokable<IEnumerable<Supplier>> SearchProc() => new SearchSuppliersProc(
            connectionString: ConnectionString,
            firstName: ModelView.FirstName,
            lastName: ModelView.LastName,
            supplierAddress: ModelView.Address,
            phoneNumber: ModelView.PhoneNumber,
            productProp: ModelView.SupplierProduct is Product productProp
                ? productProp.ReferenceID : ModelView.SupplierProduct as string,
            productQuantity: ModelView.ProductQuantity,
            modes: CalcModes()
        ).AlsoNotifyFailure();
        public override IDBInvokable<NoObj> DeleteProc()
        {
            var willDelete = GetModelIDsToDelete();

            return new DeleteSuppliersProc(ConnectionString, willDelete)
            .AlsoNotifyDelete()
            .AlsoAssureDeletion(willDelete.Count());
        }


        protected override void PopulateView(Supplier? model)
        {
            ModelView.FirstName = model?.FirstName ?? "";
            ModelView.LastName = model?.LastName ?? "";
            ModelView.Address = model?.SupplierAddress ?? "";
            ModelView.PhoneNumber = model?.PhoneNumber ?? "";
            ModelView.SupplierProduct = model?.Product.ToNamedDecorator();
            ModelView.ProductQuantity = model?.ProductQuantity;

            if (model == null)
            {
                ModelView.ListProduct.SetToSearchExcludeIfInSearch();
            }
        }

        protected override Supplier CalcModel(object? id) => new Supplier(
            firstName: ModelView.FirstName,
            lastName: ModelView.LastName,
            supplierAddress: ModelView.Address,
            phoneNumber: ModelView.PhoneNumber,
            product: (ModelView.SupplierProduct as Product
                ?? throw ConstraintKey.ProductReferenceIDNotEmpty.NotifyErr()).ToNamedDecorator(),
            productQuantity: ModelView.ProductQuantity
                ?? throw ConstraintKey.SupplierProductQuantityNonNegative.NotifyErr()
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
                = ModelView.NumProductQuantity.ReadOnly
                = State.IsTxtReadOnly();

            ModelView.NumProductQuantity.SearchMode = State.SearchMode()?.ToButtonSearchMode();

            ModelView.ListProduct.Mode = State.RefComboNoEditMode();
        }
    }
}
