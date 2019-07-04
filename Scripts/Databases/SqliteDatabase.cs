using Mono.Data.SqliteClient;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Databases {
    public abstract class SqliteDatabase : IClientDatabase
    {
        private IDbConnection connection = null;
        private IDbCommand command = null;
        private IDataReader reader = null;

        protected abstract string ResourceFilePath { get; }
        protected abstract string DatabasePath { get; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public virtual void Init() {
            TextAsset resource = Resources.Load(ResourceFilePath.RemoveString(".bytes")) as TextAsset;
            if (IsRequireReload(resource)) {
                CopyDatabase(resource);
            }

            SetupDatabase();
        }

        void SetupDatabase() {
            UnityConsole.Log(string.Format("Open Database ({0}).", this.GetType().FullName));
            connection = new SqliteConnection(DatabasePath);
            command = connection.CreateCommand();
        }

        public bool IsRequireReload(TextAsset textAsset) {
            string databaseFilePath = DatabasePath.RemoveString("URI=file:");
            if (!File.Exists(databaseFilePath)) {
                return true;
            }
            using (FileStream fs = File.Open(databaseFilePath , FileMode.Open)) {
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                if (!data.SequenceEqual(textAsset.bytes)) {
                    UnityConsole.Log("Differ.");
                    return true;
                } else {
                    UnityConsole.Log("Same.");
                }
            }
            return false;
        }

        void CopyDatabase(TextAsset textAsset) {
            string databaseFilePath = DatabasePath.RemoveString("URI=file:");
            File.Delete(databaseFilePath);
            
            using (FileStream fs = File.Create(databaseFilePath)) {
                fs.Write(textAsset.bytes, 0, textAsset.bytes.Length);
            }
            UnityConsole.Log("Resource DB Copied.\nPath : " + databaseFilePath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dao"></param> 
        /// <returns></returns>
        public List<object[]> SelectAll(DAOBase dao) {
            var items = new List<object[]>();

            connection.Open();
            
            command.CommandText = "select * from " + dao.TableName +" ;";
            reader = command.ExecuteReader();
            while (reader.Read()) {
                var item = Enumerable.Range(0, dao.ColumnCount)
                    .Select(reader.GetValue)
                    .ToArray();
                items.Add(item);
            }
            reader.Close();
            connection.Close();

            return items;
        }


        /// <summary>
        /// 
        /// </summary>
        ~SqliteDatabase() {
            Close();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Close() {
            UnityConsole.Log(string.Format("Close Database ({0})." , this.GetType().FullName));
            if (reader != null && !reader.IsClosed) {
                reader.Close();
            }
            reader = null;

            if (command != null) {
                command.Dispose();
            }
            command = null;

            if (connection != null && connection.State != ConnectionState.Closed) {
                connection.Close();
            }
            connection = null;
        }
    }
}