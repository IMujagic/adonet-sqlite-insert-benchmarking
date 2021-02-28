using Sqlite.Benchmarking.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sqlite.Benchmarking
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("***** TESTING SQLite PERFORMANCE ******");

            var sampleData = SampleDataModel.Generate(1000);
            
            Console.WriteLine($"Running insert for {sampleData.Count} items.");

            var t1 = Task.Run(() =>
            {
                Console.WriteLine("Start test without transaction.");
                Console.WriteLine($"Elapsed time for default approach: {Run(new RepositoryWithoutTransaction(Path.Combine(Environment.CurrentDirectory, "SqliteBenchmark.sqlite")), sampleData)} seconds.");
            });

            var t2 = Task.Run(() =>
            {
                Console.WriteLine("Start test with transaction.");
                Console.WriteLine($"Elapsed time for transaction approach: {Run(new RepositoryWithTransaction(Path.Combine(Environment.CurrentDirectory, "SqliteBenchmark2.sqlite")), sampleData)} seconds.");
            });

            Task.WaitAll(t1, t2);

            Console.WriteLine("Test finished");
        }

        private static double Run(IRepository repository, IEnumerable<SampleDataModel> data)
        {
            var sp = new Stopwatch();
            sp.Start();

            repository.Insert(data.ToList());

            sp.Stop();

            return sp.Elapsed.TotalSeconds;
        }
    }
}
