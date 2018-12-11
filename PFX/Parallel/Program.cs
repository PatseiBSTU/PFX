using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace ParallelClass
{
    class Program
    {
        static void Main(string[] args)
        {


           Parallel.For(1, 10, (int z, ParallelLoopState pd) =>
            {
                Console.WriteLine(z);
                int r = 1;
                for (int y = 1; y <= 10; y++)
                {
                    //  if (y == 5) pd.Stop(); 
                    r *= z;
                }

            });
            Console.WriteLine("Stop");


            ParallelLoopResult result = Parallel.For(1, 10, n => {
                int r = 1;
                for (int i = 1; i < n; i++) r *= i;

                Console.WriteLine(r);
            });
            if (!result.IsCompleted)
                Console.WriteLine($"Выполнение цикла завершено на итерации {result.LowestBreakIteration}");


            /////////////////////////////////
            Parallel.For(1, 10, i => Console.WriteLine($"{i}," +
                                    $"{Task.CurrentId} , " +
                                    $" {Thread.CurrentThread.ManagedThreadId}"));
            Parallel.ForEach(Partitioner.Create(0, 5), i => Console.WriteLine($"{i}," +
                                     $"{Task.CurrentId} , " +
                                     $" {Thread.CurrentThread.ManagedThreadId}"));

            


        }
    }
}
