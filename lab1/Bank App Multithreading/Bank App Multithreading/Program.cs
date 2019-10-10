using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace Bank_App_Multithreading
{
    internal class Program
    {
        private static List<Account> _accounts;
        private static readonly Mutex mut = new Mutex();
        private static Random _random = new Random();

        private static Operation MakeOperation(int accountNrSender, int accountNrReceiving, int moneyTransferred)
        {
            _accounts[accountNrSender].AccountBalance -= moneyTransferred;
            _accounts[accountNrReceiving].AccountBalance += moneyTransferred;
            return new Operation(_accounts[accountNrSender],_accounts[accountNrReceiving], moneyTransferred);
        }

        private static void DoManyOperations(int howManyTimes)
        {
            // Initialize random operation
            int randomNumber1 = _random.Next(0, _accounts.Count());
            int randomNumber2 = _random.Next(0, _accounts.Count());
            int randomNumber3 = _random.Next(0, 1000);


            for (int i = 0; i <= howManyTimes; i++)
            {
                // Force threads to make many operations so performance benefits can be seen more easily
                for (int j = 0; j <= 5000; j++)
                {
                    // Make sure the random numbers adhere to the rules
                    while (randomNumber1 == randomNumber2 || randomNumber3 > _accounts[randomNumber1].AccountBalance)
                    {
                        randomNumber1 = _random.Next(0, _accounts.Count());
                        randomNumber2 = _random.Next(0, _accounts.Count());
                        randomNumber3 = _random.Next(0, 1000);
                    }
                }

                mut.WaitOne();
                if (i % 1000 == 0)
                {
                    if(ConsistencyCheck()==false)throw new ApplicationException("Consistency check returned a problem.");
                }
                
                Operation op = MakeOperation(randomNumber1,randomNumber2,randomNumber3);
                _accounts[randomNumber1].OperationLog.AddToLog(op);
                _accounts[randomNumber2].OperationLog.AddToLog(op);
                mut.ReleaseMutex();
            }
        }

        private static void ShowAllAccounts()
        {
            for (int i = 0; i < _accounts.Count(); i++)
            {
                Console.WriteLine(_accounts[i].ToString());
            }
        }

        public static bool ConsistencyCheck()
        {
            // check if the amount of money in each account corresponds with the operations records associated to that account
            for (int i = 0; i < _accounts.Count(); i++)
            {
                if (_accounts[i].ConsistencyCheck() == false) return false;
            }

            return true;
        }


        private static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            const int numberOfThreads = 64;
            List<Thread> workerThreads = new List<Thread>();

            //            each thread can randomly transfer funds (make operations)
            Account acc1 = new Account("Vlad", 2000,1);
            Account acc2 = new Account("Natalia", 2000,2);
            Account acc3 = new Account("Bogdan", 2000,3);
            Account acc4 = new Account("Ileana", 2000,4);
            Account acc5 = new Account("Radu", 2000,5);

            _accounts = new List<Account>() {acc1,acc2,acc3,acc4,acc5};



            for(int i = 0; i < numberOfThreads; i++)
            {
                Thread myNewThread = new Thread(() => DoManyOperations(100000/numberOfThreads));
                workerThreads.Add(myNewThread);
                myNewThread.Start();
            }
            


            
            foreach (Thread thread in workerThreads)
            {
                thread.Join();
            }

            ShowAllAccounts();
            sw.Stop();
            Console.WriteLine("Elapsed={0}", sw.Elapsed);

            int s = 0;
            for (int i = 0; i < _accounts.Count(); i++)
            {
                s += _accounts[i].AccountBalance;
                
            }
            Console.WriteLine(s);

            Console.WriteLine("Final consistency check for all accounts: {0}",ConsistencyCheck());

            Console.ReadKey();
        }

        
    }
}
