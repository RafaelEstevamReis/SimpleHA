using Simple.API;
using System;

namespace Simple.HAApi.Exceptions
{
    public class ClientExeption : Exception
    {
        public Response Info { get; }

        public ClientExeption(string message, Response info)
            : base(message)
        {
            Info = info;
        }
    }
}
