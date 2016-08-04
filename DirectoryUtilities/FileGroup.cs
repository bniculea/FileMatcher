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
    }
}