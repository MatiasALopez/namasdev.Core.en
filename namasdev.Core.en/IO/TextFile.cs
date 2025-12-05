using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using namasdev.Core.Processing;

namespace namasdev.Core.IO
{
    public class TextFile
    {
        public static void ProcessLinesUsingReader(
           byte[] fileContent, int batchSize, Action<BatchArgs<string>> processAction,
           int? lineNumberFrom = null, int? lineNumberTo = null,
           Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.Default;

            int lineNumber = 0,
                batchLineCount = 0,
                batchNum = 1;
            string line;
            var batchLines = new List<string>();
            BatchArgs<string> batchArgs;
            using (var reader = new StreamReader(new MemoryStream(fileContent), encoding))
            {
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    lineNumber++;

                    if (lineNumberTo.HasValue && lineNumber > lineNumberTo.Value)
                    {
                        break;
                    }

                    if (lineNumberFrom.HasValue && lineNumber < lineNumberFrom)
                    {
                        continue;
                    }

                    batchLines.Add(line);
                    batchLineCount++;

                    if (batchLineCount == batchSize)
                    {
                        batchArgs = new BatchArgs<string> { BatchNumber = batchNum, Items = batchLines };
                        processAction(batchArgs);

                        batchLines.Clear();
                        batchLineCount = 0;

                        if (batchArgs.CancelProcessing)
                        {
                            break;
                        }

                        batchNum++;
                    }
                }

                if (batchLineCount > 0)
                {
                    batchArgs = new BatchArgs<string> { BatchNumber = batchNum, Items = batchLines };
                    processAction(batchArgs);
                }
            }
        }

        public static IEnumerable<string> ReadLinesUsingReader(
            byte[] fileContent,
            int? lineNumberFrom = null, int? lineNumberTo = null,
            Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.Default;

            int lineNumber = 1;
            string line;
            using (var reader = new StreamReader(new MemoryStream(fileContent), encoding))
            {
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();

                    if (lineNumberFrom.HasValue && lineNumber < lineNumberFrom)
                    {
                        lineNumber++;
                        continue;
                    }

                    if (lineNumberTo.HasValue && lineNumber > lineNumberTo.Value)
                    {
                        yield break;
                    }

                    yield return line;

                    lineNumber++;
                }
            }

            yield break;
        }

        public static IEnumerable<string> ReadLines(
            byte[] fileContent,
            Encoding encoding = null,
            string newLineSeparator = null)
        {
            encoding = encoding ?? Encoding.Default;
            newLineSeparator = newLineSeparator ?? Environment.NewLine;

            return encoding.GetString(fileContent)
                .Split(new[] { newLineSeparator }, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
        }

        public static IEnumerable<T> ReadItems<T>(
            byte[] fileContent, Func<string[], T> recordMap,
            char fieldSeparator = ',', bool containsLineHeaders = false,
            Encoding encoding = null, string newLineSeparator = null)
            where T : class
        {
            return ReadLines(fileContent, encoding, newLineSeparator)
                .Skip(containsLineHeaders ? 1 : 0)
                .Select(linea => recordMap(linea.Split(fieldSeparator)))
                .ToArray();
        }

        public static File CreateFromItems<T>(string fileName, IEnumerable<T> items, Func<T, string> itemMap,
            string headersLine = null,
            Encoding encoding = null, string newLineSeparator = null)
        {
            if (itemMap == null)
            {
                itemMap = (e) => Convert.ToString(e);
            }

            encoding = encoding ?? Encoding.Default;
            newLineSeparator = newLineSeparator ?? Environment.NewLine;

            var lines = items.Select(itemMap).ToList();

            if (!String.IsNullOrWhiteSpace(headersLine))
            {
                lines.Insert(0, headersLine);
            }

            return new File { Name = fileName, Content = encoding.GetBytes(String.Join(newLineSeparator, lines)) };
        }

        public static void CreateOrUpdateFromItemsUsingWriter<T>(string filePath, IEnumerable<T> items, Func<T, string> itemMap,
            string headersLine = null,
            Encoding encoding = null, string newLineSeparator = null)
        {
            if (itemMap == null)
            {
                itemMap = (e) => Convert.ToString(e);
            }

            encoding = encoding ?? Encoding.Default;
            newLineSeparator = newLineSeparator ?? Environment.NewLine;

            int nonFlushedLineCount = 0,
                maxLinesToFlush = 1000;

            using (var writer = new StreamWriter(filePath, append: false, encoding: encoding))
            {
                if (!String.IsNullOrWhiteSpace(headersLine))
                {
                    writer.Write(headersLine + newLineSeparator);
                }

                foreach (var e in items)
                {
                    writer.Write(itemMap(e) + newLineSeparator);

                    nonFlushedLineCount++;

                    if (nonFlushedLineCount >= maxLinesToFlush)
                    {
                        writer.Flush();
                        nonFlushedLineCount = 0;
                    }
                }

                if (nonFlushedLineCount > 0)
                {
                    writer.Flush();
                }

                writer.Close();
            }
        }
    }
}
