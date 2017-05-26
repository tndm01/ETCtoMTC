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
using System.Windows.Shapes;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.ViewModel;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Job;
using ITD.ETC.VETC.Synchonization.Controller.ETC;

namespace ITD.ETC.VETC.Synchronization.MtcAndEtc.View
{
    /// <summary>
    /// Interaction logic for SyncMainWindow.xaml
    /// </summary>
    public partial class SyncMainWindow : Window
    {
        private string builVersion = " - Version 1.01 17.03.2017";
        public SyncMainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title += builVersion;

            SyncMainViewModel viewModel = new SyncMainViewModel();
            this.DataContext = viewModel;
        }

        private void Button_Click_Viet(object sender, RoutedEventArgs e)
        {
            //EmployeeDataMTCtoETC test = new EmployeeDataMTCtoETC();
            //test.Execute(null);

        }

        private void Button_Click_Thang(object sender, RoutedEventArgs e)
        {
            //CommuterTransactionDataETCtoMTC oCommuterTransactionDataETCtoMTC = new CommuterTransactionDataETCtoMTC();
            //oCommuterTransactionDataETCtoMTC.Execute2();

            //TollTicketTransactionETCtoMTC oTollTicketTransactionETCtoMTC = new TollTicketTransactionETCtoMTC();
            //oTollTicketTransactionETCtoMTC.Execute2();

            //TollTicketMTCtoETC oTollTicketMTCtoETC = new TollTicketMTCtoETC();
            //oTollTicketMTCtoETC.Execute12();

            CommuterTicketMTCtoETC oCommuterTicketMTCtoETC = new CommuterTicketMTCtoETC();
            oCommuterTicketMTCtoETC.Execute(null);

            CommuterTransactionDataETCtoMTC oCommuterTransactionDataETCtoMTC = new CommuterTransactionDataETCtoMTC();
            oCommuterTransactionDataETCtoMTC.Execute2();

            TollTicketTransactionETCtoMTC oTollTicketTransactionETCtoMTC = new TollTicketTransactionETCtoMTC();
            oTollTicketTransactionETCtoMTC.Execute2();
            VehiclePlateDataMTCtoETC test = new VehiclePlateDataMTCtoETC();
            test.Execute(null);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            ETAGTransactionDataETCtoMTC oETAGTransactionDataETCtoMTC = new ETAGTransactionDataETCtoMTC();
            oETAGTransactionDataETCtoMTC.Execute2();
        }
    }
}
