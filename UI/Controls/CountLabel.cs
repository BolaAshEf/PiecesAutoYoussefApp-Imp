using System.ComponentModel;

namespace PiecesAutoYoussefApp.UI.Controls
{
    public partial class CountLabel : UserControl
    {
        public CountLabel()
        {
            InitializeComponent();
        }

        [Category("Custom")]
        [Browsable(true)]
        public override string Text
        {
            get => LblCount.Text;
            set => LblCount.Text = value;
        }
    }
}
