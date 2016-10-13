using System;
using System.IO;
using System.Security.Cryptography;

namespace DirectoryUtilities
{
    public class FileHasher
    {
        public string GetHash(string filePath)
        {
            using (FileStream stream = File.OpenRead(filePath))
            {
                SHA256Managed sha = new SHA256Managed();
                byte[] hashBytes = sha.ComputeHash(stream);
                return BitConverter.ToString(hashBytes);
            }
        }
    }
}
