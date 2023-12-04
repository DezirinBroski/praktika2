using System;
using System.Collections.Generic;
using System.Threading;

public class Dikar
{
    private string name;
    private string className;
    private string function;
    private ThreadPriority priority;
    private Semaphore semaphore;
    private Random random;
    private object lockObject = new object();

    public Dikar(string name, string className, string function, ThreadPriority priority, Semaphore semaphore)
    {
        this.name = name;
        this.className = className;
        this.function = function;
        this.priority = priority;
        this.semaphore = semaphore;
        this.random = new Random();
    }

    public void Eat()
    {
        Console.WriteLine($"{name} ({className}) хочет есть.");

        Console.WriteLine($"{name} ({className}) пытается войти в критическую секцию.");
        lock (lockObject)
        {
            Console.WriteLine($"{name} ({className}) вошел в критическую секцию.");

            semaphore.WaitOne(); // Запрашиваем доступ к горшку

            Console.WriteLine($"{name} ({className}) начал есть.");

            // Имитация обеда
            Thread.Sleep(random.Next(1000, 3000));

            Console.WriteLine($"{name} ({className}) закончил есть.");

            semaphore.Release(); // Освобождаем горшок

            Console.WriteLine($"{name} ({className}) освободил горшок.");
        }
    }

    public void Start()
    {
        Thread thread = new Thread(Eat);
        thread.Name = name;
        thread.Priority = priority;

        Console.WriteLine($"{name} ({className}) создан. Функция: {function}. Приоритет: {priority}");

        thread.Start();
    }
}

public class Cook
{
    private Semaphore semaphore;
    private int portions;
    private object lockObject = new object();

    public Cook(Semaphore semaphore, int portions)
    {
        this.semaphore = semaphore;
        this.portions = portions;
    }

    public void CookPortions()
    {
        while (true)
        {
            Console.WriteLine("Повар проверяет горшок.");

            if (portions > 0)
            {
                lock (lockObject)
                {
                    Console.WriteLine($"Повар готовит порцию. Осталось порций: {portions}");
                    portions--;
                }
            }
            else
            {
                lock (lockObject)
                {
                    Console.WriteLine("Горшок пуст. Повар идет спать");

                    Thread.Sleep(5000); // Имитация приготовления новой порции

                    portions = 200;

                    Console.WriteLine($"Повар наполнил горшок порциями. Осталось порций: {portions}");
                }
            }

            Thread.Sleep(1000);
        }
    }

    public void Start()
    {
        Thread thread = new Thread(CookPortions);
        thread.Name = "Cook";
        thread.Priority = ThreadPriority.BelowNormal;

        Console.WriteLine($"Повар создан.");

        thread.Start();
    }
}

public class DiningModel
{
    private Semaphore semaphore;
    private List<Dikar> dikars;
    private Cook cook;

    public DiningModel()
    {
        semaphore = new Semaphore(4, 4); // Максимальное количество дикарей, которые могут есть из горшка одновременно

        dikars = new List<Dikar>();
        dikars.Add(new Dikar("Дикарь 1", "Дикарь", "Поедание", ThreadPriority.Normal, semaphore));
        dikars.Add(new Dikar("Дикарь 2", "Дикарь", "Поедание", ThreadPriority.Normal, semaphore));
        dikars.Add(new Dikar("Дикарь 3", "Дикарь", "Поедание", ThreadPriority.Normal, semaphore));
        dikars.Add(new Dikar("Дикарь 4", "Дикарь", "Поедание", ThreadPriority.Normal, semaphore));
        dikars.Add(new Dikar("Дикарь 5", "Дикарь", "Поедание", ThreadPriority.Normal, semaphore));
        dikars.Add(new Dikar("Дикарь 6", "Дикарь", "Поедание", ThreadPriority.Normal, semaphore));
        dikars.Add(new Dikar("Дикарь 7", "Дикарь", "Поедание", ThreadPriority.Normal, semaphore));
        dikars.Add(new Dikar("Дикарь 8", "Дикарь", "Поедание", ThreadPriority.Normal, semaphore));
        dikars.Add(new Dikar("Дикарь 9", "Дикарь", "Поедание", ThreadPriority.Normal, semaphore));
        dikars.Add(new Dikar("Дикарь 10", "Дикарь", "Поедание", ThreadPriority.Normal, semaphore));
        dikars.Add(new Dikar("Дикарь 11", "Дикарь", "Поедание", ThreadPriority.Normal, semaphore));
        dikars.Add(new Dikar("Дикарь 12", "Дикарь", "Поедание", ThreadPriority.Normal, semaphore));
        dikars.Add(new Dikar("Дикарь 13", "Дикарь", "Поедание", ThreadPriority.Normal, semaphore));
        dikars.Add(new Dikar("Дикарь 14", "Дикарь", "Поедание", ThreadPriority.Normal, semaphore));
        dikars.Add(new Dikar("Дикарь 15", "Дикарь", "Поедание", ThreadPriority.Normal, semaphore));
        dikars.Add(new Dikar("Дикарь 16", "Дикарь", "Поедание", ThreadPriority.Normal, semaphore));
        dikars.Add(new Dikar("Дикарь 17", "Дикарь", "Поедание", ThreadPriority.Normal, semaphore));
        dikars.Add(new Dikar("Дикарь 18", "Дикарь", "Поедание", ThreadPriority.Normal, semaphore));

        cook = new Cook(semaphore, 200);
    }

    public void Start()
    {
        foreach (var dikar in dikars)
        {
            dikar.Start();
        }

        cook.Start();
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        DiningModel diningModel = new DiningModel();
        diningModel.Start();

        Console.ReadLine();
    }
}