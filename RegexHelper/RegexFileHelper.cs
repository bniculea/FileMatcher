
using System.Text.RegularExpressions;

namespace RegexHelper
{
    public class RegexFileHelper
    {
        public bool IsExtension(string text)
        {
            Regex regex = new Regex("^[.][a-z]+$");
            Match match = regex.Match(text);
            return match.Success;
        }
    }
}
