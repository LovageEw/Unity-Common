using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Databases.AutoGenerates {
    public class ManagerGenerator : DatabaseTemplateGeneratorBase {
        public ManagerGenerator(string outputFolderPath, string outputFilePath, string databaseName)
            : base(outputFolderPath, outputFilePath, databaseName) {
        }

        protected override string FileName
        {
            get
            {
                return "ManagerTemplate";
            }
        }

        public List<string> TableNames { get; set; }


        protected override string ReplaceTemplates(string fileTexts) {
            fileTexts = base.ReplaceTemplates(fileTexts);
            fileTexts = fileTexts.Replace("##DAO_PROPERTIES##", GenerateDAOProperty());
            return fileTexts.Replace("##DAO_INIT_REGION##" , TableNames.Select(GenerateInitCode).Aggregate((a,b) => a + b));
        }

        private string GenerateDAOProperty() {
            string propertyString = "";
            foreach (var tableName in TableNames) {
                propertyString = propertyString + string.Format("        public {0}DAO {0}DAO {{ get; private set; }}\r\n", tableName.ToCamelCase());
            }
            return propertyString;
        }

        string GenerateInitCode(string tableName) {
            return
                "            " + tableName.ToCamelCase() + "DAO = new " + tableName.ToCamelCase() + "DAO();\r\n" +
                "            " + tableName.ToCamelCase() + "DAO.Init("+ DatabaseName + "DB);\r\n";
        }
    }
}
