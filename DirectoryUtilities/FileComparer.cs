
using System.IO;

namespace DirectoryUtilities
{
    public class FileComparer
    {
        public static bool CompareFiles(string filename1, string filename2)
        {
            FileInfo fileinfo1 = new FileInfo(filename1);
            FileInfo fileinfo2 = new FileInfo(filename2);

            bool same = fileinfo1.Length == fileinfo2.Length;
            if (same)
            {
                using (FileStream fs1 = fileinfo1.OpenRead())
                using (FileStream fs2 = fileinfo2.OpenRead())
                using (BufferedStream bs1 = new BufferedStream(fs1))
                using (BufferedStream bs2 = new BufferedStream(fs2))
                {
                    for (long i = 0; i < fileinfo1.Length; i++)
                    {
                        if (bs1.ReadByte() != bs2.ReadByte())
                        {
                            same = false;
                            break;
                        }
                    }
                }
            }
            return same;
        }
    }
}
