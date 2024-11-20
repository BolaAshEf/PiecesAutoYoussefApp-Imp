using PiecesAutoYoussefApp.UI.Base;
using PiecesAutoYoussefApp.UI.Screens;
using SharedORMAppsUI.Controls;
using System.Data;

namespace PiecesAutoYoussefApp.UI.Impl
{
    public interface IClientOrderCollectionsView : IAppModelView
    {
        object[] CollectionClients
        {
            get => ((ComboBox)ListCollectionClient).Items.Cast<object>().ToArray();
            set
            {
                var items = ((ComboBox)ListCollectionClient).Items;
                if (items.Cast<object>().SequenceEqual(value))
                {
                    return;
                }

                items.Clear();
                items.AddRange(value);
            }
        }
        object? CollectionClient { get => ListCollectionClient.CurrentItem; set => ListCollectionClient.CurrentItem = value; }

        EditableRefPropCombo ListCollectionClient { get; }

        OrderDataPanel OrderDataPanel { get; }
    }
}