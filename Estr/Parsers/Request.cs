using System.Net.Http;

namespace Estr.Parsers {
    public record Request(string DomainRoute, HttpMethod method);
}
