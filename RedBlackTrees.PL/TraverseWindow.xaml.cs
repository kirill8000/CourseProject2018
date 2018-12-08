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

namespace RedBlackTrees.PL
{
    /// <summary>
    /// Interaction logic for TraverseWindow.xaml
    /// </summary>
    public partial class TraverseWindow : Window
    {
        public TraverseWindow(IEnumerable<int> values)
        {
            InitializeComponent();
            ListBox.ItemsSource = values;
        }
    }
}
