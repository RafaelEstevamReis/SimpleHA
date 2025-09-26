namespace Simple.HAApi.Exceptions;

using Simple.API;
using System;

public class ClientException : Exception
{
    public Response Info { get; }

    public ClientException(string message, Response info)
        : base(message)
    {
        Info = info;
    }
}
