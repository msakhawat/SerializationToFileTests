using SerializeToFile.Dtos;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SerializeToFile.Serializers
{
    public class FileAccessBuildInJson : FileAccess
    {
        public static void WriteReadBuildInJson(string filePath)
        {
            var stopwatchTotal = new Stopwatch();
            var stopwatchWrite = new Stopwatch();
            var stopwatchRead = new Stopwatch();
            stopwatchTotal.Start();
            stopwatchWrite.Start();
            long length = WriteBuildInJson(filePath);
            stopwatchWrite.Stop();
            stopwatchRead.Start();
            ReadBuildInJson(filePath);
            stopwatchRead.Stop();
            stopwatchTotal.Stop();

            ExcelResultsReadAndWrite.AppendFormat("{0},", stopwatchTotal.ElapsedMilliseconds);
            ExcelResultsRead.AppendFormat("{0},", stopwatchRead.ElapsedMilliseconds);
            ExcelResultsWrite.AppendFormat("{0},", stopwatchWrite.ElapsedMilliseconds);
            ExcelResultsSize.AppendFormat("{0},", length);

            Console.WriteLine("BuildInJson \t R/W:{0} \t R:{2} \t W:{1} \t Size:{3}",
                stopwatchTotal.ElapsedMilliseconds, stopwatchWrite.ElapsedMilliseconds,
                stopwatchRead.ElapsedMilliseconds,
                length);

            //File.Delete(filePath);
        }

        private static long WriteBuildInJson(string path)
        {
            using (var fs = File.Create(path))
            {
                Utf8JsonWriter utf8JsonWriter = new Utf8JsonWriter(fs);
                JsonSerializer.Serialize(utf8JsonWriter, GetTestObjects(), new JsonSerializerOptions { IgnoreNullValues = true });
            }

            return new FileInfo(path).Length;
        }

        private static void ReadBuildInJson(string path)
        {
            var jsonString = File.ReadAllText(path);
            var simpleTransferProtobuf = JsonSerializer.Deserialize<SimpleTransferProtobuf>(jsonString);
        }
    }
}
