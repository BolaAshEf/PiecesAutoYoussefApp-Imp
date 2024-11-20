using SharedAppsUtils.StateManagement;
using SharedORMAppsPresenters.Base;

namespace PiecesAutoYoussefApp.UI.Base
{
    public class MainStaticState : IEquatable<MainStaticState?>
    {
        public string Title { get; init; }
        public Color ActiveBackColor { get; init; }
        public Color InactiveBackColor { get; init; }
        public Color ActiveForeColor { get; init; }
        public Color InactiveForeColor { get; init; }
        public Control Button { get; init; }

        public MainStaticState(string title, Color activeBackColor, Control button,
            Color? inactiveBackColor = null, Color? activeForeColor = null, Color? inactiveForeColor = null)
        {
            Title = title;
            ActiveBackColor = activeBackColor;
            Button = button;

            InactiveBackColor = inactiveBackColor ?? Color.Transparent;
            ActiveForeColor = activeForeColor ?? Color.White;
            InactiveForeColor = inactiveForeColor ?? Color.Gainsboro;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as MainStaticState);
        }

        public bool Equals(MainStaticState? other)
        {
            return other is not null &&
                   Title == other.Title &&
                   ActiveBackColor.Equals(other.ActiveBackColor) &&
                   InactiveBackColor.Equals(other.InactiveBackColor) &&
                   ActiveForeColor.Equals(other.ActiveForeColor) &&
                   InactiveForeColor.Equals(other.InactiveForeColor) &&
                   EqualityComparer<Control>.Default.Equals(Button, other.Button);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, ActiveBackColor, InactiveBackColor, ActiveForeColor, InactiveForeColor, Button);
        }

        public static bool operator ==(MainStaticState? left, MainStaticState? right)
        {
            return EqualityComparer<MainStaticState>.Default.Equals(left, right);
        }

        public static bool operator !=(MainStaticState? left, MainStaticState? right)
        {
            return !(left == right);
        }
    }

    public class MainState : MainStaticState
    {
        public ILifeCycle? Presenter { get; init; }
        public Control? ModelDataPanel { get; init; }

        public MainState(
            string title, Color color, Control button, Color? inactiveBackColor = null, Color? activeForeColor = null, Color? inactiveForeColor = null)
            : base(title, color, button, inactiveBackColor, activeForeColor, inactiveForeColor)
        {

        }

        public MainState(MainStaticState staticProps) : base(
            staticProps.Title, staticProps.ActiveBackColor, staticProps.Button,
            staticProps.InactiveBackColor, staticProps.ActiveForeColor, staticProps.InactiveForeColor)
        {

        }
    }

    public class MainStateHandler : AppStateHandler<MainState>
    {
        public MainStateHandler(MainState initialState) : base(initialState)
        {

        }
    }
}
