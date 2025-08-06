using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace FinalProject
{
    public partial class MainWindow : Window
    {
        static MainWindow instance;

        public MainWindow()
        {
            SetInstance();
            IntializeDBItems();
            InitializeComponent();
            LoadLogIn();
        }

        void LoadLogIn()
        {
            PageLoader.LogInScreen();
        }

        void SetInstance()
        {
            instance = this;
        }

        public static MainWindow GetInstance()
        {
            return instance;
        }

        void IntializeDBItems()
        {
            DatabaseManager.CheckIfDueDatesUpcoming();
            DatabaseManager.InitializeIDCount();
        }

    }
}
