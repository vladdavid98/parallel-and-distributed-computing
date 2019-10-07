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
        private readonly List<Operation> log;



        public OperationLog() => log = new List<Operation>();


        public void AddToLog(Operation op)
        {
            log.Add(op);
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < log.Count(); i++)
            {
                s += log[i].ToString();
                s += "\n";
            }
            s += "\n\n";
            return s;
        }
    }
}
