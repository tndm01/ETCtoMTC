using System.Windows;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.ViewModel;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Job;

namespace ITD.ETC.VETC.Synchronization.MtcAndEtc.View
{
    /// <summary>
    /// Interaction logic for SyncMainWindow.xaml
    /// </summary>
    public partial class SyncMainWindow : Window
    {
        private string builVersion = " - Version 1.06 24.03.2017";
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
            //LaneImageETCtoMTC imag = new LaneImageETCtoMTC();
            //imag.Execute(null);
        }

        private void Button_Click_Thang(object sender, RoutedEventArgs e)
        {
            //CommuterTransactionDataETCtoMTC oCommuterTransactionDataETCtoMTC = new CommuterTransactionDataETCtoMTC();
            //oCommuterTransactionDataETCtoMTC.Execute2();

            //TollTicketTransactionETCtoMTC oTollTicketTransactionETCtoMTC = new TollTicketTransactionETCtoMTC();
            //oTollTicketTransactionETCtoMTC.Execute2();

            //TollTicketMTCtoETC oTollTicketMTCtoETC = new TollTicketMTCtoETC();
            //oTollTicketMTCtoETC.Execute12();

            //CommuterTicketMTCtoETC oCommuterTicketMTCtoETC = new CommuterTicketMTCtoETC();
            //oCommuterTicketMTCtoETC.Execute(null);

            //CommuterTransactionDataETCtoMTC oCommuterTransactionDataETCtoMTC = new CommuterTransactionDataETCtoMTC();
            //oCommuterTransactionDataETCtoMTC.Execute2();

            //TollTicketTransactionETCtoMTC oTollTicketTransactionETCtoMTC = new TollTicketTransactionETCtoMTC();
            //oTollTicketTransactionETCtoMTC.Execute2();
            //VehiclePlateDataMTCtoETC test = new VehiclePlateDataMTCtoETC();
            //test.Execute(null);
        }

        //private void btnStart_Click(object sender, RoutedEventArgs e)
        //{
        //    //SpecicalTransactionETCtoMTC sb = new SpecicalTransactionETCtoMTC();
        //    //sb.ExecuteSpecial();
        //}

        //private void btnStart_Click(object sender, RoutedEventArgs e)
        //{
        //    //ETAGTransactionDataETCtoMTC oETAGTransactionDataETCtoMTC = new ETAGTransactionDataETCtoMTC();
        //    //oETAGTransactionDataETCtoMTC.Execute2();
        //}
    }
}
