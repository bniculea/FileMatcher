using System;

namespace DirectoryUtilities
{
    public class ExtensionFilterCondition : IFileFilterCondition
    {
        private string ExtensionToBeSelected { get; set; }
        public ExtensionFilterCondition(string extensionToBeSelected)
        {
            ExtensionToBeSelected = extensionToBeSelected;
        }

        public bool IsFullfilled(string file)
        {
            return file.EndsWith(ExtensionToBeSelected, StringComparison.OrdinalIgnoreCase);
        }
    }
}
