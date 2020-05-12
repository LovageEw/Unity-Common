using System.Collections.Generic;
using Commons.Editors.AutoGenerates;

namespace Databases.AutoGenerates
{
    public abstract class DatabaseTemplateGeneratorBase : TemplateGenerator
    {
        protected string DatabaseName { get; private set; }
        
        public DatabaseTemplateGeneratorBase(string outputFolderPath, string outputFilePath, string databaseName)
            : base(outputFolderPath, outputFilePath)
        {
            DatabaseName = databaseName;
        }

        protected override string ReplaceTemplates(string fileTexts)
        {
            return fileTexts.Replace("#DATABASENAME#", DatabaseName);
        }

        protected override IEnumerable<(string, string)> GetPreservedRegion()
        {
            yield return ("##USINGS##", "Additional usings");
            yield return ("##COPYCONST##", "Additional Copy Constructor");
            yield return ("##ORIGINAL##", "Class-specific Codes");
        }
    }
}