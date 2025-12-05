using System;
using System.Collections.Generic;

namespace namasdev.Core.Types
{
    public static class ByteArrayExtensions
    {
        public static int? GetInt(this byte[] bytes, int index, int byteCount)
        {
            int result = 0;

            if (byteCount > 3)
            {
                result |= bytes[index++] << 24;
            }
            if (byteCount > 2)
            {
                result |= bytes[index++] << 16;
            }
            if (byteCount > 1)
            {
                result |= bytes[index++] << 8;
            }
            result |= bytes[index];

            return result;
        }

        public static decimal?[] GetDecimalList(this byte[] bytes, int blockSize, int decimalDigits)
        {
            var list = new List<decimal?>();

            int blockCount = (bytes.Length - 4) / blockSize;
            int pos;
            for (int i = 0; i < blockCount; i++)
            {
                pos = (i * blockSize) + 4;

                list.Add(ObtenerDecimal(bytes, pos, blockSize - 1, decimalDigits));
            }

            return list.ToArray();
        }

        public static decimal? ObtenerDecimal(this byte[] bytes, int index, int byteCount, int decimalDigits)
        {
            int? integer = bytes[index] == 0
                ? GetInt(bytes, index + 1, byteCount)
                : null;

            if (integer.HasValue)
            {
                return integer.Value * (decimal)Math.Pow(10, -decimalDigits);
            }
            else
            {
                return null;
            }
        }
    }
}
