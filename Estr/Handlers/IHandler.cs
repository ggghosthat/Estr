using System.IO;
using Estr.Parsers;
using System.Threading.Tasks;
using System;
namespace Estr.Handlers{
    public interface IHandler{
        public Guid HandlerId {get; init;}
        public bool IsActive {get;}

        void Handle(Stream stream, Request request);
        Task HandleAsync(Stream stream, Request request);

        void Activate();
        void Deactivate();
    }
}
