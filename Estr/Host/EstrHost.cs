using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Estr.Handlers;
using Estr.Parsers;

namespace Estr.Host{

    public class EstrHost
    {
        internal readonly IHandler _handler;

        public EstrHost(IHandler handler)
        {
            _handler = handler;
        }

        public void StartSingleThread()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 80);
            tcpListener.Start();
            System.Console.WriteLine("Server started !");

            while(true)
            {
                try
                {
                    using(var client = tcpListener.AcceptTcpClient())
                    using (var stream = client.GetStream())
                    using (var reader = new StreamReader(stream))
                    {

                        var head = reader.ReadLine();
                        for (string line = null; line != string.Empty; line = reader.ReadLine());
                        
                        var request = RequestParser.Parse(head);
                        _handler.Handle(stream, request);
                    }
                }
                catch(Exception ex)
                {
                    System.Console.WriteLine(ex);
                }
            }
        }

        public void StartThreadPool()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 80);
            
            tcpListener.Start();
            System.Console.WriteLine("Server started !");

            while(true)
            {
                var client = tcpListener.AcceptTcpClient();
                ProcessClient(client);
            }
        }

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

        private void ProcessClient(TcpClient client)
        {
            ThreadPool.QueueUserWorkItem(o => 
            {
                try{
                    using(client)
                    using (var stream = client.GetStream())
                    using (var reader = new StreamReader(stream))
                    {

                        var head = reader.ReadLine();
                        for (string line = null; line != string.Empty; line = reader.ReadLine());
                        
                        var request = RequestParser.Parse(head);
                        _handler.Handle(stream, request);
                    }
                }
                catch(Exception ex)
                {
                    System.Console.WriteLine(ex);
                }
            });
        }

        private async Task ProcessClientAsync(TcpClient client)
        {
            try
            {
                using(client)
                using (var stream = client.GetStream())
                using (var reader = new StreamReader(stream))
                {

                    var head = await reader.ReadLineAsync();
                    for (string line = null; line != string.Empty; line = await reader.ReadLineAsync());
                
                    var request = RequestParser.Parse(head);
                    await _handler.HandleAsync(stream, request);
                }
            }
            catch(Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }
    }
}