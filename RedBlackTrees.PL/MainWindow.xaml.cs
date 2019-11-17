using System;
using System.Diagnostics;
using System.Windows;
using Algorithms;

namespace RedBlackTrees.PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Map<int, string> _map = new Map<int, string>();

        public MainWindow()
        {
            InitializeComponent();
            Closing += (sender, args) =>
            {
                var r = MessageBox.Show("Вы действительно хотите выйти?", "Предупреждение", MessageBoxButton.YesNo);
                if (r != MessageBoxResult.Yes)
                {
                    args.Cancel = true;
                }
            };
            for (int i = 0; i < 10; i++)
            {
                _map.Add(i, "");
            }
            TreeView.ItemsSource = _map.Root;
            TraverseButton.Click += (sender, args) =>
            {
                if (_map.Size > 0)
                    new TraverseWindow(_map.Keys).Show();
                else
                    MessageBox.Show("Дерево пустое", "Warning!", MessageBoxButton.OK);
            };
                
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(ValueBox.Text, out int val))
            {
                _map.Add(val, "");
                TreeView.ItemsSource = _map.Root;
                DeleteMinimumButton.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Необходимо ввести число!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ValueBox.Clear();
        }

        private void DeleteMinimumButton_Click(object sender, RoutedEventArgs e)
        {
            if (_map.IsEmpty || _map.Size == 1)
            {
                DeleteMinimumButton.IsEnabled = false;
            }

            _map.DeleteMin();
            TreeView.ItemsSource = _map.Root;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Курсовой проект \"Бинарные деревья поиска\". Вариант 5.", "О программе",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BFSButton_Click(object sender, RoutedEventArgs e)
        {
            if (_map.Size > 0)
                new TraverseWindow(_map.GetBreadthFirstSearchEnumerator()).Show();
            else
                MessageBox.Show("Дерево пустое", "Warning!", MessageBoxButton.OK);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _map = new Map<int, string>();
            TreeView.ItemsSource = _map.Root;
        }

        private void Help(object sender, RoutedEventArgs e)
        {
            var commandText = @"Help.chm";
            var proc = new Process();
            proc.StartInfo.FileName = commandText;
            proc.StartInfo.UseShellExecute = true;
            proc.Start();
        }
    }
}
