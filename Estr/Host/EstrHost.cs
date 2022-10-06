using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Estr.Handlers;
using Estr.Parsers;
using Estr.Processor;

namespace Estr.Host{

    public class EstrHost
    {
        internal readonly IHandler _handler;
        internal readonly IProcessor _processor;

        public EstrHost(IHandler handler)
        {
            _handler = handler;
        }

        public EstrHost(IProcessor processor) =>
            _processor = processor;

        
        
        public async Task StartAsync()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 80);
            
            tcpListener.Start();
            System.Console.WriteLine("Server started !");

            while(true)
            {
                var client = await tcpListener.AcceptTcpClientAsync();
                var _ = ProcessClientAsync(client);                
            }
        }

        
        private async Task ProcessClientAsync(TcpClient client)
        {
            try
            {
                using (client)
                using (var stream = client.GetStream())
                using (var reader = new StreamReader(stream))
                {

                    var head = await reader.ReadLineAsync();
                    for (string line = null; line != string.Empty; line = await reader.ReadLineAsync());
                
                    //Console.WriteLine(head);

                    var request = RequestParser.Parse(head);
                    Console.WriteLine($"{request.DomainRoute}");
                    //await _handler.HandleAsync(stream, request);
                }
            }
            catch(Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }
    }
}
