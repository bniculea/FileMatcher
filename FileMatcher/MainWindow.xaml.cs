using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using DirectoryUtilities;
using FileMatcherController;
using Microsoft.WindowsAPICodePack.Dialogs;
using RegexHelper;
using MessageBox = System.Windows.MessageBox;

namespace FileMatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string IdColumn = "Nr";
        private const string FileNameColumn = "Filename";
        public const string SameContentColumn = "Equals";
        private const string LocationColumn = "Location";
        private BackgroundWorker BackgroundWorker { get; set; }
        private string Extension { get; set; }
        private string DirectoryPath { get; set; }
        private ObservableCollection<FileGroup> FileGroups { get; set; }
        private DataTable DataTable { get; set; }
        public DataView DataView { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            BackgroundWorker = new BackgroundWorker();
        }

        private void BrowseButton_OnClick(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog("Select folder to search for files:");
            commonOpenFileDialog.IsFolderPicker = true;
            CommonFileDialogResult result =  commonOpenFileDialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                TxtSelectedPath.Text = commonOpenFileDialog.FileName;
            }
        }

        private void ButtonRun_OnClick(object sender, RoutedEventArgs e)
        {
            Extension = TxtExtension.Text;
            DirectoryPath = TxtSelectedPath.Text;
            BackgroundWorker.WorkerReportsProgress = true;
            BackgroundWorker.DoWork += BackgroundWorker_DoWork;
            BackgroundWorker.RunWorkerAsync();
            BackgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
           
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Dispatcher.Invoke(()=> ProgressBarStatus.IsIndeterminate = false);
            BackgroundWorker.DoWork -= BackgroundWorker_DoWork;
            BackgroundWorker.RunWorkerCompleted -= BackgroundWorker_RunWorkerCompleted;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (AreInputsValid())
            {
                Dispatcher.Invoke(() => ProgressBarStatus.IsIndeterminate = true);
                Controller controller = new Controller(DirectoryPath, Extension);
                FileGroups = controller.GetGroupedFiles();
                Dispatcher.Invoke(() => PopulateViewFromGroups(FileGroups));
                Dispatcher.Invoke(()=>MessageBox.Show(this, "Matching finished!", "FileMatcher", MessageBoxButton.OK, MessageBoxImage.Information));

            }
            else
            {
                Dispatcher.Invoke(()=>MessageBox.Show(this, "Invalid extension type!", "FileMatcher", MessageBoxButton.OK, MessageBoxImage.Error));
            }
        }

        private void PopulateViewFromGroups(ObservableCollection<FileGroup> fileGroups)
        {
            DataTable = new DataTable();
            DataTable.Columns.Add(IdColumn);
            DataTable.Columns.Add(FileNameColumn);
            DataTable.Columns.Add(SameContentColumn);
            int rowIndex = 1;
            AddFileGroupsToDataTable(fileGroups, rowIndex);
            SetUpDataGridRendering();
        }

        private void SetUpDataGridRendering()
        {
            DataContext = null;
            DataView = DataTable.DefaultView;
            DataContext = this;
        }

        private void AddFileGroupsToDataTable(ObservableCollection<FileGroup> fileGroups, int rowIndex)
        {
            foreach (FileGroup fileGroup in fileGroups)
            {
                DataRow dataRow = DataTable.NewRow();
                dataRow[IdColumn] = $"{rowIndex++}";
                dataRow[FileNameColumn] = fileGroup.Name;
                dataRow[SameContentColumn] = fileGroup.AreFileEqualsInGroup().ToString();
                int locationCount = 1;
                CreateAndPopulateDataRowFromFileGroup(fileGroup, locationCount, dataRow);
                DataTable.Rows.Add(dataRow);
            }
        }

        private void CreateAndPopulateDataRowFromFileGroup(FileGroup fileGroup, int locationCount, DataRow dataRow)
        {
            foreach (string file in fileGroup.GroupFiles)
            {
                if (!DataTable.Columns.Contains($"{LocationColumn} #{locationCount}"))
                {
                    DataTable.Columns.Add($"{LocationColumn} #{locationCount}");
                }
                dataRow[$"{LocationColumn} #{locationCount}"] = file;

                locationCount++;
            }
        }

        private bool AreInputsValid()
        {
            RegexFileHelper regexFileHelper = new RegexFileHelper();
            return !string.IsNullOrEmpty(Extension) && regexFileHelper.IsExtension(Extension);
        }


        private void FileMatchedGridView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dependencyObject = (DependencyObject) e.OriginalSource;
            while (dependencyObject != null && !(dependencyObject is DataGridCell) &&
                   !(dependencyObject is DataGridColumnHeader))
            {
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }
            if (dependencyObject == null) return;
            OpenExplorerAtSelectedLocation(dependencyObject);
        }

        private void OpenExplorerAtSelectedLocation(DependencyObject dependencyObject)
        {
            if (dependencyObject is DataGridCell)
            {
                DataGridCell selectedDataGridCell = dependencyObject as DataGridCell;
                TextBlock txt = selectedDataGridCell.Content as TextBlock;
                if (File.Exists(txt.Text))
                {
                    string filePath = Path.GetDirectoryName(txt.Text);
                    Process.Start(filePath);
                }
            }
        }
    }
}
