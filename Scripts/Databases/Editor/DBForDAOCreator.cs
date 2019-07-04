using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Databases.AutoGenerates {
    public class DBForDAOCreator : SqliteDatabase {

        string dbName;
        string resourcePath;

        protected override string DatabasePath
        {
            get
            {
                return "URI=file:" + Application.persistentDataPath + "/" + dbName + ".bytes";
            }
        }

        protected override string ResourceFilePath
        {
            get
            {
                return resourcePath;
            }
        }

        public DBForDAOCreator(string dbName , string resourcePath) {
            this.dbName = dbName;
            this.resourcePath = resourcePath;
            Init();
        }
    }
}
