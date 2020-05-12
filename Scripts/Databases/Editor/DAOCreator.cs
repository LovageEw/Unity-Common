using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;

namespace Databases.AutoGenerates {
    public class DAOCreator : DAOBase {

        public override int ColumnCount { get { return 5; } }

        public override string TableName { get { return "sqlite_master"; } }

        public string ResourcePath { get; internal set; }

        string outputFolderName;
        string databaseName;

        public void Execute(string outputFolderName , string databaseName, IClientDatabase db) {
            this.outputFolderName = outputFolderName;
            this.databaseName = databaseName;

            Init(db);
        }


        protected override void InitTableItems(List<object[]> values) {
            ExecuteGenerateClass(values);
        }

        private void ExecuteGenerateClass(List<object[]> values) {
            var managerGen = new ManagerGenerator(outputFolderName , databaseName + "DBManager.cs" , databaseName);
            var tableNameList = new List<string>();

            var queries = values.Where(x => x[4] != null)
                .Select(x => {
                    try {
                        return ((string)x[4]);
                    } catch (InvalidCastException) {
                        return "";
                    }
                }).Where(x => x.ToUpper().Contains("CREATE TABLE"));
            foreach (var query in queries) {
                List<DBColumn> columnList = new List<DBColumn>();
                var queryList = query
                    .RemoveString("[", "]", "\n", "\r", "\"")
                    .Replace("\t",",")
                    .Split(',')
                    .SelectMany(x => x.Split('('))
                    .SelectMany(x => x.Split(')'));
                string tableName = GetTableName(queryList);
                foreach (var column in MakeColumnList(queryList)) {
                    var columnData = column.SkipWhile(string.IsNullOrEmpty).ToArray();
                    if (columnData.Length < 2) { continue; }

                    columnList.Add(new DBColumn() { KeyName = columnData[0], TypeName = columnData[1] });
                }

                if (columnList.Count > 0) {
                    string camelTableName = tableName.ToCamelCase();
                    tableNameList.Add(camelTableName);
                    GenerateDAOClass(tableName, columnList.Count());
                    GenerateItemClass(camelTableName, columnList);
                }
            }

            if (tableNameList.Count > 0) {
                managerGen.TableNames = tableNameList;
                managerGen.GenerateClass();
            }
        }

        private void GenerateDAOClass(string tableName, int columnCount) {
            var daoGen = new DAOGenerator(outputFolderName, tableName.ToCamelCase() + "DAO.cs", databaseName, columnCount , tableName);
            daoGen.GenerateClass();
        }

        private void GenerateItemClass(string tableName , List<DBColumn> dbColumns) {
            var itemGen = new ItemGenerator(outputFolderName, tableName + ".cs", databaseName);
            itemGen.TableName = tableName;
            itemGen.ColumnList = dbColumns;
            itemGen.GenerateClass();
        }

        string GetTableName(IEnumerable<string> queryList) {
            return queryList
                .First(x => x.ToUpper().Contains("CREATE TABLE"))
                .Split(' ')
                .SkipWhile(x => !x.ToUpper().Contains("TABLE"))
                .ElementAt(1);
        }

        IEnumerable<string[]> MakeColumnList(IEnumerable<string> queryList) {
            var exceptList = queryList
                .Where(x => !x.ToUpper().Contains(" TEXT"))
                
                .Where(x => !x.ToUpper().Contains(" INTEGER"))
                .Where(x => !x.ToUpper().Contains(" REAL"))
                .Where(x => !x.ToUpper().Contains(" BOOLEAN"))
                .Where(x => !x.ToUpper().Contains(" VARCHAR"));
            return queryList.Except(exceptList).Select(x => x.Split(' '));
        }
    }
}
