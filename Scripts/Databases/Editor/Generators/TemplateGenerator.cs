using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System;
using System.Text;

namespace Databases.AutoGenerates {

    public abstract class TemplateGenerator {

        protected abstract string FileName { get; }

        protected string DatabaseName { get; private set; }

        string templatePath;
        string outputPath;

        public TemplateGenerator(string outputFolderPath, string outputFilePath, string databaseName) {
            outputPath = outputFolderPath + "/" + outputFilePath;
            DatabaseName = databaseName;

            var allFiles = Directory.GetFiles(Directory.GetCurrentDirectory() , "*.template" , SearchOption.AllDirectories);
            templatePath = allFiles.FirstOrDefault(x => x.Contains(FileName + ".template"));
        }

        public void GenerateClass() {
            var originalRegion = PreserveRegion("Class-specific Codes");
            var usingRegion = PreserveRegion("Additional usings");
            
            CopyFile();

            ReplaceFileString(ReplaceTempletes);
            ReplaceFileString((str) => {
                str = str.Replace("#DATABASENAME#", DatabaseName);
                str = str.Replace("##USINGS##", usingRegion);
                return str.Replace("##ORIGINAL##", originalRegion);
            });

            ReplaceFileString(UnifyEscape);
        }

        protected abstract string ReplaceTempletes(string fileTexts);

        protected string PreserveRegion(string regionName) {
            if (!File.Exists(outputPath)) {
                return "";
            }
            string texts = "";
            using (StreamReader sr = new StreamReader(outputPath , Encoding.UTF8)) {
                texts = sr.ReadToEnd();
            }
            return JoinByEscape( 
                texts.Replace("\r\n", "\n").Split('\n')
                .SkipWhile(x => !x.Contains(regionName)).Skip(1)
                .TakeWhile(x => !x.Contains(regionName))
            );
        }

        private string[] DivideByEscape(string str) {
            return str.Replace("\r\n", "\n").Split('\n');
        }

        private string JoinByEscape(IEnumerable<string> strArray) {
            if (strArray.Count() == 0) {
                return "";
            }else if(strArray.Count() == 1) {
                return strArray.ElementAt(0) + "";
            }
            return strArray.Aggregate((a, b) => a + "\n" + b);
        }

        void CopyFile() {
            UnityConsole.Log("Create File :"  + outputPath);
            if (File.Exists(outputPath)) {
                File.Delete(outputPath);
            }
            File.Copy(templatePath, outputPath);
        }
        
        private void ReplaceFileString(Func<string, string> replaceFunc) {
            string texts = "";
            using (StreamReader sr = new StreamReader(outputPath , Encoding.UTF8)) {
                texts = sr.ReadToEnd();
            }
            texts = replaceFunc(texts);
            using (StreamWriter sw = new StreamWriter(outputPath)) {
                sw.Write(texts);
            }
        }

        private string UnifyEscape(string str) {
#if UNITY_EDITOR_WIN
            return str.Replace("\n", "\r\n").Replace("\r\r\n", "\r\n");
#else
            return str.Replace("\r\n", "\n");
#endif
        }
    }
}
