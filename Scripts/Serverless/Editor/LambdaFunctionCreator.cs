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

        private void OnGUI()
        {
            
        }
    }
}