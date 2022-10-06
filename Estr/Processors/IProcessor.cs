using System.Net.Sockets;
using System.Net;
using Estr.Handlers;
using System;

namespace Estr.Processor
{
    public interface IProcessor
    {
        
        void Process(string domainRoute);
        void RegistHandler(IHandler handler);
        void RemoveHandler(Guid hid);

        void RemoveAllHandlers();

        void ActivateAllHandlers();
        void DeactivateAllHandlers();

        void ActivateHandler(Guid hid);
        void DeactivateHandler(Guid hid);
        
    }
}
