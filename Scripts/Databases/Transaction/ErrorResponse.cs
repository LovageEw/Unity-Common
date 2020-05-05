using System;

namespace Databases.Transaction
{
    [Serializable]
    public class ErrorResponse
    {
        public int statusCode;
        public string message;
    }
}