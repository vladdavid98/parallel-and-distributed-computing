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

        private static Operation MakeRandomOperation()
        {
//            get random account number
            int randomNumber1 = _random.Next(0, _accounts.Count()); 
            int randomNumber2 = _random.Next(0, _accounts.Count());
            int randomNumber3 = _random.Next(0, 1000);

            while (randomNumber1 == randomNumber2 || randomNumber3 > _accounts[randomNumber1].AccountBalance)
            {
                
                randomNumber1 = _random.Next(0, _accounts.Count());
                randomNumber2 = _random.Next(0, _accounts.Count());
                
            }

//            mut.WaitOne();
            _accounts[randomNumber1].AccountBalance -= randomNumber3;
            _accounts[randomNumber2].AccountBalance += randomNumber3;
//            mut.ReleaseMutex();

            //            accounts[randomNumber1].OperationLog.AddToLog(this);
            return new Operation(_accounts[randomNumber1],_accounts[randomNumber2],randomNumber3);

        }

        private static void DoOperations(int howManyTimes)
        {
            for (int i = 0; i <= howManyTimes; i++)
            {
                Operation op = MakeRandomOperation();
                _accounts[0].OperationLog.AddToLog(op);
                _accounts[1].OperationLog.AddToLog(op);
            }
        }

        private static void ShowAllAccounts()
        {
            for (int i = 0; i < _accounts.Count(); i++)
            {
                Console.WriteLine(_accounts[i].ToString());
            }
        }


        private static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            const int numberOfThreads = 10;
            List<Thread> workerThreads = new List<Thread>();

            //            each thread can randomly transfer funds (make operations)
            Account acc1 = new Account("Vlad", 2000,1);
            Account acc2 = new Account("Natalia", 2000,2);
//            Account acc3 = new Account("Bogdan", 2000);
//            Account acc4 = new Account("Ileana", 2000);
//            Account acc5 = new Account("Radu", 2000);

            _accounts = new List<Account>() {acc1,acc2};



            for(int i = 0; i < numberOfThreads; i++)
            {
                Thread myNewThread = new Thread(() => DoOperations(62000/numberOfThreads));
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

            Console.ReadKey();


            

            
        }

        
    }
}
