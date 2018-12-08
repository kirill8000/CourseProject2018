using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using Algorithms;

namespace RedBlackTrees.PL
{
    public class Node
    {
        public string Name { get; set; }
        public ObservableCollection<Node> Nodes { get; set; }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Map<int, string> _map = new Map<int, string>();
        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < 10; i++)
            {
                _map.Add(i, i.ToString());
            }
            treeView.ItemsSource = _map.Root;
            BFSButton.Click += (sender, args) => { new TraverseWindow(_map.GetBreadthFirstSearchEnumerator()).Show(); };
            TraverseButton.Click += (sender, args) =>
            {
                new TraverseWindow(_map.Keys).Show();
            };
            ClearButton.Click += (sender, args) =>
            {
                _map = new Map<int, string>();
                treeView.ItemsSource = _map.Root;
            };
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(valueBox.Text, out int val))
            {
                _map.Add(val, "");
                treeView.ItemsSource = _map.Root;
                DeleteMinimumButton.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Введите число!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            valueBox.Clear();
        }

        private void DeleteMinimumButton_Click(object sender, RoutedEventArgs e)
        {
            if (_map.IsEmpty || _map.Size == 1)
            {
                DeleteMinimumButton.IsEnabled = false;
            }
            _map.DeleteMin();
            treeView.ItemsSource = _map.Root;
        }
    }
}
