using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PrjTake.Messages;
using PrjTake.Utilities.Math;
using PrjTake.Communications.Tcp;
using PrjTake.Storage;

namespace PrjTake
{
    class Program
    {
        internal static DatabaseManager Manager;

        static void Main(string[] args)
        {
            NewSocket C = new NewSocket();
            C.Server();

            Manager = new DatabaseManager(
                new DatabaseServer("localhost",
                    3306,
                    "root",
                    ""),
                    new Database("yuccadb",
                        5,
                        15));

            while (true)
                Console.Read();
        }
    }
}
