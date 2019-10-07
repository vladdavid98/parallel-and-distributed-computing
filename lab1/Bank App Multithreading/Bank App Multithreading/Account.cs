using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App_Multithreading
{
    internal class Account
    {
        public Account(string accountName, int newBalance, int accountId)
        {
            this.AccountName = accountName;
            this.AccountBalance = newBalance;
            AccountId = accountId;
            this.OperationLog = new OperationLog();
        }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public int AccountBalance { get; set; }
        internal OperationLog OperationLog { get; set; }

        public override string ToString()
        {
            string s = "";
            s += "Account ";
            s += AccountName;
            s += " has a balance of ";
            s += AccountBalance;
//            s += " and operation log: ";
//            s += "\n";
//            s += OperationLog.ToString();
            return s;
        }

    }
}
