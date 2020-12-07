using System.Collections.Generic;
using System.Linq;
using Commons.Editors.AutoGenerates;
using Networks;

namespace Common.Scripts.Serverless.Editor.Generators
{
    
    public abstract class LambdaResultGeneratorBase : TemplateGenerator
    {
        private readonly string functionName;
        private readonly string modelName;
        private readonly string lambdaBody;
        private readonly ILambdaModel lambdaModel;
        
        public LambdaResultGeneratorBase(ILambdaModel lambdaModel, string outputFolderPath)
            : base(outputFolderPath, GetOutputFilePath(lambdaModel))
        {
            this.lambdaModel = lambdaModel;
            functionName = lambdaModel.LambdaName;
            modelName = lambdaModel.LambdaResult.Name;
            lambdaBody = lambdaModel.LambdaBody.Name;
        }

        private static string GetOutputFilePath(ILambdaModel lambdaModel)
        {
            return lambdaModel.LambdaName.ToCamelCase() + "NetworkingResult.cs";
        }

        protected override string ReplaceTemplates(string fileTexts)
        {
            fileTexts = fileTexts.Replace("#FunctionName#", functionName);
            fileTexts = fileTexts.Replace("#Model#", modelName);
            fileTexts = fileTexts.Replace("#ModelName#", functionName.ToCamelCase());
            fileTexts = fileTexts.Replace("#LambdaBody#", lambdaBody);
            fileTexts = fileTexts.Replace("#RequiredUsing#", GetRequiredUsing());
            return fileTexts;
        }

        private string GetRequiredUsing()
        {
            return new[] {lambdaModel.LambdaBody.Namespace, lambdaModel.LambdaResult.Namespace}
                .Distinct()
                .Select(x => "using " + x + ";")
                .Aggregate((a, b) => a + "\n" + b);
        }

        protected override IEnumerable<(string, string)> GetPreservedRegion()
        {
            foreach (var valueTuple in base.GetPreservedRegion())
            {
                yield return valueTuple;
            }
            yield return ("##Post-Deserialize##", "Additional post-Deserialize process");
            yield return ("##OnFunctionError##", "Define OnFunctionError");
        }
    }
}