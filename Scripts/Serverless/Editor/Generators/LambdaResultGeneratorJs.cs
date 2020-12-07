using Networks;

namespace Common.Scripts.Serverless.Editor.Generators
{
    public class LambdaResultGeneratorJs : LambdaResultGeneratorBase
    {
        protected override string FileName => "NetworkingResultTemplateJs";

        public LambdaResultGeneratorJs(ILambdaModel lambdaModel, string outputFolderPath)
            : base(lambdaModel, outputFolderPath)
        {
        }
    }
}