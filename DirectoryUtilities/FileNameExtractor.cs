using System;
using System.IO;

namespace DirectoryUtilities
{
    public  class FileNameExtractor
    {

        public string Extract(string filePath)
        {
            if(string.IsNullOrEmpty(filePath)) throw new ArgumentException();
            int lastIndexOfPathDelimiter = filePath.LastIndexOf(Path.DirectorySeparatorChar);
            return filePath.Substring(lastIndexOfPathDelimiter+1);
        }
    }
}