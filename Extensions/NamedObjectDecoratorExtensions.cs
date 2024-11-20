using PiecesAutoYoussefApp.Decorators;
using PiecesAutoYoussefApp.Models;

namespace PiecesAutoYoussefApp.Extensions
{
    public static class NamedObjectDecoratorExtensions
    {
        public static NamedClientDecorator ToNamedDecorator(this Client client) =>
            new NamedClientDecorator(client);

        public static NamedClientCategoryDecorator ToNamedDecorator(this ClientCategory clientCategory) =>
            new NamedClientCategoryDecorator(clientCategory);

        public static NamedModelDecorator ToNamedDecorator(this Model model) =>
            new NamedModelDecorator(model);

        public static NamedOrderDecorator ToNamedDecorator(this Order order) =>
            new NamedOrderDecorator(order);

        public static NamedOrderCollectionDecorator ToNamedDecorator(this OrderCollection orderCollection) =>
            new NamedOrderCollectionDecorator(orderCollection);

        public static NamedPieceDecorator ToNamedDecorator(this Piece piece) =>
            new NamedPieceDecorator(piece);

        public static NamedProductDecorator ToNamedDecorator(this Product product) =>
            new NamedProductDecorator(product);

        public static NamedSupplierDecorator ToNamedDecorator(this Supplier supplier) =>
            new NamedSupplierDecorator(supplier);





        public static IEnumerable<NamedClientDecorator> ToNamedDecorator(this IEnumerable<Client> clients) =>
        clients.Select(client => new NamedClientDecorator(client));

        public static IEnumerable<NamedClientCategoryDecorator> ToNamedDecorator(this IEnumerable<ClientCategory> clientCategories) =>
            clientCategories.Select(clientCategory => new NamedClientCategoryDecorator(clientCategory));

        public static IEnumerable<NamedModelDecorator> ToNamedDecorator(this IEnumerable<Model> models) =>
            models.Select(model => new NamedModelDecorator(model));

        public static IEnumerable<NamedOrderDecorator> ToNamedDecorator(this IEnumerable<Order> orders) =>
            orders.Select(order => new NamedOrderDecorator(order));

        public static IEnumerable<NamedOrderCollectionDecorator> ToNamedDecorator(this IEnumerable<OrderCollection> orderCollections) =>
            orderCollections.Select(orderCollection => new NamedOrderCollectionDecorator(orderCollection));

        public static IEnumerable<NamedPieceDecorator> ToNamedDecorator(this IEnumerable<Piece> pieces) =>
            pieces.Select(piece => new NamedPieceDecorator(piece));

        public static IEnumerable<NamedProductDecorator> ToNamedDecorator(this IEnumerable<Product> products) =>
            products.Select(product => new NamedProductDecorator(product));

        public static IEnumerable<NamedSupplierDecorator> ToNamedDecorator(this IEnumerable<Supplier> suppliers) =>
            suppliers.Select(supplier => new NamedSupplierDecorator(supplier));
    }
}
