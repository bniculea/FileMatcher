using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.SqlServer.Server;

namespace DirectoryUtilities
{
    public class FileGroup
    {
        public string Name { get; private set; }
        public List<string> GroupFiles{ get; set; }

        public FileGroup(string name, List<string> groupFiles)
        {
            Name = name;
            GroupFiles = groupFiles;
        }

        public void Add(string fileItem)
        {
            GroupFiles.Add(fileItem);
        }

        public bool AreFileEqualsInGroup()
        {
            bool same = true;
            for (int i = 0; i < GroupFiles.Count-1; i++)
            {
                if (!FileComparer.CompareFiles(GroupFiles[i], GroupFiles[i + 1]))
                {
                    same = false;
                    break;
                }
            }
            return same;
        }
    }
}