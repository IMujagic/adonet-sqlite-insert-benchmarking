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
        static void Main(string[] args)
        {
            var outputDirectory = Environment.CurrentDirectory;
            var sampleSizes = new int[] { 1, 10, 100, 1000 };
            var results = new List<(string RepositoryName, int SampleSize, double elapsedTime)>();

            Console.WriteLine("***** TESTING SQLite INSERT PERFORMANCE ******");
            Console.WriteLine(Environment.NewLine);
            
            foreach(var sampleSize in sampleSizes)
            {
                var sampleData = SampleDataModel.Generate(sampleSize);

                Console.WriteLine($"Running insert for {sampleData.Count} items.");

                Task.WaitAll(
                    Task.Run(() =>
                    {
                        var elapsedTime = Run(new RepositoryWithoutTransaction(Path.Combine(outputDirectory, "SqliteBenchmarkDefault.sqlite")), sampleData);
                        results.Add((nameof(RepositoryWithoutTransaction), sampleSize, elapsedTime));
                    }),
                    Task.Run(() =>
                    {
                        var elapsedTime = Run(new RepositoryWithTransaction(Path.Combine(outputDirectory, "SqliteBenchmarkWTransaction.sqlite")), sampleData);
                        results.Add((nameof(RepositoryWithTransaction), sampleSize, elapsedTime));
                    }),
                    Task.Run(() =>
                    {
                        var elapsedTime = Run(new RepositoryWithTransactionReusedCommand(Path.Combine(outputDirectory, "SqliteBenchmarkWTransactionReusedCommand.sqlite")), sampleData);
                        results.Add((nameof(RepositoryWithTransactionReusedCommand), sampleSize, elapsedTime));
                    }));
            }

            Console.WriteLine("Test finished....");

            foreach(var resultGroup in results.GroupBy(x => x.SampleSize))
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine($"Result for sample size: {resultGroup.Key}");

                foreach(var groupResult in resultGroup.OrderBy(x => x.elapsedTime))
                {
                    Console.WriteLine($"Approach {groupResult.RepositoryName} for sample size {groupResult.SampleSize} was running for {groupResult.elapsedTime} seconds.");
                }
            };
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
