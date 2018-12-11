using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PFX
{
    class Program
    {
        static void Main(string[] args)
        {
            // Запуск задач - способы
            int i = 10;
            Task task1 = new Task(() => 
                        { i++; Console.WriteLine("Task 1 finished"); });
            task1.Start();
            Task task2 = Task.Factory.StartNew(() =>
                        { ++i; Console.WriteLine("Task 2 finished"); });
            Task task3 = Task.Run(() => 
                        { ++i; Console.WriteLine("Task 3 finished"); });

            Console.WriteLine(i);
            Console.WriteLine("-----------------------------------------------------");
            // Синхронный запуск
            Action<object> method = x =>
                    { Thread.Sleep(1000); Console.WriteLine(x.ToString()); };
            var task4 = new Task(method, TaskCreationOptions.LongRunning);
            task4.RunSynchronously();

            // task3.Start(); // System.InvalidOperationException:
            //Start нельзя вызывать для уже запущенной задачи.
            Console.WriteLine("-----------------------------------------------------");
            //Wait
            Task.WaitAll(task1, task2, task3);
            Console.WriteLine(i);
            Console.WriteLine("-----------------------------------------------------");
            //Возврат результата
            Func<int> func = () =>
                       {
                           Thread.Sleep(1000);
                           return ++i;
                       };
            Task<int> task = new Task<int>(func);
            Console.WriteLine(task.Status);        // Created 
            task.Start();
            Console.WriteLine(task.Status);        // WaitingToRun 
            task.Wait();
            Console.WriteLine(task.Result);        // 14
            Console.WriteLine("Main going to finished");
            Console.WriteLine("-----------------------------------------------------");
            // Обработка исключений
            Task task5 = Task.Run(() =>
            {
                throw new Exception();
            });
            try
            {
                task5.Wait();
            }
            catch (AggregateException ex)
            {
                var message = ex.InnerException.Message;
                Console.WriteLine(message);
            }
            Console.WriteLine("-----------------------------------------------------");
            // Работа с токеном отмены
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            // используем  токен в двух задачах 
            var taskC1 = new Task(method, tokenSource.Token);
            var taskC2 = new Task(method, tokenSource.Token);
            taskC1.Start();
            taskC2.Start();
            //  отменяем  задачи 
            tokenSource.Cancel();
            Task.WaitAll(taskC1, taskC2);
            Console.WriteLine(" Статус " + taskC1.Status);
            Console.WriteLine(" Статус " + taskC2.Status);

            //Задача с продолжением
            Console.WriteLine("-----------------------------------------------------");
            Task task6 = Task.Run(() => Console.Write("Doing.."));
            Task task7 = task6.ContinueWith(t => Console.WriteLine("continuation"));
            Task.WaitAll(task6, task7);
            //продолжение с условием
            Console.WriteLine("-----------------------------------------------------");
            Task task8 = Task.Run(() => Console.Write("One...."));
            Task task9 = Task.Run(() => Console.Write("Two..."));
            Task continuation = Task.WhenAll(task8, task9).
                              ContinueWith(t => Console.WriteLine("Three...."));
            Task.WaitAll(task8, continuation);

            Console.WriteLine("-----------------------------------------------------");

            // работа с awaiter
            Task<int> what = Task.Run(() => Enumerable.Range(1, 100000)
                          .Count(n => (n % 2 == 0)));
            // получаем объект продолжения 
            var awaiter = what.GetAwaiter();
            // что делать после окончания предшественника 
            awaiter.OnCompleted(() =>
            {
                // получаем результат вычислений предшественника  
                int res = awaiter.GetResult();
                Console.WriteLine(res);
            });
            Thread.Sleep(4000);
            Console.WriteLine("Main finished");

        }
    }
}
