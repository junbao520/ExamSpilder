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
using WpfApp1.ViewModel.Common;

namespace WpfApp1
{
    /// <summary>
    /// ExamWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ExamWindow : Window
    {
        public ExamWindow()
        {
            InitializeComponent();
            this.Register<EnterScoreWindow>("EnterScoreWindow");
        }
        private void content_key(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnQuery_Click(null, null);
            }
        }
        private void BtnQuery_Click(object sender, RoutedEventArgs e)
        {

            // UpdateListView<MailList>(listview, result);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
        }
    }
}
