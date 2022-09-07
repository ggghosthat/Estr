using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace Estr.Parsers {
    internal static class ResponseWriter
    {
        public static void WriteStatusCode(HttpStatusCode statusCode, Stream stream)
        {
            using var streamWriter = new StreamWriter(stream, leaveOpen:true);
            streamWriter.WriteLine($"HTTP/1.0 {(int)statusCode} - {statusCode}");
            streamWriter.WriteLine();
        }

        public static async Task WriteStatusCodeAsync(HttpStatusCode statusCode, Stream stream)
        {
            using var streamWriter = new StreamWriter(stream, leaveOpen:true);
            await streamWriter.WriteLineAsync($"HTTP/1.0 {(int)statusCode} - {statusCode}");
            await streamWriter.WriteLineAsync();
        }
    }
}