using System;

namespace ZEMS.Web.AppException
{
    public class ModelStateException : Exception 
    {
        public ModelStateException(string message) : base(message)
        { 
        }
    }
}
