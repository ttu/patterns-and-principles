using System;
using System.Collections.Generic;
using System.Text;

namespace PatternsAndPrinciples.Patterns.GoF.Creational
{
    // Intended purpose of the class containing a factory method is not to create objects

    public interface IHttpResponse
    { }

    public interface IHttpRequestHandler
    {
        // .. all other methods ..
        IHttpResponse CreateResponse(int httpStatusCode);
    }
}
