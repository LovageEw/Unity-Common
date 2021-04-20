using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Databases.AutoGenerates
{
    public class PayloadGenerator : DatabaseTemplateGeneratorBase
    {
        public PayloadGenerator(string outputFolderPath, string outputFilePath, string databaseName) : base(
            outputFolderPath, outputFilePath, databaseName)
        {
        }

        protected override string FileName => "PayloadTemplate";
        public List<DBColumn> ColumnList { get; set; }
        public string TableName { get; set; }

        protected override string ReplaceTemplates(string fileTexts)
        {
            fileTexts = base.ReplaceTemplates(fileTexts);
            fileTexts = fileTexts.Replace("##COLUMN_FIELDS##", GenerateColumnFields());
            fileTexts = fileTexts.Replace("##COLUMN_INITIALIZE##", GeneratePayloadInitializer());
            fileTexts = fileTexts.Replace("##COLUMNS##", GenerateColumns());
            return fileTexts.Replace("#TABLENAME#", TableName);
        }

        private string GenerateColumnFields()
        {
            var sb = new StringBuilder();
            foreach (var column in ColumnList.Where(x => typeReplaceDict.ContainsKey(x.TypeName.ToUpper())))
            {
                sb.AppendLine($"        public {typeReplaceDict[column.TypeName.ToUpper()]} {column.KeyName.ToCamelCase()};");
            }
            return sb.ToString();
        }
        
        private string GeneratePayloadInitializer()
        {
            var sb = new StringBuilder();
            foreach (var column in ColumnList.Where(x => typeReplaceDict.ContainsKey(x.TypeName.ToUpper())))
            {
                sb.AppendLine($"            {column.KeyName.ToCamelCase()} = model.{column.KeyName.ToCamelCase()};");
            }
            return sb.Remove(sb.Length - 2, 2).ToString();
        }

        private string GenerateColumns()
        {
            var sb = new StringBuilder();
            foreach (var column in ColumnList.Where(x => typeReplaceDict.ContainsKey(x.TypeName.ToUpper())))
            {
                sb.Append($"{column.KeyName.ToCamelCase()}, ");
            }
            return sb.Remove(sb.Length - 2, 2).ToString();
        }
    }
}