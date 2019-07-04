using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Databases.AutoGenerates {
    public class DAOGenerator : TemplateGenerator {
        public DAOGenerator(string outputFolderPath, string outputFilePath, string databaseName, int columnCount, string tableName) 
            : base(outputFolderPath, outputFilePath, databaseName) {
            ColumnCount = columnCount;
            TableName = tableName;
        }

        protected override string FileName
        {
            get
            {
                return "DAOTemplate";
            }
        }

        public int ColumnCount { get; private set; }
        public string TableName { get; private set; }

        protected override string ReplaceTempletes(string fileTexts) {
            fileTexts = fileTexts.Replace("#TABLENAMEORIGINAL#", TableName);
            fileTexts = fileTexts.Replace("#TABLENAME#", TableName.ToCamelCase());
            fileTexts = fileTexts.Replace("#STABLENAME#", TableName.ToCamelCase().ToLower());
            return fileTexts.Replace("#COLUMNCOUNT#", ColumnCount.ToString());
        }
    }
}
