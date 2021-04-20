using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;

namespace Databases.AutoGenerates
{
    public class DAOCreator : DAOBase
    {
        public override int ColumnCount => 5;
        public override string TableName => "sqlite_master";

        public string ResourcePath { get; internal set; }
        public List<string> TableNameList { get; } = new List<string>();
        public DatabaseType DatabaseType { get; set; }

        string outputFolderName;
        string databaseName;

        public void Execute(string outputFolderName, string databaseName, IClientDatabase db)
        {
            this.outputFolderName = outputFolderName;
            this.databaseName = databaseName;

            Init(db);
        }

        protected override void InitTableItems(List<object[]> values)
        {
            ExecuteGenerateClass(values);
        }

        private void ExecuteGenerateClass(List<object[]> values)
        {
            var managerGen = DatabaseType == DatabaseType.WebGL 
                ? new WebGLManagerGenerator(outputFolderName, databaseName + "DBManager.cs", databaseName)
                : new ManagerGenerator(outputFolderName, databaseName + "DBManager.cs", databaseName);

            var queries = values.Where(x => x[4] != null).Select(x =>
            {
                try
                {
                    return ((string) x[4]);
                }
                catch (InvalidCastException)
                {
                    return "";
                }
            }).Where(x => x.ToUpper().Contains("CREATE TABLE"));
            foreach (var query in queries)
            {
                var columnList = new List<DBColumn>();
                var queryList = query.RemoveString("[", "]", "\n", "\r", "\"").Replace("\t", ",").Split(',')
                    .SelectMany(x => x.Split('(')).SelectMany(x => x.Split(')')).ToArray();
                string tableName = GetTableName(queryList);
                foreach (var column in MakeColumnList(queryList))
                {
                    var columnData = column.SkipWhile(string.IsNullOrEmpty).ToArray();
                    if (columnData.Length < 2) { continue; }

                    columnList.Add(new DBColumn() {KeyName = columnData[0], TypeName = columnData[1]});
                }

                if (columnList.Count > 0)
                {
                    string camelTableName = tableName.ToCamelCase();
                    TableNameList.Add(camelTableName);
                    GenerateDaoClass(tableName, columnList.Count());
                    GenerateItemClass(camelTableName, columnList);
                    if (DatabaseType == DatabaseType.WebGL)
                    {
                        GeneratePayloadClass(camelTableName, columnList);
                    }
                }
            }

            if (TableNameList.Count > 0)
            {
                managerGen.TableNames = TableNameList;
                managerGen.GenerateClass();
            }
        }

        private void GenerateDaoClass(string tableName, int columnCount)
        {
            var daoGen = new DAOGenerator(outputFolderName, tableName.ToCamelCase() + "DAO.cs", databaseName,
                columnCount, tableName);
            daoGen.GenerateClass();
        }

        private void GenerateItemClass(string tableName, List<DBColumn> dbColumns)
        {
            var itemGen = new ItemGenerator(outputFolderName, tableName + ".cs", databaseName);
            itemGen.TableName = tableName;
            itemGen.ColumnList = dbColumns;
            itemGen.GenerateClass();
        }
        
        private void GeneratePayloadClass(string tableName, List<DBColumn> dbColumns)
        {
            var itemGen = new PayloadGenerator(outputFolderName, tableName + "Payload.cs", databaseName);
            itemGen.TableName = tableName;
            itemGen.ColumnList = dbColumns;
            itemGen.GenerateClass();
        }
        
        string GetTableName(IEnumerable<string> queryList)
        {
            return queryList.First(x => x.ToUpper().Contains("CREATE TABLE")).Split(' ')
                .SkipWhile(x => !x.ToUpper().Contains("TABLE")).ElementAt(1);
        }

        IEnumerable<string[]> MakeColumnList(IEnumerable<string> queryList)
        {
            var array = queryList as string[] ?? queryList.ToArray();
            var exceptList = array.Where(x => !x.ToUpper().Contains(" TEXT"))
                .Where(x => !x.ToUpper().Contains(" INTEGER")).Where(x => !x.ToUpper().Contains(" REAL"))
                .Where(x => !x.ToUpper().Contains(" BOOLEAN")).Where(x => !x.ToUpper().Contains(" VARCHAR"));
            return array.Except(exceptList).Select(x => x.Split(' '));
        }
    }
}