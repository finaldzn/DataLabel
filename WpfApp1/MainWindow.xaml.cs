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
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;

namespace Datalabel
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    /// 
    public class ComboData
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
    
    public partial class MainWindow : Window
    {
        string[] fileentries;
        string targetdirectory;
        int current;
        string pathcsv;
        string result;
        
        public MainWindow()
        {
            InitializeComponent();
            targetdirectory = "";
            string json = File.ReadAllText("config.json");
            Dictionary<string, List<string>> values = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);            
            List<ComboData> comboDatas = new List<ComboData>();
            int id = 1;
            foreach(string elem in values["cast"])
            {
                comboDatas.Add(new ComboData { Id = id, Value = elem });
                id++;
            }
            type.ItemsSource = comboDatas;
            type.DisplayMemberPath = "Value";
            type.SelectedValuePath = "Id";
            type.SelectedIndex = 1;
            List<ComboData> comboDatas2 = new List<ComboData>();
            id = 1;
            foreach (string elem in values["health"])
            {
                comboDatas2.Add(new ComboData { Id = id, Value = elem });
                id++;
            }
            health.ItemsSource = comboDatas2;
            health.DisplayMemberPath = "Value";
            health.SelectedValuePath = "Id";
            health.SelectedIndex = 1;
            List<ComboData> comboDatas3 = new List<ComboData>();
            id = 1;
            foreach (string elem in values["species"])
            {
                comboDatas3.Add(new ComboData { Id = id, Value = elem });
                id++;
            }
            species.ItemsSource = comboDatas3;
            species.DisplayMemberPath = "Value";
            species.SelectedValuePath = "Id";
            species.SelectedIndex = 1;
        }
        private void ExploreFolder()
        {
            fileentries = Directory.GetFiles(targetdirectory);
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if(current != fileentries.Length)
            {
                BitmapImage Content = new BitmapImage();
                Content.BeginInit();
                Content.UriSource = new Uri(fileentries[current], UriKind.RelativeOrAbsolute);
                Content.DecodePixelWidth = 241;
                Content.EndInit();
                content.Source = Content;
                if(current !=0)
                {
                    ComboData cbi = (ComboData)type.SelectedItem;
                    ComboData cbi2 = (ComboData)health.SelectedItem;
                    ComboData cbi3 = (ComboData)species.SelectedItem;
                    string str1 = cbi.Value;
                    string str2 = cbi2.Value;
                    string str3 = cbi3.Value;
                    string[] temp = fileentries[current].Split('\\');
                    result = temp[temp.Length-1] + ";" + str1+ ";" + str2 + ";" + str3+"\n";
                    File.AppendAllText(pathcsv, result);
                }
                
                current++;
            }
        }
        

        private void Choose_Click(object sender, RoutedEventArgs e)
        {
           
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                MessageBox.Show("You selected: " + dialog.FileName);
                targetdirectory = dialog.FileName;
                CurrentFold.Text = dialog.FileName;
                ExploreFolder();
                pathcsv = dialog.FileName + "_dataset.csv";
            }
        }

        private void Recognize_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "PNG(*.png)|*.png";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                BitmapImage Content = new BitmapImage();
                Content.BeginInit();
                Content.UriSource = new Uri(filename, UriKind.RelativeOrAbsolute);
                Content.DecodePixelWidth = 241;
                Content.EndInit();
                reconnaitre.Source = Content;
            }
            
        }
    }
}
