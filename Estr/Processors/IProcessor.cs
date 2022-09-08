using System.Net.Sockets;

namespace Estr.Processor
{
    interface IProcessor
    {
        public NetworkStream ContextStream {get;}    
        void Process();
         
    }
}
