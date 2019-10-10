using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App_Multithreading
{
    internal class Operation
    {
        public int SerialNr { get; set; }

        public static int GlobalOperationSerialNr { get; set; } = 0;
        internal int SenderAccId { get; set; }
        internal int RecipientAccId { get; set; }
        public int TransactionValue { get; set; }

        public Operation(Account sender, Account recipient, int transVal)
        {
            SerialNr = ++GlobalOperationSerialNr;
            SenderAccId = sender.AccountId;
            RecipientAccId = recipient.AccountId;
            TransactionValue = transVal;
        }

        public override string ToString()
        {
            string s = "";
            s += "Operation: ";
            s += SerialNr.ToString();
            s += " sender: ";
            s += SenderAccId;
            s += " recipient: ";
            s += RecipientAccId;
            s += " value: ";
            s += TransactionValue;
            return s;
        }





    }
}
