using System.Collections.Generic;
using System.Windows;

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
