using System;
using System.Net;
using System.IO;
using Estr.Parsers;
using System.Threading.Tasks;

namespace Estr.Handlers{
    public class StaticFileHandler : IHandler
    {
        private readonly string _path;

        public Guid HandlerId {get; init;} = Guid.NewGuid();

        public string DomainName {get; init;} = "StaticDomain";

        public bool IsActive {get; private set;}

        public StaticFileHandler(string path)
        {
            _path = path;
        }

        public void Handle(Stream stream, Request request)
        {
            if (!IsActive)
                return;
            
            using (var writer = new StreamWriter(stream))
            {
                var completePath = Path.Combine(_path, request.Path.Substring(1));
                
                if (!File.Exists(completePath))
                {
                    ResponseWriter.WriteStatusCode(HttpStatusCode.NotFound, stream);
                    return;
                }

                ResponseWriter.WriteStatusCode(HttpStatusCode.OK, stream);
                using (var fileStream = File.OpenRead(completePath))
                {
                    fileStream.CopyTo(stream);
                }

                Console.WriteLine(completePath);
            }
        }

        public async Task HandleAsync(Stream stream, Request request)
        {
            if (!IsActive)
                return;

            using (var writer = new StreamWriter(stream))
            {
                var completePath = Path.Combine(_path, request.Path.Substring(1));
                
                if (!File.Exists(completePath))
                {
                    await ResponseWriter.WriteStatusCodeAsync(HttpStatusCode.NotFound, stream);
                    return;
                }
                
                await ResponseWriter.WriteStatusCodeAsync(HttpStatusCode.OK, stream); 
                using (var fileStream = File.OpenRead(completePath))
                {
                    await fileStream.CopyToAsync(stream);
                }

                Console.WriteLine(completePath);
            }
        }   

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
        
    }
}
