using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Databases.AutoGenerates
{
    public class DAOCreatorWindow : EditorWindow
    {
        [MenuItem("Tools/KakuriyoCommons/SQLite")]
        static void Open()
        {
            GetWindow<DAOCreatorWindow>("DAO Generator");
        }

        private DefaultAsset textField = null;
        private TextAsset database = null;
        private DatabaseType databaseType = DatabaseType.Master;
        private List<string> tableNameList;

        private void OnGUI()
        {
            database = (TextAsset) EditorGUILayout.ObjectField("Database File", database, typeof(TextAsset), true);
            GUILayout.Space(10);
            EditorGUILayout.LabelField("■ Create data access objects", new GUIStyle{fontStyle = FontStyle.Bold});
            GUILayout.Space(10);
            databaseType = (DatabaseType) EditorGUILayout.EnumPopup("Type", (System.Enum) databaseType);
            textField = (DefaultAsset) EditorGUILayout.ObjectField("Output Folder", textField, typeof(DefaultAsset),
                true);
            if (textField != null)
            {
                string path = AssetDatabase.GetAssetOrScenePath(textField);
                string[] folderList = path.Split('/');
                if (folderList[folderList.Length - 1].Contains("."))
                {
                    textField = null;
                }
            }

            if (database != null && textField != null)
            {
                GUILayout.Space(15);
                if (GUILayout.Button("Execute", GUILayout.Width(100)))
                {
                    Execute();
                }
            }
            
            GUILayout.Space(10);
            EditorGUILayout.LabelField("■ Create Enum definitions", new GUIStyle{fontStyle = FontStyle.Bold});
            if (tableNameList == null || database == null || textField == null)
            {
                tableNameList = null;
                EditorGUILayout.HelpBox(
                    "Before you run the Enum generation, you should run the DAO generation.",
                    MessageType.Info, true);
                return;
            }

            EditorGUILayout.LabelField("Tables");
            foreach (var table in tableNameList)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(table);
                GUILayout.Space(10);
                if (GUILayout.Button("Create Enum Definition", GUILayout.Width(150)))
                {
                    GenerateEnumDefinition(table);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void Execute()
        {
            string databaseName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(database));
            string assetPath = AssetDatabase.GetAssetPath(database).RemoveString("Assets/Resources/");
            CreateAccessor(databaseName, assetPath);

            var db = new DBForDAOCreator(databaseName, assetPath);
            var dao = new DAOCreator();
            dao.ResourcePath = AssetDatabase.GetAssetPath(database).Split('/').SkipWhile(x => x != "Resources").Skip(1)
                .Aggregate((a, b) => a + "\\" + b);

            dao.Execute(AssetDatabase.GetAssetPath(textField), databaseName, db);
            tableNameList = dao.TableNameList;

            AssetDatabase.Refresh();
        }
        
        private void GenerateEnumDefinition(string tableName)
        {
            string databaseName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(database));
            string assetPath = AssetDatabase.GetAssetPath(database).RemoveString("Assets/Resources/");
            var db = new DBForDAOCreator(databaseName, assetPath);

            var queryString = $"select id, name from {tableName};";
            var items = db.Query(queryString, 2);

            var outputPath = AssetDatabase.GetAssetPath(textField);
            var enumGenerator =
                new EnumGenerator(outputPath, tableName + "Id.cs", databaseName)
                {
                    Items = items, EnumName = tableName + "Id"
                };
            enumGenerator.GenerateClass();
            
            AssetDatabase.Refresh();
        }

        void CreateAccessor(string databaseName, string assetPath)
        {
            var templater = new AccessorGenerator(AssetDatabase.GetAssetPath(textField), databaseName + "Database.cs",
                databaseName);
            templater.DatabaseFileName = databaseName + ".bytes";
            templater.AssetPath = assetPath;
            templater.GenerateClass();
        }
    }
}