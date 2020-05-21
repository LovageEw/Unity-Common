using System.Collections.Generic;
using System.Text;

namespace Databases.AutoGenerates
{
    public class EnumGenerator : DatabaseTemplateGeneratorBase
    {
        public EnumGenerator(string outputFolderPath, string outputFilePath, string databaseName)
            : base(outputFolderPath, outputFilePath, databaseName)
        {
        }

        protected override string FileName => "EnumTemplate";
        public string EnumName { get; set; }
        public List<object[]> Items { get; set; }

        protected override string ReplaceTemplates(string fileTexts)
        {
            fileTexts = fileTexts.Replace("#ENUMNAME#", EnumName);
            fileTexts = fileTexts.Replace("##DEFINITIONS##", CreateEnumMembers());
            return base.ReplaceTemplates(fileTexts);
        }

        private string CreateEnumMembers()
        {
            var stringBuilder = new StringBuilder();
            foreach (var item in Items)
            {
                stringBuilder.Append($"        {item[1]} = {item[0]},\n");
            }

            return stringBuilder.ToString();
        }
    }
}