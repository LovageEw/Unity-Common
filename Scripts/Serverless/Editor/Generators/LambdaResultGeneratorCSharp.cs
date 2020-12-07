using Networks;

namespace Common.Scripts.Serverless.Editor.Generators
{
    public class LambdaResultGeneratorCSharp : LambdaResultGeneratorBase
    {
        protected override string FileName => "NetworkingResultTemplateCsharp";

        public LambdaResultGeneratorCSharp(ILambdaModel lambdaModel, string outputFolderPath)
            : base(lambdaModel, outputFolderPath)
        {
        }
    }
}