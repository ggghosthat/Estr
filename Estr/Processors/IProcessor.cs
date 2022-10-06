using System.Net.Sockets;
using System.Net;
using Estr.Handlers;
using System;

namespace Estr.Processor
{
    interface IProcessor
    {
        
        void Process(TcpClient client);
        void RegistHandler(IHandler handler);
        void RemoveHandler(Guid hid);

        void RemoveAllHandlers();

        void ActivateAllHandlers();
        void DeactivateAllHandlers();

        void ActivateHandler(Guid hid);
        void DeactivateHandler(Guid hid);
        
    }
}
