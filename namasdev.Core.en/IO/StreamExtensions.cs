using System.IO;

using namasdev.Core.Validation;

namespace namasdev.Core.IO
{
    public static class StreamExtensions
    {
        public static byte[] ReadAllBytes(this Stream stream)
        {
            Validator.ValidateRequiredArgumentAndThrow(stream, nameof(stream));
            
            int length = (int)stream.Length;
            var bytes = new byte[length];
            stream.Read(bytes, 0, length);

            return bytes;
        }
    }
}
