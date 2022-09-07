using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Reflection;
using Estr.Controllers;
using Estr.Parsers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Estr.Handlers{
    public class ControllerHandler : IHandler
    {
        private readonly Dictionary<string, Func<object>> _routs;

        public ControllerHandler(Assembly controllerAssembly)
        {
            this._routs = controllerAssembly.GetTypes()
                .Where(x => typeof(IController).IsAssignableFrom(x))
                .SelectMany(Controller => Controller.GetMethods().Select(Method => 
                    new 
                    {
                        Controller,
                        Method
                    }
                ))
                .ToDictionary(key => GetPath(key.Controller, key.Method),
                              value => GetEndpointMethod(value.Controller, value.Method));
        }

        private Func<object> GetEndpointMethod(Type controller, MethodInfo method)
        {
            return () => method.Invoke(Activator.CreateInstance(controller), Array.Empty<object>());
        }

        private string GetPath(Type controller, MethodInfo method)
        {
            string name = controller.Name;

            if (name.EndsWith("controller", StringComparison.InvariantCultureIgnoreCase))
                name = name.Substring(0,name.Length - "controller".Length);
            if (name.Equals("Index", StringComparison.InvariantCultureIgnoreCase))
                return "/" + name;
            
            return "/" + name + "/" + method.Name;
        }

        public void Handle(Stream stream, Request request)
        {
            if (!_routs.TryGetValue(request.Path, out var func))
                ResponseWriter.WriteStatusCode(HttpStatusCode.NotFound, stream);
            else
            {
                ResponseWriter.WriteStatusCode(HttpStatusCode.OK, stream);
                WriteControllerResponse(func(), stream);
            }
        }

        public async Task HandleAsync(Stream stream, Request request)
        {
            if (!_routs.TryGetValue(request.Path, out var func))
                await ResponseWriter.WriteStatusCodeAsync(HttpStatusCode.NotFound, stream);
            else
            {
                await ResponseWriter.WriteStatusCodeAsync(HttpStatusCode.OK, stream);
                await WriteControllerResponseAsync(func(), stream);
            }
        }

        private void WriteControllerResponse(object response, Stream stream)
        {
            if (response is string str)
            {
                using var writer = new StreamWriter(stream, leaveOpen:true);
                writer.Write(str);
            }
            else if (response is byte[] buffer)
            {
                stream.Write(buffer, 0, buffer.Length);
            }
            else
            {
                WriteControllerResponse(JsonConvert.SerializeObject(response), stream);
            }
        }

        private async Task WriteControllerResponseAsync(object response, Stream stream)
        {
            if (response is string str)
            {
                using var writer = new StreamWriter(stream, leaveOpen:true);
                writer.Write(str);
            }
            else if (response is byte[] buffer)
            {
                stream.Write(buffer, 0, buffer.Length);
            }
            else if (response is Task task)
            {
                await task;
                await WriteControllerResponseAsync(JsonConvert.SerializeObject(task.GetType().GetProperty("Result").GetValue(task)), stream);
            }
            else
            {
                await WriteControllerResponseAsync(JsonConvert.SerializeObject(response), stream);
            }
        }
    }
}