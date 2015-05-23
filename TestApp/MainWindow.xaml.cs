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


namespace LogFileManipulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private string[] inputFilePaths;
        //private OpenFileDialog inputOpenFileDialog;
        private List<string> inputFiles = new List<string>();
        private List<string> inputFilePaths = new List<string>();
        private string inputFilePreviewString = "";
        private string outputPreviewString = "";

#region Shit I won't change
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

        private static Label Create_Regex_Label(int gridRowNumber)
        { 
            Label label = new Label();
            label.IsEnabled = false;
            label.Content = "/";
            label.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            Grid.SetColumn(label, gridRowNumber);
            return label;
        }

#endregion
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
            comboBox.Items.Add("Match Line");
            comboBox.Items.Add("Remove Lines");
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

            Button deleteLineButton = new Button();
            deleteLineButton.Margin = new Thickness(5);
            deleteLineButton.Content = "X";
            deleteLineButton.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            deleteLineButton.Click += deleteLineButton_Click;
            Grid.SetColumn(deleteLineButton, 7);
            gridRow.Children.Add(deleteLineButton);

            try
            {
                OperationsStack.Children.Add(gridRow);
            }
            catch
            {
                MessageBox.Show("We Tried. We Failed. =(");
            }
        }

        void deleteLineButton_Click(object sender, RoutedEventArgs e)
        {
            Grid grid = ((sender as Button).Parent as Grid);
            (grid.Parent as StackPanel).Children.Remove(grid);

            outputPreviewString = CalculateOutput(inputFilePreviewString);
            OutputFilePreview.Text = outputPreviewString;
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            Grid grid = VisualTreeHelper.GetParent(sender as ComboBox) as Grid;
            if ((sender as ComboBox).Text == "Match Line")
            {
                grid.ColumnDefinitions[4].Width = new GridLength(0, GridUnitType.Star);
                grid.ColumnDefinitions[5].Width = new GridLength(0, GridUnitType.Star);
                grid.ColumnDefinitions[6].Width = new GridLength(0, GridUnitType.Star);
            }
            else if ((sender as ComboBox).Text == "Remove Lines")
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
            outputPreviewString = CalculateOutput(inputFilePreviewString);
            OutputFilePreview.Text = outputPreviewString;
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
                // Opens and reads files, adds to list
                // TODO: Check for douplicates
                Stream[] inputFileStream = fileDialog.OpenFiles();
                inputFilePaths.AddRange(fileDialog.FileNames);
                foreach (Stream s in inputFileStream)
                {
                    inputFiles.Add((new StreamReader(s)).ReadToEnd());
                }
                UpdatePreview();
            }
        }

        private void RegexBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string temp = CalculateOutput(inputFilePreviewString);
            if (temp != outputPreviewString)
            {
                OutputFilePreview.Text = temp;
                outputPreviewString = temp;
            }

        }


        private string CalculateOutput(string buffer)
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
                        if (operation == "Match Line")
                        {
                            buffer = RegexTasks.MatchLine(buffer, a);
                        }
                        else if (operation == "Find/Replace")
                        {
                            buffer = RegexTasks.FindAndReplace(buffer, a, b);
                        }
                        else if (operation == "Remove Lines")
                        {
                            buffer = RegexTasks.NoMatchLine(buffer, a);
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
                string temp = CalculateOutput(inputFilePreviewString);
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
                    if (inputFiles.Count > 1)
                    {
                        inputFiles.RemoveAt(0);
                        inputFilePaths.RemoveAt(0);
                    }

                    UpdatePreview();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not save file \n\n" + ex.ToString());
                    
                    Debug.WriteLine("Could not save file ");
                    Debug.WriteLine(ex.ToString());
                }
                finally
                {
                    Cancel_Button.Content = "Close";
                }
            }
        }

        private void UpdatePreview()
        {
            // File Name Preview and Save All Button
            if (inputFiles.Count > 0)
            {
                if (inputFiles.Count == 1)
                {
                    InputFileNameTextBlock.Text = Path.GetFileName(inputFilePaths[0]);
                    Save_All_Button.IsEnabled = false;
                }
                else // Multiple Files
                {
                    Save_All_Button.IsEnabled = true;
                    Save_All_Button.ClearValue(Button.ToolTipProperty);
                    InputFileNameTextBlock.Text = Path.GetFileName(inputFilePaths[0]) + " ["
                                              + inputFilePaths.Count.ToString() + "]";
                }

                InputClearButton.Visibility = System.Windows.Visibility.Visible;
                Save_Button.IsEnabled = true;
                // Update input preview 
                if (inputFilePreviewString != inputFiles[0])
                {
                    InputFilePreview.Text = inputFilePreviewString = inputFiles[0];
                }

                // Update output preview 
                string temp = CalculateOutput(inputFiles[0]);
                if (temp != outputPreviewString)
                {
                    OutputFilePreview.Text = outputPreviewString = temp;
                }
            }
            else // No files left, or somehow negative files. 
            {
                InputFilePreview.Text = inputFilePreviewString = "";
                OutputFilePreview.Text = outputPreviewString = "";
                InputFileNameTextBlock.Text = "";
                InputClearButton.Visibility = System.Windows.Visibility.Hidden;
                Save_All_Button.IsEnabled = false;
                Save_Button.IsEnabled = false;
            }
        }

        private void Save_All_Button_Click(object sender, RoutedEventArgs e)
        {
            string fileExtension = OutputFileName.Text;
            try
            {
                for (int i = 0; i < inputFiles.Count; i++)
                {
                    System.IO.File.WriteAllText(Path.GetDirectoryName(inputFilePaths[i]) + "\\" +
                                                Path.GetFileNameWithoutExtension(inputFilePaths[i]) + 
                                                fileExtension,
                                                CalculateOutput(inputFiles[i]));
                }
                inputFiles.Clear();
                inputFilePaths.Clear();
            }
            catch
            {
                // Do something
            }
            finally
            {
                UpdatePreview();
            }
        }

        private void InputClearButton_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("aoeu");
            inputFiles.Clear();
            inputFilePaths.Clear();
            UpdatePreview();
        }
    }
}