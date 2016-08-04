using System.Collections.Generic;
using System.Collections.ObjectModel;
using DirectoryUtilities;

namespace FileMatcherController
{
    public class Controller
    {
        private string Path { get; set; }
        private string Extension { get; set; }

        public Controller(string path, string extension)
        {
            Path = path;
            Extension = extension;
        }

        public ObservableCollection<FileGroup> GetGroupedFiles()
        {
            FilesFinder filesFinder = new FilesFinder(new ExtensionFilterCondition(Extension));
            List<string> files = filesFinder.GetFiles(Path);
            FileGrouper fileGrouper = new FileGrouper(files);
            ObservableCollection<FileGroup> groups = fileGrouper.GetGroups();
            return groups;
        }
    }
}
