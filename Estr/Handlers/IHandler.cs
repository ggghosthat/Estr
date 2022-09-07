using System.IO;
using Estr.Parsers;
using System.Threading.Tasks;

namespace Estr.Handlers{
    public interface IHandler{
        void Handle(Stream stream, Request request);
        Task HandleAsync(Stream stream, Request request);
    }
}