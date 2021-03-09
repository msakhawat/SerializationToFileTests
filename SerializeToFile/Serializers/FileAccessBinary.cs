using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Converters;
using SerializeToFile.Dtos;

namespace SerializeToFile.Serializers
{
    public class FileAccessBinary : FileAccess
    {
        public static void WriteReadBinary(string filePath)
        {
            var stopwatchTotal = new Stopwatch();
            var stopwatchWrite = new Stopwatch();
            var stopwatchRead = new Stopwatch();
            stopwatchTotal.Start();
            stopwatchWrite.Start();
            long length = WriteBinary(filePath);
            stopwatchWrite.Stop();
            stopwatchRead.Start();
            ReadBinary(filePath);
            stopwatchRead.Stop();
            stopwatchTotal.Stop();

            ExcelResultsReadAndWrite.AppendFormat("{0},", stopwatchTotal.ElapsedMilliseconds);
            ExcelResultsRead.AppendFormat("{0},", stopwatchRead.ElapsedMilliseconds);
            ExcelResultsWrite.AppendFormat("{0},", stopwatchWrite.ElapsedMilliseconds);
            ExcelResultsSize.AppendFormat("{0},", length);

            Console.WriteLine("Binary \t\t R/W:{0} \t R:{2} \t W:{1} \t Size:{3}",
                stopwatchTotal.ElapsedMilliseconds, stopwatchWrite.ElapsedMilliseconds,
                stopwatchRead.ElapsedMilliseconds,
                length);

            //File.Delete(filePath);
        }

        private static long WriteBinary(string path)
        {
            using (var fs = File.Create(path))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fs, GetTestObjects());
            }

            return new FileInfo(path).Length;
        }

        private static void ReadBinary(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                var binaryFormatter = new BinaryFormatter();
                SimpleTransferProtobuf simpleTransferProtobuf;
                simpleTransferProtobuf = (SimpleTransferProtobuf)binaryFormatter.Deserialize(fs);
            }
        }
    }
}
