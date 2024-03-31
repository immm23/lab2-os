namespace Lab2
{
    internal class Program
    {
        static Semaphore[] _forks = null!;

        static void Main(string[] args)
        {
            int numPhilosophers = 3;
            _forks = new Semaphore[numPhilosophers];
            for (int i = 0; i < numPhilosophers; i++)
            {
                // 1 person can use one fork at a time
                _forks[i] = new Semaphore(1, 1);
            }

            // start all tasks in new threads
            Thread[] philosophers = new Thread[numPhilosophers];
            for (int i = 0; i < numPhilosophers; i++)
            {
                philosophers[i] = new Thread(Philosopher);
                philosophers[i].Start(i);
            }

            foreach (Thread philosopher in philosophers)
            {
                philosopher.Join();
            }
        }

        static void Philosopher(object num)
        {
            int philosopherNum = (int)num;
            Random rand = new Random();

            while (true)
            {
                Console.WriteLine($"Philosopher {philosopherNum} is now thinking......");
                Thread.Sleep(rand.Next(1000, 5000));

                Console.WriteLine($"Philosopher {philosopherNum} wants to eat and is trying to get a fork");
                _forks[philosopherNum].WaitOne();
                Console.WriteLine($"Philosopher {philosopherNum} recieved first fork");
                _forks[(philosopherNum + 1) % _forks.Length].WaitOne();
                Console.WriteLine($"Philosopher {philosopherNum} recieved second fork");

                Console.WriteLine($"Philosopher {philosopherNum} is eating.");
                Thread.Sleep(rand.Next(1000, 5000));

                _forks[philosopherNum].Release();
                _forks[(philosopherNum + 1) % _forks.Length].Release();
            }
        }
    }
}
