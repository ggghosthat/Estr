using System.Net;
using System.Net.Sockets;
using Estr.Handlers;
using System.Collections;
using System;

namespace Estr.Processor
{
    internal struct Processor : IProcessor
    {
        private Hashtable _handlers = new();


        public Processor(){}

        public void Process(TcpClient client)              
        {
        }

        public void RegistHandler(IHandler handler)
        {
            if (!_handlers.Contains(handler.HandlerId))
                _handlers.Add(handler.HandlerId, handler);

        }

        public void RemoveHandler(Guid hid)
        {
            if (_handlers.Contains(hid))
                _handlers.Remove(hid);
        }

        public void RemoveAllHandlers()
        {
	    _handlers.Clear();
        }

        public void ActivateAllHandlers()
        {
	    foreach (IHandler handler in _handlers.Values)
		handler.Activate();
        }

        public void DeactivateAllHandlers()
        {
	    foreach (IHandler handler in _handlers.Values)
		handler.Deactivate();
        }

        public void ActivateHandler(Guid hid)
        {
	    ((IHandler)_handlers[hid]).Activate();
        }

        public void DeactivateHandler(Guid hid)
        {
	    ((IHandler)_handlers[hid]).Deactivate();
        }
    }
}
