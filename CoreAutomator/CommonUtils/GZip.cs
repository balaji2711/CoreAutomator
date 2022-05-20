using System.IO.Compression;
using System.Text;

namespace CoreAutomator.CommonUtils
{
    public class GZip
    {
        public static string CompressMessage(string xml)
        {
            byte[] data = ConvertToByteFromString(xml);
            byte[] compress = Compress(data);
            return Base64_Encode(compress);
        }

        public static byte[] ConvertToByteFromString(string data)
        {
            byte[] result;
            result = Encoding.UTF8.GetBytes(data);
            return result;
        }

        public static string ConvertToStringFromByte(byte[] data)
        {
            string result = Encoding.UTF8.GetString(data);
            return result;
        }

        public static byte[] Compress(byte[] data)
        {
            using (var compressedStream = new MemoryStream())
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                zipStream.Write(data, 0, data.Length);
                zipStream.Close();
                return compressedStream.ToArray();
            }
        }

        public static string Base64_Encode(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            return Convert.ToBase64String(data);
        }

        public static byte[] Base64_Decode(string encodedData)
        {
            byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);
            return encodedDataAsBytes;
        }

        public static byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                var buffer = new byte[4096];
                int read;

                while ((read = zipStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    resultStream.Write(buffer, 0, read);
                }
                return resultStream.ToArray();
            }
        }

        public static string PrintDecompressedValueAsString(string data)
        {
            string? result = null;
            byte[] decoded = Base64_Decode(data);
            byte[] decompressed = Decompress(decoded);
            result = Encoding.UTF8.GetString(decompressed);
            return result;
        }

        public static string PrintCompressedValueAsString(byte[] compress)
        {
            string? result = null;
            string encoded = Base64_Encode(compress);
            byte[] decoded = Base64_Decode(encoded);
            result = Encoding.UTF8.GetString(decoded);
            return result;
        }

        public static void CreateXMLDocument(string content, string fileName)
        {
            string startupPath = Directory.GetCurrentDirectory();
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(startupPath, fileName + ".xml")))
            {
                outputFile.WriteLine(content);
            }
        }

        public static MemoryStream ConvertStringToStream(string contents)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(contents);
            MemoryStream stream = new MemoryStream(byteArray);
            return stream;
        }

        public static string ConvertStreamToString(MemoryStream stream)
        {
            StreamReader reader = new StreamReader(stream);
            string text = reader.ReadToEnd();
            return text;
        }
    }
}
