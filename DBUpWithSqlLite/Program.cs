using System;
using System.Reflection;
using System.Diagnostics;
using DbUp;
using DbUp.Engine;
using DbUp.SQLite.Helpers;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Data.SQLite;

namespace DBUpWithSqlLite
{
    class Program
    {
        static void Main()
        {
            SQLiteConnection.CreateFile("./mydb.db");

            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = "./mydb.db",
                
            };
                var upgrader =
                    DeployChanges.To
                    .SQLiteDatabase(connectionStringBuilder.ConnectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

                var watch = new Stopwatch();
                watch.Start();

                var result = upgrader.PerformUpgrade();

                watch.Stop();
                Display("File", result, watch.Elapsed);

            
        }

        static void Display(string dbType, DatabaseUpgradeResult result, TimeSpan ts)
        {
            // Display the result
            if (result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success!");
                Console.WriteLine("{0} Database Upgrade Runtime: {1}", dbType,
                    String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10));
                Console.ReadKey();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ReadKey();
                Console.WriteLine("Failed!");
            }
        }
    }
}
