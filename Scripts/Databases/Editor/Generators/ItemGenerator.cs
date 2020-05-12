using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Databases.AutoGenerates {
    public class ItemGenerator : DatabaseTemplateGeneratorBase {
        public ItemGenerator(string outputFolderPath, string outputFilePath, string databaseName) 
            : base(outputFolderPath, outputFilePath, databaseName) {
        }

        private const string TypeNameBoolean = "BOOLEAN";

        protected override string FileName
        {
            get
            {
                return "ItemTemplate";
            }
        }

        public List<DBColumn> ColumnList { get; set; }

        public string TableName { get; set; }

        protected override string ReplaceTemplates(string fileTexts) {
            fileTexts = base.ReplaceTemplates(fileTexts);
            fileTexts = fileTexts.Replace("##COLUMN_PROPERTIES##", GenerateColumnProperty());
            fileTexts = fileTexts.Replace("##COLUMN_INITIALIZE##", GenerateColumnInitializer());
            fileTexts = fileTexts.Replace("##COPY_CONSTRUCTOR##", GenerateCopyConstructor());
            return fileTexts.Replace("#TABLENAME#", TableName);
        }

        Dictionary<string, string> typeReplaceDict = new Dictionary<string, string>() {
            { "INTEGER" , "int" },
            { "TEXT"    , "string" },
            { "REAL"    , "double" },
            { TypeNameBoolean , "bool" },
            { "VARCHAR" , "string" },
        };

        private string GenerateColumnProperty() {
            string propertyString = "";
            foreach (var column in ColumnList) {
                if (!typeReplaceDict.ContainsKey(column.TypeName.ToUpper())) {
                    continue;
                }
                propertyString = propertyString + string.Format("        public {0} {1} {{ get; private set; }}\r\n",typeReplaceDict[column.TypeName.ToUpper()] , column.KeyName.ToCamelCase());
            }
            return propertyString;
        }

        private string GenerateColumnInitializer() {
            string propertyString = "";
            for (int i = 0 ; i < ColumnList.Count ; i++) {
                if (!typeReplaceDict.ContainsKey(ColumnList[i].TypeName.ToUpper())) {
                    continue;
                }

                if (ColumnList[i].TypeName == TypeNameBoolean)
                {
                    propertyString = propertyString + string.Format("            {0} = (long)datas[{1}] == 1;\r\n", ColumnList[i].KeyName.ToCamelCase(), i);
                }
                else
                {
                    propertyString = propertyString + string.Format("            {0} = ({1})datas[{2}];\r\n", ColumnList[i].KeyName.ToCamelCase(), typeReplaceDict[ColumnList[i].TypeName.ToUpper()], i);
                }
            }
            return propertyString;
        }

        private string GenerateCopyConstructor()
        {
            string propertyString = "";
            foreach (var column in ColumnList) {
                if (!typeReplaceDict.ContainsKey(column.TypeName.ToUpper())) {
                    continue;
                }
                propertyString = propertyString + string.Format("            {0} = other.{0};\r\n", column.KeyName.ToCamelCase());
            }
            return propertyString;
        }
    }
}
