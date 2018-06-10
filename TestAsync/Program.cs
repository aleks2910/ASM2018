using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start.");
            Console.WriteLine("{0}", Thread.CurrentThread.ManagedThreadId);
            //var t = new S1().Run();

            var t = new S2().Run();

            var t3 = new S3().Run2();


            Task.WaitAll(t, t3);

            Console.WriteLine("Finish!!!!!!!!!");
            Console.ReadKey();
        }
    }


    class S1
    {
        /// ТУТ ВСЕ СИНХРОННО
        async Task myMethod()
        {
            int sum = 0;
            Console.WriteLine("выполнился цикл2 - {0}", Thread.CurrentThread.ManagedThreadId);
            await SomeCycleAsync();
            Console.WriteLine("выполнился цикл2");
        }
        async Task SomeCycleAsync()
        {
            var myTask = await ResultOfCycle();

            Console.WriteLine("выполнился цикл1  {0}", Thread.CurrentThread.ManagedThreadId);
        }
        async Task<int> ResultOfCycle()
        {
            Console.WriteLine("Start LONG cicle- {0}", Thread.CurrentThread.ManagedThreadId);

            int sum = 0;
            for (int i = 0; i < 1000000000; i++)
            {
                sum += i;
            }
            return sum;
        }


        public async Task Run()
        {
            await myMethod();
        }
    }

    class S2
    {

        async Task myMethod()
        {
            Console.WriteLine("начало -  {0}", Thread.CurrentThread.ManagedThreadId);
            await SomeCycleAsync();
            Console.WriteLine("выполнился цикл-2 -  {0}", Thread.CurrentThread.ManagedThreadId);
        }

        async Task SomeCycleAsync()
        {
            Console.WriteLine("стартует цикл -  {0}", Thread.CurrentThread.ManagedThreadId);
            // это запускает длинное вычисление на пуле потоков
            //var result = await Task.Run(ResultOfCycle);

            var result = await Task.Run(() =>
            {
                return ResultOfCycle();
            });

            Log.Add("выполнился цикл, результат: " + result);
        }

        int ResultOfCycle()
        {
            Console.WriteLine("Цикл -  {0}", Thread.CurrentThread.ManagedThreadId);
            int sum = 0;
            for (int i = 0; i < 1000000000; i++)
                sum += i;
            return sum;
        }

        public async Task Run()
        {
            await myMethod();
        }
    }


    class S3
    {

        public async Task DisplayResultAsync2()
        {
            int num = 5;
            int result = -1;
            var task = FactorialAsync(num);
            Log.Add("Task initialized");
            Thread.Sleep(1000);
            Log.Add("after sleep");
            try
            {
                result = await task;
            }
            catch (Exception ex)
            {
                Log.Add("Исключение: " + ex.Message);
                Log.Add("IsFaulted: " + task.IsFaulted);
            }
            Log.Add(String.Format("Факториал числа {0} равен {1}", num, result));

            num = 6;
            result = FactorialAsync(num).GetAwaiter().GetResult();
            Log.Add(String.Format("Факториал числа {0} равен {1}", num, result));

            result = await Task.Run(() =>
            {
                Log.Add("SUmming factorials");
                int res = 1;
                for (int i = 1; i <= 9; i++)
                {
                    res += i * i;
                }
                return res;
            });
            Log.Add(String.Format("Сумма квадратов чисел равна {0}", result));


        }

        public  Task Run2()
        {
            return DisplayResultAsync2();
            //Log.Add("Finished Run2 - S3");           
        }


        public void Run()
        {
            DisplayResultAsync();
            Log.Add("Finished");
            Console.ReadLine();
        }

        async Task DisplayResultAsync()
        {
            Log.Add("Disp result start");
            int num = 5;

            int result = await FactorialAsync(num);
            Thread.Sleep(3000);
            Log.Add(String.Format("Факториал числа {0} равен {1}", num, result));
            Console.WriteLine();
        }

        Task<int> FactorialAsync(int x)
        {
            Log.Add("> FactorialAsync - start");
            int result = 1;

            Task<int> res = Task.Run(() =>
            {
                Log.Add("> COMPUTING");
                for (int i = 1; i <= x; i++)
                {
                    result *= i;
                }
                return result;
            });


            Log.Add("Facts  continues");
            return res;
        }
    }

}

