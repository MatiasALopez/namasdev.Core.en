using System.Linq;

namespace namasdev.Core.Types
{
    public class NumbersHelper
    {
        public static string CreateNumberFromDigits(int digit, int integerDigitCount, int decimalDigitCount)  
        {
            char digitChar = digit.ToString().First();
            return $"{new string(digitChar, integerDigitCount)}.{new string(digitChar, decimalDigitCount)}";
        }
    }
}
