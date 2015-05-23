using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace LogFileManipulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private string[] inputFileNames;
        private OpenFileDialog inputOpenFileDialog;
        private string inputPreviewString = "";
        private string outputPreviewString = "";

        public MainWindow()
        {
            InitializeComponent();
            Add_RegexColumn("", "");
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://regexlib.com/CheatSheet.aspx");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            //fileDialog.DefaultExt = "*.*";
            //fileDialog.Filter = "All Documents|*.*";

            //Nullable<bool> result = fileDialog.ShowDialog();

            //if (result == true)
            //{
            //    System.Diagnostics.Debug.WriteLine(fileDialog.FileName);
            //}
            MessageBox.Show("To Do");
        }

        private static ColumnDefinition Create_Column_Definition(int length, GridUnitType gridType)
        {
            ColumnDefinition columnDefinition = new ColumnDefinition();
            columnDefinition.Width = new GridLength(length, gridType);
            return columnDefinition;
        }

        private static Label Create_Regex_Label(int gridRowNumber)
        { 
            Label label = new Label();
            label.IsEnabled = false;
            label.Content = "/";
            label.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            Grid.SetColumn(label, gridRowNumber);
            return label;
        }

        private void Add_RegexColumn(string s1, string s2)
        {
            // Grid
            Grid gridRow = new Grid();
            gridRow.ColumnDefinitions.Add(Create_Column_Definition(1, GridUnitType.Auto));
            gridRow.ColumnDefinitions.Add(Create_Column_Definition(1, GridUnitType.Auto));
            gridRow.ColumnDefinitions.Add(Create_Column_Definition(1, GridUnitType.Star));
            gridRow.ColumnDefinitions.Add(Create_Column_Definition(1, GridUnitType.Auto));
            gridRow.ColumnDefinitions.Add(Create_Column_Definition(0, GridUnitType.Star));
            gridRow.ColumnDefinitions.Add(Create_Column_Definition(0, GridUnitType.Star));
            gridRow.ColumnDefinitions.Add(Create_Column_Definition(0, GridUnitType.Star));
            gridRow.ColumnDefinitions.Add(Create_Column_Definition(1, GridUnitType.Auto));

            DockPanel.SetDock(gridRow, Dock.Right);

            ComboBox comboBox = new ComboBox();
            comboBox.Margin = new Thickness(5);
            comboBox.MinWidth = 100;
            comboBox.IsEditable = false;
            comboBox.Items.Add("Line Contains");
            comboBox.Items.Add("Find/Replace");
            //comboBox.Items.Add("Reformat");
            comboBox.SelectedIndex = 0;
            comboBox.DropDownClosed += ComboBox_DropDownClosed;
            Grid.SetColumn(comboBox, 0);
            gridRow.Children.Add(comboBox);

            Label l = Create_Regex_Label(1);
            gridRow.Children.Add(l);

            TextBox t = new TextBox();
            t.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            t.Margin = new Thickness(0, 5, 0, 5);
            t.Text = s1;
            t.LostFocus += RegexBox_LostFocus;
            Grid.SetColumn(t, 2);
            gridRow.Children.Add(t);

            l = Create_Regex_Label(3);
            //Grid.SetColumn(l, 3);
            gridRow.Children.Add(l);

            l = Create_Regex_Label(4);
            gridRow.Children.Add(l);

            t = new TextBox();
            t.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            t.Margin = new Thickness(0, 5, 0, 5);
            t.Text = s2;
            t.LostFocus += RegexBox_LostFocus;
            t.Tag = "second";
            Grid.SetColumn(t, 5);
            gridRow.Children.Add(t);

            l = Create_Regex_Label(6);
            gridRow.Children.Add(l);

            Button b = new Button();
            b.Margin = new Thickness(5);
            b.Content = "X";
            b.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            b.Click += b_Click;
            Grid.SetColumn(b, 7);
            gridRow.Children.Add(b);

            try
            {
                OperationsStack.Children.Add(gridRow);
            }
            catch
            {
                MessageBox.Show("We Tried. We Failed. =(");
            }
        }

        void b_Click(object sender, RoutedEventArgs e)
        {
            Grid grid = ((sender as Button).Parent as Grid);
            (grid.Parent as StackPanel).Children.Remove(grid);

            outputPreviewString = UpdateOutput(inputPreviewString);
            OutputFilePreview.Text = outputPreviewString;
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            Grid grid = VisualTreeHelper.GetParent(sender as ComboBox) as Grid;
            if ((sender as ComboBox).Text == "Line Contains")
            {
                grid.ColumnDefinitions[4].Width = new GridLength(0, GridUnitType.Star);
                grid.ColumnDefinitions[5].Width = new GridLength(0, GridUnitType.Star);
                grid.ColumnDefinitions[6].Width = new GridLength(0, GridUnitType.Star);
            }
            else
            {
                grid.ColumnDefinitions[4].Width = new GridLength(1, GridUnitType.Auto);
                grid.ColumnDefinitions[5].Width = new GridLength(1, GridUnitType.Star);
                grid.ColumnDefinitions[6].Width = new GridLength(1, GridUnitType.Auto);
            }
            outputPreviewString = UpdateOutput(inputPreviewString);
            OutputFilePreview.Text = outputPreviewString;
        }

        private void GridSplitter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Left_Column.Width = new GridLength(1, GridUnitType.Star);
            Right_Column.Width = new GridLength(1, GridUnitType.Star);
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                Add_RegexColumn("", "");

            }

        }

        private void InputFileSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Filter = "All files (*.*)|*.*|Text Files (*.txt)|*.txt|Log Files (*.log)|*.log";
            fileDialog.DefaultExt = "*.*";
            Nullable<bool> result = fileDialog.ShowDialog();

            if (result == true)
            {
                // Write to the debug console
                System.Diagnostics.Debug.WriteLine(fileDialog.FileName);
                System.Diagnostics.Debug.WriteLine(fileDialog.FileNames);
                inputOpenFileDialog = fileDialog;

                // Modify InputPreviewFileName TextBlock 
                // Includes the first input file (previewed file)
                // and total number of files if applicable
                if (fileDialog.FileNames.Count<string>() > 1)
                {
                    Save_All_Button.IsEnabled = true;
                    Save_All_Button.ClearValue(Button.ToolTipProperty);
                    InputPreviewFileName.Text = fileDialog.SafeFileName
                                              + " ["
                                              + fileDialog.FileNames.Count<string>().ToString()
                                              + "]";
                    System.Diagnostics.Debug.WriteLine(InputPreviewFileName);
                }
                else
                {
                    InputPreviewFileName.Text = fileDialog.SafeFileName;
                    Save_All_Button.IsEnabled = false;
                }

                // Importing first file to string
                try
                {
                    inputPreviewString = File.ReadAllText(fileDialog.FileName);
                }
                catch
                {
                    MessageBox.Show("Could not read text from: " + fileDialog.SafeFileName);
                    inputPreviewString = "";
                }
                finally
                {
                    InputFilePreview.Text = inputPreviewString;
                }
                
                
            }

            // If Open File Dialog Failed (eg. Cancel Button) 
            else
            {
                inputPreviewString = "";
            }

            string temp = UpdateOutput(inputPreviewString);
            if (temp != outputPreviewString)
            {
                OutputFilePreview.Text = temp;
                outputPreviewString = temp;
            }
        }

        private void RegexBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string temp = UpdateOutput(inputPreviewString);
            if (temp != outputPreviewString)
            {
                OutputFilePreview.Text = temp;
                outputPreviewString = temp;
            }

        }


        private string UpdateOutput(string buffer)
        {
            foreach (Grid row in OperationsStack.Children)
            {
                string operation = (row.Children[0] as ComboBox).Text;
                string a = (row.Children[2] as TextBox).Text;
                string b = (row.Children[5] as TextBox).Text;

                try
                {
                    if (a != "")
                    {
                        if (operation == "Line Contains")
                        {
                            buffer = RegexTasks.Match(buffer, a);
                        }
                        else if (operation == "Find/Replace")
                        {
                            buffer = RegexTasks.FindAndReplace(buffer, a, b);
                        }
                        else if (operation == "Reformat")
                        {
                            buffer = RegexTasks.Reformat(buffer, a, b);
                        }
                    }
                    (row.Children[2] as TextBox).Background = Brushes.White;
                    (row.Children[2] as TextBox).ClearValue(TextBox.ToolTipProperty);
                }
                catch (System.ArgumentException e)
                {   
                    ToolTip t = new ToolTip();
                    t.Content = e.Message;
                    ToolTipService.SetShowDuration(t, 10000);

                    (row.Children[2] as TextBox).ToolTip = t;
                    (row.Children[2] as TextBox).Background = Brushes.OrangeRed;
                }
            }
            return buffer;
        }

        private void RegexBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string temp = UpdateOutput(inputPreviewString);
                if (temp != outputPreviewString)
                {
                    OutputFilePreview.Text = temp;
                    outputPreviewString = temp;
                }
            }
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fDialog = new SaveFileDialog();
            fDialog.DefaultExt = "*.txt";
            fDialog.Filter = "Text Files (*.txt)|*.txt|Log Files (*.log)|*.log|All files (*.*)|*.*";

            Nullable<bool> result = fDialog.ShowDialog();

            if (result == true)
            {
                try
                {
                    System.IO.File.WriteAllText(fDialog.FileName, outputPreviewString);
                }
                catch
                {
                    MessageBox.Show("Could not save file");
                }
                finally
                {
                    Cancel_Button.Content = "Close";
                }
            }
        }

        private void Save_All_Button_Click(object sender, RoutedEventArgs e)
        {
            string fileExtension = OutputFileName.Text;
            //foreach (string s in inputFileNames)
            //{
            //    // Read file into string 
            //    // Modify the string
            //    // Modify file extension 
            //    // Write to file 
            //}
            foreach (string s in inputOpenFileDialog.FileNames)
            {
                string buffer;
                try
                {
                    buffer = System.IO.File.ReadAllText(s);
                    buffer = UpdateOutput(buffer);
                    System.IO.File.WriteAllText(s + fileExtension, buffer);
                }
                catch 
                {

                }
                finally
                {

                }
            }
        }

       
    }
}