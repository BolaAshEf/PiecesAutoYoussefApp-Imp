using PiecesAutoYoussefApp.UI.Base;
using PiecesAutoYoussefApp.UI.Screens;

namespace PiecesAutoYoussefApp.UI.Impl
{
    public interface IRefPropView : IAppModelView
    {
        string? PropName
        {
            get => ListID.CurrentItem?.ToString();
            set => ListID.CurrentItem = value;
        }
    }

    public interface IAllRefPropsView
    {
        public ClientCategoryDataPanel ClientCategoryDataPanel { get; }
        public ModelDataPanel ModelDataPanel { get; }
        public PieceDataPanel PieceDataPanel { get; }
    }
}