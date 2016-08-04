using System.Collections.Generic;
using System.IO;

namespace DirectoryUtilities
{
    public class FilesFinder
    {
        private  List<string> Files { get; set; }
        private IFileFilterCondition FilterCondition { get; set; }
        public FilesFinder(IFileFilterCondition filterCondition)
        {
            FilterCondition = filterCondition;
            Files = new List<string>();
        }

        public List<string> GetFiles(string path)
        {
            foreach (string directory in Directory.GetDirectories(path))
            {
                GetFiles(directory);
            }
            foreach (string variable in Directory.EnumerateFiles(path))
            {
                if (FilterCondition.IsFullfilled(variable))
                {
                    Files.Add(variable);
                }
            }
            return Files;
        }
    }

}
