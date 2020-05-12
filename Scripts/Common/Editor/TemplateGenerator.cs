using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System;
using System.Text;

namespace Commons.Editors.AutoGenerates {

    public abstract class TemplateGenerator {

        protected abstract string FileName { get; }

        private readonly string templatePath;
        private readonly string outputPath;

        protected TemplateGenerator(string outputFolderPath, string outputFilePath) {
            outputPath = outputFolderPath + "/" + outputFilePath;

            var allFiles = Directory.GetFiles(Directory.GetCurrentDirectory() , "*.template" , SearchOption.AllDirectories);
            templatePath = allFiles.FirstOrDefault(x => x.Contains(FileName + ".template"));
        }

        public void GenerateClass() {
            var preserverRegionList = new Dictionary<string,string>();
            foreach (var (replacer, regionNotation) in GetPreservedRegion())
            {
                var preserved = PreserveRegion(regionNotation);
                preserverRegionList.Add(replacer, preserved);
            }
            
            CopyFile();

            ReplaceFileString(ReplaceTemplates);
            ReplaceFileString((str) =>
            {
                return preserverRegionList.Aggregate(str, (current, item) => current.Replace(item.Key, item.Value));
            });

            ReplaceFileString(UnifyEscape);
        }

        protected virtual IEnumerable<(string, string)> GetPreservedRegion()
        {
            yield return ("##USINGS##", "Additional usings");
            yield return ("##COPYCONST##", "Additional Copy Constructor");
            yield return ("##ORIGINAL##", "Class-specific Codes");
        }
        
        protected abstract string ReplaceTemplates(string fileTexts);

        private string PreserveRegion(string regionName) {
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

        private static string JoinByEscape(IEnumerable<string> strArray) {
            var array = strArray as string[] ?? strArray.ToArray();
            if (!array.Any()) {
                return "";
            }else if(array.Count() == 1) {
                return array.ElementAt(0) + "";
            }
            return array.Aggregate((a, b) => a + "\n" + b);
        }

        private void CopyFile() {
            UnityConsole.Log("Create File :"  + outputPath);
            if (File.Exists(outputPath)) {
                File.Delete(outputPath);
            }
            File.Copy(templatePath, outputPath);
        }
        
        private void ReplaceFileString(Func<string, string> replaceFunc) {
            string texts = "";
            using (var sr = new StreamReader(outputPath , Encoding.UTF8)) {
                texts = sr.ReadToEnd();
            }
            texts = replaceFunc(texts);
            using (var sw = new StreamWriter(outputPath)) {
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
