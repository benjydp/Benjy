using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
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
            ToDoItems.CollectionChanged += ToDoItems_CollectionChanged;
            lvToDoList.ItemsSource = ToDoItems;
            foreach (var item in ReadListFromDisk())
                ToDoItems.Add(item);
        }

        private void ToDoItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems != null)
            {
                foreach(var item in e.NewItems)
                {
                    if(item is ToDoItem toDoItem)
                    {
                        toDoItem.PropertyChanged += ToDoItem_PropertyChanged;
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    if (item is ToDoItem toDoItem)
                    {
                        toDoItem.PropertyChanged -= ToDoItem_PropertyChanged;
                    }
                }
            }

            WriteListToDisk(ToDoItems.ToList());
        }

        private void ToDoItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            WriteListToDisk(ToDoItems.ToList());
        }

        public ObservableCollection<ToDoItem> ToDoItems { get; } = new();


        private static readonly string diskFileDirectory = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\Benjy.CheckYoShit";
        private static readonly string diskFilePath = $@"{diskFileDirectory}\data.json";

        public static List<ToDoItem> ReadListFromDisk()
        {
            if (File.Exists(diskFilePath)) //Checks if the file exists
            {
                string jsonText = File.ReadAllText(diskFilePath);
                List<ToDoItem> toDoItems = JsonSerializer.Deserialize<List<ToDoItem>>(jsonText);
                return toDoItems;
            }
            
            return new();
        }

        public static void WriteListToDisk(List<ToDoItem> list)
        {
            if (!Directory.Exists(diskFileDirectory))
            {
                Directory.CreateDirectory(diskFileDirectory);
            }
            string jsonText = JsonSerializer.Serialize(list);

            File.WriteAllText(diskFilePath, jsonText);
        }

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