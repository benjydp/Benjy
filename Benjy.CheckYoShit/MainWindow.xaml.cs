using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Benjy.CheckYoShit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            lvToDoList.ItemsSource = ToDoItems;
        }
        public ObservableCollection<ToDoItem> ToDoItems { get; } = new();

        private void bAddItem_Click(object sender, RoutedEventArgs e)
        {
            AddToList(); 
        }

        private void AddToList()
        {
            if (!string.IsNullOrEmpty(tbDescription.Text))
            {
                var item = new ToDoItem()
                {
                    Description = tbDescription.Text
                };

                ToDoItems.Add(item);
                tbDescription.Text = string.Empty;
            }
        }

        private void lvToDoList_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is ToDoItem item)
            {
                if(e.Key == Key.Delete)
                {
                    ToDoItems.Remove(item);
                }
            }
        }

        private void tbDescription_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(sender is TextBox tbDescription)
            {
                if(e.Key == Key.Enter)
                {
                    AddToList();
                    e.Handled = true;
                }
            }
        }
    }
}