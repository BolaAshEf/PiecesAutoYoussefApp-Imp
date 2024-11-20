using PiecesAutoYoussefApp.UI.Impl;
using SharedORMAppsUI.Base;

namespace PiecesAutoYoussefApp.UI.Screens
{
    public partial class AllRefPropsDataPanel : AutoHeightUserControl, IAllRefPropsView
    {
        public AllRefPropsDataPanel()
        {
            InitializeComponent();
        }

        ClientCategoryDataPanel IAllRefPropsView.ClientCategoryDataPanel => ClientCategoryDataPanel;
        ModelDataPanel IAllRefPropsView.ModelDataPanel => ModelDataPanel;
        PieceDataPanel IAllRefPropsView.PieceDataPanel => PieceDataPanel;
    }
}
