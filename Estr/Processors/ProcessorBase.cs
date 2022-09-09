using System.Net;
using System.Net.Sockets;
using Estr.Handlers;

namespace Estr.Processor
{
    abstract class ProcessorBase : IProcessor
    {
        public abstract void Process(TcpClient client);
        
        
        public abstract void RegistHandler(IHandler handler);
        public abstract void RemoveHandler(string handler);

        public abstract void RemoveAllHandlers();

        public abstract void ActivateAllHandlers();
        public abstract void DeactivateAllHandlers();

        public abstract void ActivateHandler(string hid);
        public abstract void DeactivateHandler(string hid);

    }
}
