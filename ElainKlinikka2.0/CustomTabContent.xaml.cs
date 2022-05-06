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

namespace ElainKlinikka2._0
{
    /// <summary>
    /// Interaction logic for CustomTabContent.xaml
    /// </summary>
    public partial class CustomTabContent : UserControl
    {
        public CustomTabContent()
        {
            InitializeComponent();
        }

        private TabControl tabControl1;
        public System.Windows.TabPage SelectedTab { get; set; }
        private void BTN_Sulje_Click(object sender, RoutedEventArgs e)
        {
            
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
        }

    }
}
