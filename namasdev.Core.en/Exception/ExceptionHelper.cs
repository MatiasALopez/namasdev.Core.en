using System;
using System.Text;

namespace namasdev.Core.Exceptions
{
    public class ExceptionHelper
    {
        public static string GetMessagesRecursively(Exception ex)
        {
            var sbMessage = new StringBuilder(ex.Message);
            if (ex.InnerException != null)
            {
                sbMessage.Append(Environment.NewLine + GetMessagesRecursively(ex.InnerException));
            }
            return sbMessage.ToString();
        }
    }
}
