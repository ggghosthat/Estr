using System.Net;
using System.Net.Sockets;
using Estr.Handlers;
using System.Collections;
using System.Collections.Generics;
using System;
using System.Linq;

namespace Estr.Processor
{
    public struct Processor : IProcessor
    {
        private Hashtable _handlers = new();
        private readonly IDictionary<string, Guid> _domains = new Dictionary<string, Guid>();


        public Processor(){}

        public void Process(string domainRoute)              
        {
        }


        public void RegistHandler(IHandler handler)
        {
            if (!_handlers.Contains(handler.HandlerId))
            {
                _handlers.Add(handler.HandlerId, handler);
                _domains.Add(handler.DomainName, handler.HandlerId);
            }
        }


        public void RemoveHandler(Guid hid)
        {
            if (_handlers.Contains(hid))
            {
                _handlers.Remove(hid);
                
                string item = _domains.First(dmn => dmn.Value.Equals(hid));
                domains.Remove(item.Key);
            }
        }

        public void RemoveAllHandlers()
        {
    	    _handlers.Clear();
            _domains.Clear();
        }




        //Activate/Deactivate area.

        public void ActivateAllHandlers()
        {
	        foreach (IHandler handler in _handlers.Values)
		        handler.Activate();

            BuildDomainStore();
        }

        public void DeactivateAllHandlers()
        {
	        foreach (IHandler handler in _handlers.Values)
		        handler.Deactivate();

            _domains.Clear();
        }

        public void ActivateHandler(Guid hid)
        {
	        ((IHandler)_handlers[hid]).Activate();

            ActivateDomain(((IHandler)_handlers[hid]).DomainName,
                           ((IHandler)_handlers[hid]).HandlerId);
        }

        public void DeactivateHandler(Guid hid)
        {
	        ((IHandler)_handlers[hid]).Deactivate();

             string item = _domains.First(dmn => dmn.Value.Equals(hid));
             domains.Remove(item.Key);
        }




        private void BuildDomainStore()
        {
            if (_handlers.Count > 0)
            {
	            foreach (IHandler handler in _handlers.Values)
                    _domains.Add(handler.DomainName, handler.HandlerId);
            }
        }

        private void ActivateDomain(string domain, Guid hid)
        {
            _domains.Add(domain, hid);
        }
    }
}
