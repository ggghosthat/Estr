using System.Net.Http;

namespace Estr.Parsers {
    public record Request(string Path, HttpMethod Method);
}