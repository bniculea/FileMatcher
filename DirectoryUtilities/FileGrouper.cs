using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DirectoryUtilities
{
    public class FileGrouper
    {
        private List<string> Files { get; set; }

        public FileGrouper(List<string> files )
        {
            Files = files;
        }

        public ObservableCollection<FileGroup> GetGroups()
        {
            FileNameExtractor  fileNameExtractor = new FileNameExtractor();
            Dictionary<string, FileGroup> fileGroups = new Dictionary<string, FileGroup>();
            foreach (string filePath in Files)
            {
                string fileName = fileNameExtractor.Extract(filePath);

                if (!fileGroups.ContainsKey(fileName))
                {
                    fileGroups.Add(fileName, new FileGroup(fileName, new List<string> { filePath }));
                }
                else
                {
                    fileGroups[fileName].Add(filePath);
                }
            }
            return CreateFileGroupsCollection(fileGroups);
        }

        private ObservableCollection<FileGroup> CreateFileGroupsCollection(Dictionary<string, FileGroup> fileGroups)
        {
            ObservableCollection<FileGroup> fileGroupsCollection = new ObservableCollection<FileGroup>();
            foreach (string key in fileGroups.Keys)
            {
                fileGroupsCollection.Add(fileGroups[key]);
            }
            return fileGroupsCollection;
        }
    }
}
