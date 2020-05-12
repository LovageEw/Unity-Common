using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Scripts.Serverless.Editor.Generators;
using Networks;
using UnityEditor;
using UnityEngine;

namespace Common.Scripts.Serverless.Editor
{
    public class LambdaFunctionCreator : EditorWindow
    {
        [MenuItem("Tools/Lambda/Create AWS Lambda Functions")]
        private static void ShowWindow()
        {
            var window = GetWindow<LambdaFunctionCreator>();
            window.titleContent = new GUIContent("Lambda Function Creator");
            window.Show();
        }

        private DefaultAsset outputDirectory = null;
        
        private void OnGUI()
        {
            EditorGUILayout.HelpBox(
                "Create AWS Lambda access resulting class by declarations in which implemented ILambdaModel.",
                MessageType.Info, true);
            outputDirectory = (DefaultAsset)EditorGUILayout.ObjectField(
                "Output Directory", outputDirectory, typeof(DefaultAsset), true);

            if (outputDirectory == null) {return;}
            GUILayout.Space(15);
            if (GUILayout.Button("Execute", GUILayout.Width(100))) {
                Execute();
            }
        }

        private void Execute()
        {
            var models = GetTargetModels();
            GenerateModelClass(models);

            AssetDatabase.Refresh();
        }

        private IEnumerable<object> GetTargetModels()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies();
            return types
                .SelectMany(x => x.GetTypes())
                .Where(x => x.Name == "GetUser")
                .Where(x => x.GetTypeInfo().ImplementedInterfaces.Contains(typeof(ILambdaModel)))
                .Select(Activator.CreateInstance);
            
        }

        private void GenerateModelClass(IEnumerable<object> models)
        {
            foreach (var model in models.Select(x => x as ILambdaModel))
            {
                var generator = new LambdaResultGenerator(model, AssetDatabase.GetAssetPath(outputDirectory));
                generator.GenerateClass();
            }
        }
    }
}