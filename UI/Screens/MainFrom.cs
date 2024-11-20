using PiecesAutoYoussefApp.Presenters.Impl;
using PiecesAutoYoussefApp.UI.Base;
using SharedAppsUtils.General;
using SharedAppsUtils.StateManagement;
using SharedORMAppsPresenters.Base;
using SharedORMAppsUI.Base;
using System.Configuration;

namespace PiecesAutoYoussefApp.UI.Screens
{
    public partial class MainFrom : Form, IFullView
    {
        private readonly MainState DummyView = new MainState("", Color.Black, new Control(), Color.Black, Color.Black, Color.Black);


        private readonly MainState MainView;
        private readonly MainStaticState ClientsView;
        private readonly MainStaticState SuppliersView;
        private readonly MainStaticState ProductsView;
        private readonly MainStaticState OrdersView;
        private readonly MainStaticState PropertiesView;

        private readonly MainStateHandler StateHandler;

        private static string connectionString = "";

        private KeyPreviewHandler _keyPreviewHandler;
        public MainFrom()
        {
            InitializeComponent();

            MainView = new MainState(LblAppTitle.Text, Color.FromArgb(255, 0, 150, 136), LblViewTitle,
                Color.FromArgb(255, 0, 150, 136), LblViewTitle.ForeColor, LblViewTitle.ForeColor);
            ClientsView = new MainStaticState(BtnShowClients.Text, Color.FromArgb(255, 0, 210, 210), BtnShowClients);
            SuppliersView = new MainStaticState(BtnShowSuppliers.Text, Color.FromArgb(255, 230, 70, 50), BtnShowSuppliers);
            ProductsView = new MainStaticState(BtnShowProducts.Text, Color.FromArgb(255, 180, 30, 70), BtnShowProducts);
            OrdersView = new MainStaticState(BtnShowOrders.Text, Color.FromArgb(255, 0, 175, 225), BtnShowOrders);
            PropertiesView = new MainStaticState(BtnShowRefProps.Text, Color.Gray, BtnShowRefProps);


            StateHandler = new MainStateHandler(DummyView);
            StateHandler.StateChanged += StateHandler_StateChanged;

            try
            {
                //connectionString = ConfigurationManager.ConnectionStrings["DBFileConnectionString"].ConnectionString.Replace("DBFILEPATH", ConstUtils.DB_FILE_PATH);
                connectionString = ConfigurationManager.ConnectionStrings["DBServerConnectionString"].ConnectionString;
            }
            catch (Exception e)
            {

            }

            StateHandler.State = MainView;

            LblAppTitle.Click += (object? sender, EventArgs e) => StateHandler.State = MainView;
            BtnShowClients.Click += (object? sender, EventArgs e) =>
            {
                Control? m = new ClientDataPanel();
                ILifeCycle? p = new ClientPresenter(connectionString, (ClientDataPanel)m, this);
                StateHandler.State = new MainState(ClientsView) { Presenter = p, ModelDataPanel = m };
            };
            BtnShowSuppliers.Click += (object? sender, EventArgs e) =>
            {
                Control? m = new SupplierDataPanel();
                ILifeCycle? p = new SupplierPresenter(connectionString, (SupplierDataPanel)m, this);
                StateHandler.State = new MainState(SuppliersView) { Presenter = p, ModelDataPanel = m };
            };
            BtnShowProducts.Click += (object? sender, EventArgs e) =>
            {
                Control? m = new ProductDataPanel();
                ILifeCycle? p = new ProductPresenter(connectionString, (ProductDataPanel)m, this);
                StateHandler.State = new MainState(ProductsView) { Presenter = p, ModelDataPanel = m };
            };
            BtnShowOrders.Click += (object? sender, EventArgs e) =>
            {
                Control? m = new ClientOrderCollectionDataPanel();
                ILifeCycle? p = new ClientOrderCollectionPresenter(
                    connectionString,
                    (ClientOrderCollectionDataPanel)m,
                    (ClientOrderCollectionDataPanel)m,
                    this
                );
                StateHandler.State = new MainState(OrdersView) { Presenter = p, ModelDataPanel = m };
            };
            BtnShowRefProps.Click += (object? sender, EventArgs e) =>
            {
                Control? m = new AllRefPropsDataPanel();
                ILifeCycle? p = new AllRefPropsPresenter(connectionString, (AllRefPropsDataPanel)m, this);
                StateHandler.State = new MainState(PropertiesView) { Presenter = p, ModelDataPanel = m };
            };



            _keyPreviewHandler = new KeyPreviewHandler(this);
            _keyPreviewHandler.KeyDown += (object? sender, KeyEventArgs e) =>
            {
                Control? control = sender as Control;

                bool inModelDataPanel = false;
                Control? tempControl = control;
                while (tempControl != null)
                {
                    if (tempControl == StateHandler.State.ModelDataPanel)
                    {
                        inModelDataPanel = true;
                        break;
                    }
                    tempControl = tempControl?.Parent;
                }

                if (StateHandler.State != PropertiesView && inModelDataPanel && e.KeyCode == Keys.Enter)
                {
                    SelectNextControl(control, true, true, true, true);
                }
            };

            // keyup: problem not allowing me to repeat(because after save the id is focused).
            // keydown: problem not moving after add.
        }

