using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
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
        private Controller Controller { get; set; }
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
                    if (fileMatchingTask.Result.Count == 0)
                    {
                        HandleWhenNoResultsAreReturned();
                    }
                    else
                    {
                        await Task.Run(() => PopulateViewFromGroups(fileMatchingTask.Result));
                        EnableControls(true);
                        MessageBox.Show(this, "Matching finished!", "File Matcher", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                   MessageBox.Show(this, "Invalid extension type!", "File Matcher", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(this, "Please select a location where to search.", "File Matcher", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void HandleWhenNoResultsAreReturned()
        {
            EnableControls(true);
            MessageBox.Show(this,
                "No results returned using the current settings. Try to change the extension or the location",
                "File matcher", MessageBoxButton.OK, MessageBoxImage.Information);
            FileMatchedGridView.ItemsSource = null;
            FileMatchedGridView.Items.Refresh();
        }

        private void EnableControls(bool isEnabled)
        {
            ProgressBarStatus.IsIndeterminate = !isEnabled;
            InputPanel.IsEnabled = isEnabled;
            ControlPanel.IsEnabled = isEnabled;
        }

        private ObservableCollection<FileGroup> RunMatching()
        {
            Controller = new Controller(DirectoryPath, Extension);
            FileGroups = Controller.GetGroupedFiles();
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
            foreach (string file in fileGroup.GroupFilePaths)
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

        private void FileMatchedGridView_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            bool areEqual = Convert.ToBoolean(((DataRowView)(e.Row.DataContext)).Row.ItemArray[2]);
            Dictionary<string, List<Tuple<string, int>>> filesGroupedByContent = new Dictionary<string, List<Tuple<string,int>>>();
            // for each filePathInRow group files in anDictionart
            if (!areEqual)
            {
                FileHasher fileHasher = new FileHasher();
                object[] fileLocations = ((DataRowView) (e.Row.DataContext)).Row.ItemArray;
                for (int i = 3; i < fileLocations.Length; i++)
                {
                    if (!string.IsNullOrEmpty(fileLocations[i].ToString()))
                    {
                        string hash = fileHasher.GetHash(fileLocations[i].ToString());
                        if (!filesGroupedByContent.ContainsKey(hash))
                        {
                            filesGroupedByContent.Add(hash, new List<Tuple<string, int>> {Tuple.Create(fileLocations[i].ToString(), i)});
                        }
                        else
                        {
                            filesGroupedByContent[hash].Add(Tuple.Create(fileLocations[i].ToString(), i));
                        }
                    }
                }
                DataGridRow dataGridRow = e.Row;
               // Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() => AlterRow(e)));
               AlterRow(e,3, filesGroupedByContent);
            }
        }

        private Brush GetNextColor(int index)
        {
            SolidColorBrush[] AvailableColors = new[] {new SolidColorBrush(Colors.Green), new SolidColorBrush(Colors.Red), new SolidColorBrush(Colors.Yellow)};
            return AvailableColors[index];
        }


        private void AlterRow(DataGridRowEventArgs e, int index, Dictionary<string, List<Tuple<string, int>>> filesGroupedByHash)
        {
            var cell = GetCell(FileMatchedGridView, e.Row, index);
            if (cell == null) return;

            var item = e.Row.Item;
            if (item == null) return;

            TextBlock cellContent = cell.Content as TextBlock;
            //if (cellContent != null)
            //{
            //    cell.Background = GetNextColor()
            //}
            cell.Background = Brushes.Red;
        }

        public static DataGridRow GetRow(DataGrid grid, int index)
        {
            var row = grid.ItemContainerGenerator.ContainerFromIndex(index) as DataGridRow;

            if (row == null)
            {
                // may be virtualized, bring into view and try again
                grid.ScrollIntoView(grid.Items[index]);
                row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }

        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                var v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T ?? GetVisualChild<T>(v);
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        public static DataGridCell GetCell(DataGrid host, DataGridRow row, int columnIndex)
        {
            if (row == null) return null;

            var presenter = GetVisualChild<DataGridCellsPresenter>(row);
            if (presenter == null) return null;

            // try to get the cell but it may possibly be virtualized
            var cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
            if (cell == null)
            {
                // now try to bring into view and retreive the cell
                host.ScrollIntoView(row, host.Columns[columnIndex]);
                cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
            }
            return cell;

        }

    }
}
