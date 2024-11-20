using SharedORMAppsModels.DB.Impl.Sql.Search;
using SharedORMAppsUI.Controls;
using SharedORMAppsUI.States;

namespace PiecesAutoYoussefApp.Extensions
{
    public static class ScreenStateExtensions
    {
        public static DBSearchMode? SearchMode(this ScreenState screenState)
        {
            return screenState switch
            {
                ViewScreenState => null,
                AddScreenState => null,
                UpdateScreenState => null,
                SearchScreenState => DBSearchMode.NotCare,
                _ => throw new NotImplementedException()
            };
        }

        public static bool IsPopulatingGridView(this ScreenState screenState)
        {
            return screenState switch
            {
                ViewScreenState => true,
                AddScreenState => false,
                UpdateScreenState => false,
                SearchScreenState => false,
                _ => throw new NotImplementedException()
            };
        }

        public static bool IsCountVisible(this ScreenState screenState)
        {
            return screenState switch
            {
                ViewScreenState => true,
                AddScreenState => false,
                UpdateScreenState => false,
                SearchScreenState => false,
                _ => throw new NotImplementedException()
            };
        }

        public static bool IsTxtReadOnly(this ScreenState screenState)
        {
            return screenState switch
            {
                ViewScreenState => true,
                AddScreenState => false,
                UpdateScreenState => false,
                SearchScreenState => false,
                _ => throw new NotImplementedException()
            };
        }

        public static EditableRefPropCombo.ComboMode RefComboMode(this ScreenState screenState)
        {
            return screenState switch
            {
                ViewScreenState => EditableRefPropCombo.ComboMode.ReadOnly,
                AddScreenState => EditableRefPropCombo.ComboMode.EditList,
                UpdateScreenState => EditableRefPropCombo.ComboMode.EditList,
                SearchScreenState => EditableRefPropCombo.ComboMode.SearchListExclude,
                _ => throw new NotImplementedException()
            };
        }

        public static EditableRefPropCombo.ComboMode RefComboNoEditMode(this ScreenState screenState)
        {
            return screenState switch
            {
                ViewScreenState => EditableRefPropCombo.ComboMode.ReadOnly,
                AddScreenState => EditableRefPropCombo.ComboMode.ChooseFromList,
                UpdateScreenState => EditableRefPropCombo.ComboMode.ChooseFromList,
                SearchScreenState => EditableRefPropCombo.ComboMode.SearchListExclude,
                _ => throw new NotImplementedException()
            };
        }

        public static bool IsRecomenddedIDEnable(this ScreenState screenState)
        {
            return screenState switch
            {
                ViewScreenState => true,
                AddScreenState => false,
                UpdateScreenState => false,
                SearchScreenState => false,
                _ => throw new NotImplementedException()
            };
        }
    }
}