        private void StateHandler_StateChanged(object? sender, AppStateEventArgs<MainState> e)
        {
            e.Old.Presenter?.Dispose();

            if (PanelModelData.Controls.Count > 0)
            {
                Control? controlToRemove = PanelModelData.Controls[0];
                PanelModelData.Controls.Remove(controlToRemove);
                controlToRemove.Dispose();
            }

            TblAll.Visible = e.Current != MainView;
            PicHome.Visible = false; // TODO : e.Current == MainView;

            // reset prev
            e.Old.Button.BackColor = e.Old.InactiveBackColor;
            e.Old.Button.ForeColor = e.Old.InactiveForeColor;

            // set current
            LblViewTitle.Text = e.Current.Title;
            LblViewTitle.BackColor
                = e.Current.Button.BackColor
                = e.Current.ActiveBackColor;
            e.Current.Button.ForeColor = e.Current.ActiveForeColor;

            if (e.Current.ModelDataPanel != null)
            {
                PanelModelData.Controls.Add(e.Current.ModelDataPanel);
                e.Current.ModelDataPanel.Location = new Point(0, 0);
            }

            e.Current.Presenter?.Start();
            ReassignEnterForButtons();
        }

        // to be the last action taken.
        private void ReassignEnterForButtons()
        {
            BtnAdd.Click -= BtnAdd_Click;
            BtnSave.Click -= BtnSave_Click;

            BtnAdd.Click += BtnAdd_Click;
            BtnSave.Click += BtnSave_Click;
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            if (StateHandler.State != PropertiesView)
            {
                StateHandler.State.ModelDataPanel?.Focus();
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (StateHandler.State != PropertiesView)
            {
                BtnAdd.Focus();
            }
        }

        private void MainFrom_Load(object sender, EventArgs e)
        {
            //GeneralUtils.SetFontForAllControls(this, ConstUtils.INIT_FONT_SCALE);
            //Font font = this.Font;
            //this.Font = new Font(font.FontFamily, font.Size * 0.825F, font.Style, font.Unit);
        }



        Control IViewableView.BtnView => BtnView;
        Control IAddableView.BtnAdd => BtnAdd;
        Control IDeletableView.BtnDelete => BtnDelete;
        Control IUpdatableView.BtnUpdate => BtnUpdate;
        Control ISavable.BtnSave => BtnSave;
        Control ISearchableView.BtnSearch => BtnSearch;
        Control IWithClear.BtnClear => BtnClear;
        Control IWithCountView.CountView => CountLabel;
        Control IWithExport.BtnExportCSV => BtnExportCSV;
        DataGridView IWithGridView.GridView => GridView;
    }

    /* Future
        - allow adding properties through Enter
        - analyze code for all projects
        - imp logs
        - imp localization
        - Separate this app models, UI, and presenter(already separable).
    */

    /* TODO : When changing lang 
        - ErrorHandler.cs
        - InfoHandler
        - CSVParser
        - GRIDViewTableParser
        - All Designer Files.
     */
}