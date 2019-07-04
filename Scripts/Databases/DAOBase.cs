using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Databases {
    public abstract class DAOBase {

        public abstract int ColumnCount { get; }
        public abstract string TableName { get; }

        protected abstract void InitTableItems(List<object[]> values);

        protected IClientDatabase database;

        public virtual void Init(IClientDatabase database) {
            this.database = database;
            var values = database.SelectAll(this);
            InitTableItems(values);
        }
    }
}