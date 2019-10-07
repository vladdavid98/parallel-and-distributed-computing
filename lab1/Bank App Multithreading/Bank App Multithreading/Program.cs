using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace Bank_App_Multithreading
{
    internal class Program
    {
        private static List<Account> accounts;
        private static Mutex mut = new Mutex();


        private static Operation MakeRandomOperation()
        {
            
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            var byteArray = new byte[4];
            provider.GetBytes(byteArray);

//            convert 4 bytes to an integer
            int randomInteger = BitConverter.ToInt32(byteArray, 0);
            if (randomInteger < 0) randomInteger *= -1;


            Random random = new Random();


//            get random account number
            int randomNumber1 = random.Next(0, accounts.Count()); 
            int randomNumber2 = random.Next(0, accounts.Count());
            int randomNumber3 = randomInteger%100;

            while (randomNumber1 == randomNumber2 || randomNumber3 > accounts[randomNumber1].AccountBalance)
            {
                
                randomNumber1 = random.Next(0, accounts.Count());
                randomNumber2 = random.Next(0, accounts.Count());
                
            }

            mut.WaitOne();
            accounts[randomNumber1].AccountBalance -= randomNumber3;
            accounts[randomNumber2].AccountBalance += randomNumber3;
            mut.ReleaseMutex();

            //            accounts[randomNumber1].OperationLog.AddToLog(this);
            return new Operation(accounts[randomNumber1],accounts[randomNumber2],randomNumber3);

        }

        private static void DoOperations(int howManyTimes)
        {
            for (int i = 0; i <= howManyTimes; i++)
            {
                Operation op = MakeRandomOperation();
                accounts[0].OperationLog.AddToLog(op);
                accounts[1].OperationLog.AddToLog(op);
            }
        }

        private static void ShowAllAccounts()
        {
            for (int i = 0; i < accounts.Count(); i++)
            {
                Console.WriteLine(accounts[i].ToString());
            }
        }


        private static void Main(string[] args)
        {
            const int numberOfThreads = 10;
            
//            each thread can randomly transfer funds (make operations)
            Account acc1 = new Account("Vlad", 2000,1);
            Account acc2 = new Account("Natalia", 2000,2);
//            Account acc3 = new Account("Bogdan", 2000);
//            Account acc4 = new Account("Ileana", 2000);
//            Account acc5 = new Account("Radu", 2000);

            accounts = new List<Account>() {acc1,acc2};



            for(int i = 0; i < numberOfThreads; i++)
            {
                Thread myNewThread = new Thread(() => DoOperations(10000/numberOfThreads));
                myNewThread.Start();
            }


            Thread.Sleep(2000);

            
            ShowAllAccounts();

            Console.ReadKey();
        }

        
    }
}
