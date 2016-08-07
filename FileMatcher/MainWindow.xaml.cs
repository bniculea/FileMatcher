﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
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
        private string Extension { get; set; }
        private string DirectoryPath { get; set; }
        private ObservableCollection<FileGroup> FileGroups { get; set; }
        private DataTable DataTable { get; set; }
        public DataView DataView { get; set; }
        private ICollectionView _dataGridCollection;
       
        public MainWindow()
        {
            InitializeComponent();
        }

      public ICollectionView DataGridCollection
        {
            get { return _dataGridCollection; }
            set
            {
                _dataGridCollection = value;
                NotifyPropertyChanged("DataGridCollection");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
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

        private async void ButtonRun_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsLocationSet())
            {
                if (IsExtensionInputValid())
                {
                   
                    Extension = TxtExtension.Text;
                    DirectoryPath = TxtSelectedPath.Text;
                    EnableControls(false);
                    Task<ObservableCollection<FileGroup>> fileMatchingTask = Task.Run(() => RunMatching());
                    await Task.Run(()=>PopulateViewFromGroups(fileMatchingTask.Result));
                    EnableControls(true);
                    MessageBox.Show(this, "Matching finished!", "FileMatcher", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                   MessageBox.Show(this, "Invalid extension type!", "FileMatcher", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(this, "Please select a location where to search.", "FileMatcher", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void EnableControls(bool isEnabled)
        {
            ProgressBarStatus.IsIndeterminate = !isEnabled;
            InputPanel.IsEnabled = isEnabled;
            ControlPanel.IsEnabled = isEnabled;
        }

        private ObservableCollection<FileGroup> RunMatching()
        {
            Controller controller = new Controller(DirectoryPath, Extension);
            FileGroups = controller.GetGroupedFiles();
            return FileGroups;
        }


        private bool IsLocationSet()
        {
            return !string.IsNullOrEmpty(TxtSelectedPath.Text);
        }

        private void PopulateViewFromGroups(ObservableCollection<FileGroup> fileGroups)
        {
            PrepareDataTable();
            AddFileGroupsToDataTable(fileGroups);
            SetUpDataGridRendering();
        }

        private void PrepareDataTable()
        {
            DataTable = new DataTable();
            DataTable.Columns.Add(IdColumn);
            DataTable.Columns.Add(FileNameColumn);
            DataTable.Columns.Add(SameContentColumn);
        }

        private void SetUpDataGridRendering()
        {
            Dispatcher.Invoke(() =>
            {
                DataContext = null;
                DataGridCollection = CollectionViewSource.GetDefaultView(DataTable);
                DataContext = this;
            });
        }

        private void AddFileGroupsToDataTable(ObservableCollection<FileGroup> fileGroups)
        {
            int rowIndex = 1;
            foreach (FileGroup fileGroup in fileGroups)
            {
                DataRow dataRow = DataTable.NewRow();
                dataRow[IdColumn] = $"{rowIndex++}";
                dataRow[FileNameColumn] = fileGroup.Name;
                dataRow[SameContentColumn] = fileGroup.AreFileEqualsInGroup().ToString();
              
                CreateAndPopulateDataRowFromFileGroup(fileGroup, dataRow);
                DataTable.Rows.Add(dataRow);
            }
        }

        private void CreateAndPopulateDataRowFromFileGroup(FileGroup fileGroup, DataRow dataRow)
        {
            int locationCount = 1;
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

        private bool IsExtensionInputValid()
        {
            RegexFileHelper regexFileHelper = new RegexFileHelper();
            return !string.IsNullOrEmpty(TxtExtension.Text) && regexFileHelper.IsExtension(TxtExtension.Text);
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

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
