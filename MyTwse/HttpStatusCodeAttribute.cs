using System;
using System.Net;

namespace MyTwse
{
    public class HttpStatusCodeAttribute : Attribute
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public HttpStatusCodeAttribute(HttpStatusCode statusCode)
        {
            HttpStatusCode = statusCode;
        }
    }
}