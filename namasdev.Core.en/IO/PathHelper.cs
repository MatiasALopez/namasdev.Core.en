using System.IO;

namespace namasdev.Core.IO
{
    public class PathHelper
    {
        public static string ChangeFileNameKeepingExtension(string currentFileNameWithExtension, string newNameWithoutExtension)
        {
            return $"{newNameWithoutExtension}{Path.GetExtension(currentFileNameWithExtension)}";
        }
    }
}
