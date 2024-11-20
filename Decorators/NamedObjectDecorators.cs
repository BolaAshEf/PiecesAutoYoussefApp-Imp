using PiecesAutoYoussefApp.Models;

namespace PiecesAutoYoussefApp.Decorators
{
    public class NamedClientDecorator : Client
    {
        public NamedClientDecorator(Client obj) : base(
            firstName: obj.FirstName,
            lastName: obj.LastName,
            phoneNumber: obj.PhoneNumber,
            clientAddress: obj.ClientAddress,
            clientCategory: obj.ClientCategory)
        {
            Id = obj.Id;
        }

        public override string ToString() => FullName;
    }

    public class NamedClientCategoryDecorator : ClientCategory
    {
        public NamedClientCategoryDecorator(ClientCategory obj) : base(
            categoryName: obj.CategoryName)
        {
            Id = obj.Id;
        }

        public override string ToString() => CategoryName;
    }

    public class NamedModelDecorator : Model
    {
        public NamedModelDecorator(Model obj) : base(
            modelName: obj.ModelName)
        {
            Id = obj.Id;
        }

        public override string ToString() => ModelName;
    }

    public class NamedOrderDecorator : Order
    {
        public NamedOrderDecorator(Order obj) : base(
            product: obj.Product,
            quantity: obj.Quantity,
            unitPrice: obj.UnitPrice,
            vatRate: obj.VatRate,
            orderDate: obj.OrderDate,
            updateProductsThen: obj.UpdateProductsThen)
        {
            Id = obj.Id;
        }

        public override string ToString() => $"{Product} * {Quantity} = {TotalPrice}";
    }

    public class NamedOrderCollectionDecorator : OrderCollection
    {
        public NamedOrderCollectionDecorator(OrderCollection obj) : base(
            client: obj.Client)
        {
            Id = obj.Id;
        }

        public override string ToString() => $"For -> {Client}";
    }

    public class NamedPieceDecorator : Piece
    {
        public NamedPieceDecorator(Piece obj) : base(
            pieceName: obj.PieceName)
        {
            Id = obj.Id;
        }

        public override string ToString() => PieceName;
    }

    public class NamedProductDecorator : Product
    {
        public NamedProductDecorator(Product obj) : base(
            referenceID: obj.ReferenceID,
            model: obj.Model,
            piece: obj.Piece,
            stock: obj.Stock,
            unitPrice: obj.UnitPrice,
            additionDate: obj.AdditionDate)
        {
            Id = obj.Id;
        }

        public override string ToString() => $"{ReferenceID}({Model.ModelName}, {Piece.PieceName})";
    }

    public class NamedSupplierDecorator : Supplier
    {
        public NamedSupplierDecorator(Supplier obj) : base(
            firstName: obj.FirstName,
            lastName: obj.LastName,
            phoneNumber: obj.PhoneNumber,
            supplierAddress: obj.SupplierAddress,
            product: obj.Product,
            productQuantity: obj.ProductQuantity)
        {
            Id = obj.Id;
        }

        public override string ToString() => FullName;
    }
}
