using System;
using System.Runtime.Remoting.Messaging;
using static System.Console;
using static System.Threading.Thread;

namespace AsyncCallbackDelegate
{
    public delegate int BinaryOp(int x, int y);

    class Program
    {
        private static bool isDone = false;

        static void Main()
        {
            WriteLine("*****  AsyncCallbackDelegate Example *****");
            WriteLine($"Main() invoked on thread {CurrentThread.ManagedThreadId}.");

            BinaryOp b = new BinaryOp(Add);
            IAsyncResult ar = b.BeginInvoke(10, 10,
              new AsyncCallback(AddComplete),
              "Main() thanks you for adding these numbers.");

            // Assume other work is performed here...
            while (!isDone)
            {
                Sleep(1000);
                WriteLine("Working....");
            }

            ReadLine();
        }

        // Target for AsyncCallback delegate
        // Don't forget to add a 'using' directive for 
        // System.Runtime.Remoting.Messaging!
        static void AddComplete(IAsyncResult iar)
        {
            WriteLine($"AddComplete() invoked on thread {CurrentThread.ManagedThreadId}.");
            WriteLine("Your addition is complete");

            // Now get the result.
            AsyncResult ar = (AsyncResult)iar;
            BinaryOp b = (BinaryOp)ar.AsyncDelegate;
            WriteLine($"10 + 10 is {b.EndInvoke(iar)}.");

            // Retrieve the informational object and cast it to string.
            string msg = (string)iar.AsyncState;
            WriteLine(msg);

            isDone = true;
        }

        //Target for BinaryOp delegate
        static int Add(int x, int y)
        {
            WriteLine($"Add() invoked on thread {CurrentThread.ManagedThreadId}.");
            Sleep(5000);
            return x + y;
        }
    }
}