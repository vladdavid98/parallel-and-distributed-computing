using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App_Multithreading
{

    class Account
    {
        String accountName;
        Balance accountBalance;

        public Account(String accountName)
        {
            this.accountName = accountName;
            this.accountBalance = new Balance();
            doStuff();
        }

        void doStuff()
        {
            Console.WriteLine("stuff");
        }
        
    }

    public class Balance
    {
        
    }

    public class Operation
    {

    }








    class Program
    {
        static void Main(string[] args)
        {
            Account acc1 = new Account("Vlad");







            Console.ReadKey();
        }
    }
}
