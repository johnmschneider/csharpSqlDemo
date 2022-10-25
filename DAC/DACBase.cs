using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADODB;

namespace jobDemo1.DAC
{
    public abstract class DACBase
    {
        protected internal Connection DbConnection { get; set; }
        protected internal Recordset mRecordSet { get; set; }
        protected internal string SqlQuery { get; set; }
        protected internal CursorTypeEnum CursorType { get; set; } = CursorTypeEnum.adOpenStatic;
        protected internal LockTypeEnum LockType { get; set; } = LockTypeEnum.adLockBatchOptimistic;

        protected internal DACBase(Connection dbConnection)
        {
            DbConnection = dbConnection;
        }

        public void Open()
        {
            mRecordSet = new Recordset();
            mRecordSet.Open(SqlQuery, DbConnection, CursorType, LockType, 0);
        }

        public abstract void Retrieve();

        public void Close()
        {
            mRecordSet.Close();
        }
    }
}
