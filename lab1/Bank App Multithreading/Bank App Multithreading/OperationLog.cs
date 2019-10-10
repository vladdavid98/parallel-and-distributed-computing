using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App_Multithreading
{
    internal class OperationLog
    {
        private readonly List<Operation> _log;

        public OperationLog() => _log = new List<Operation>();

        public void AddToLog(Operation op)
        {
            _log.Add(op);
        }

        public int Count()
        {
            return _log.Count();
        }

        public Operation GetOperationFromLog(int operationPosition)
        {
            return _log[operationPosition];
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < _log.Count(); i++)
            {
                s += _log[i].ToString();
                s += "\n";
            }
            s += "\n\n";
            return s;
        }
    }
}
