using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Databases.AutoGenerates {
    public class AccessorGenerator : TemplateGenerator {

        public AccessorGenerator(string outputFolderPath, string outputFilePath, string databaseName) 
            : base(outputFolderPath, outputFilePath, databaseName) {
        }

        public string DatabaseFileName { get; set; }
        public string AssetPath { get; set; }

        protected override string FileName
        {
            get
            {
                return "DatabaseTemplate";
            }
        }

        protected override string ReplaceTempletes(string fileTexts) {
            fileTexts = fileTexts.Replace("#DATABASE_FILENAME#" , DatabaseFileName);
            fileTexts = fileTexts.Replace("#ASSETPATH#", AssetPath);
            return fileTexts;
        }
    }
}
