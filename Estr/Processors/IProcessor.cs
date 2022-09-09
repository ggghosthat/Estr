using System.Net.Sockets;
using System.Net;
using Estr.Handlers;

namespace Estr.Processor
{
    interface IProcessor
    {
        void Process(TcpClient client);
        void RegistHandler(IHandler handler);
        void RemoveHandler(string hid);

        void RemoveAllHandlers();

        void ActivateAllHandlers();
        void DeactivateAllHandlers();

        void ActivateHandler(string hid);
        void DeactivateHandler(string hid);
        
    }
}
