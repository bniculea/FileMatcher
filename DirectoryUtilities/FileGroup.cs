using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.SqlServer.Server;

namespace DirectoryUtilities
{
    public class FileGroup
    {
        public string Name { get; private set; }
        public List<string> GroupFilePaths{ get; set; }

        public FileGroup(string name, List<string> groupFilePaths)
        {
            Name = name;
            GroupFilePaths = groupFilePaths;
        }

        public void Add(string fileItem)
        {
            GroupFilePaths.Add(fileItem);
        }

        public bool AreFileEqualsInGroup()
        {
            bool same = true;
            for (int i = 0; i < GroupFilePaths.Count-1; i++)
            {
                if (!FileComparer.CompareFiles(GroupFilePaths[i], GroupFilePaths[i + 1]))
                {
                    same = false;
                    break;
                }
            }
            return same;
        }
    }
}