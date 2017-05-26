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

namespace ITD.ETC.VETC.Synchronization.MtcAndEtc.View
{
    /// <summary>
    /// Interaction logic for ConfigView.xaml
    /// </summary>
    public partial class ConfigView : Window
    {
        public ConfigView()
        {
            InitializeComponent();
        }

        private void ConfigWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ConfigViewModel vm = new ConfigViewModel();
            this.DataContext = vm;
        }
    }
}
