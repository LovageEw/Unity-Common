using System;

namespace Networks
{
    public interface ILambdaModel
    {
        string LambdaName { get; }
        Type LambdaBody { get; }
        Type LambdaResult { get; }
    }
}