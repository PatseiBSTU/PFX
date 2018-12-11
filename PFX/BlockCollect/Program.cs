using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlockCollect
{
    class Program
    {
        static int x = 0;
        static void Main(string[] args)
        {   
            
            BlockingCollection<int> blockcoll = new BlockingCollection<int>();
            for (int producer = 0; producer < 5; producer++)
            {
                Task.Factory.StartNew(() =>
                {    x++;
                    for (int ii = 0; ii < 3; ii++)
                    {
                        x++;
                        Thread.Sleep(100);
                        int id =  x;
                        blockcoll.Add(id);
                        Console.WriteLine("Produser  add " + id);
                    }
                });
            }

            Task consumer = Task.Factory.StartNew(
                    () =>
                    {
                        foreach (var item in blockcoll.GetConsumingEnumerable())
                        {
                            Console.WriteLine(" Reading " +item);
                        }
                    });
            consumer.Wait();

        }


    }
}
