namespace DirectoryUtilities
{
    public interface IFileFilterCondition
    {
        bool IsFullfilled(string file);
    }
}
