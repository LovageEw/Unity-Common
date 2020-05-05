using System;

namespace Databases.Transaction
{
    [Serializable]
    public class ResponseHeader
    {
        public long statusCode;
        public string body;
    }
}