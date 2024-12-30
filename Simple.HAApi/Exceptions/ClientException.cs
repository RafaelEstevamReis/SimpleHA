using Simple.API;
using System;

namespace Simple.HAApi.Exceptions
{
    public class ClientException : Exception
    {
        public Response Info { get; }

        public ClientException(string message, Response info)
            : base(message)
        {
            Info = info;
        }
    }
}
