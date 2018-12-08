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
    /// Interaction logic for BFSWindow.xaml
    /// </summary>
    public partial class BFSWindow : Window
    {
        public BFSWindow(IEnumerable<int> values)
        {
            InitializeComponent();
            ListBox.ItemsSource = values;

        }
    }
}
