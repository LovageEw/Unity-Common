using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Databases.AutoGenerates {

    public class DAOCreatorWindow : EditorWindow {

        [MenuItem("Tools/SQLite/Create data access objects" )]
        static void Open() {
            GetWindow<DAOCreatorWindow>("DAO Generator");
        }

        DefaultAsset textField = null;
        TextAsset database = null;
        DatabaseType databaseType = DatabaseType.Master;

        private void OnGUI() {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("  DAO Generator Setting");
            GUILayout.Space(10);

            database = (TextAsset)EditorGUILayout.ObjectField("Database File", database, typeof(TextAsset), true);
            databaseType = (DatabaseType)EditorGUILayout.EnumPopup("Type", (System.Enum)databaseType);
            textField = (DefaultAsset)EditorGUILayout.ObjectField("Output Folder", textField, typeof(DefaultAsset), true);
            if (textField != null) {
                string path = AssetDatabase.GetAssetOrScenePath(textField);
                string[] folderList = path.Split('/');
                if (folderList[folderList.Length - 1].Contains(".")) {
                    textField = null;
                }
            }

            if (database != null && textField != null) {
                GUILayout.Space(15);
                if (GUILayout.Button("Execute", GUILayout.Width(100))) {
                    Execute();
                }
            }
        }

        private void Execute() {
            string databaseName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(database));
            string assetPath = AssetDatabase.GetAssetPath(database).RemoveString("Assets/Resources/");
            CreateAccessor(databaseName , assetPath);

            var db = new DBForDAOCreator(databaseName , assetPath);
            var dao = new DAOCreator();
            dao.ResourcePath = AssetDatabase.GetAssetPath(database).Split('/').SkipWhile(x => x != "Resources").Skip(1).Aggregate((a, b) => a + "\\" + b);

            dao.Execute(AssetDatabase.GetAssetPath(textField), databaseName, db);
        }

        void CreateAccessor(string databaseName , string assetPath) {
            var templater = new AccessorGenerator(AssetDatabase.GetAssetPath(textField), databaseName + "Database.cs" , databaseName);
            templater.DatabaseFileName = databaseName + ".bytes";
            templater.AssetPath = assetPath;
            templater.GenerateClass();
        }
    }
}
