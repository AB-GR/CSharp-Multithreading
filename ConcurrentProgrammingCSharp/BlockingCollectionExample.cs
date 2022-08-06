using System.Collections.Concurrent;

namespace ConcurrentProgrammingCSharp
{
	internal class BlockingCollectionExample
	{
		static BlockingCollection<int> bc = new BlockingCollection<int>(new ConcurrentBag<int>(), 10);
		static CancellationTokenSource cts = new CancellationTokenSource();
		static Random rnd = new Random();

		public static void Run()
		{
			var tasks = new List<Task>();

			tasks.Add(Task.Factory.StartNew(StartProducer));
			tasks.Add(Task.Factory.StartNew(StartConsumer));

			Task.WaitAll(tasks.ToArray(), cts.Token);

			Console.ReadKey();
		}

		private static void StartConsumer()
		{
			foreach (var item in bc.GetConsumingEnumerable())
			{
				cts.Token.ThrowIfCancellationRequested();
				Console.WriteLine($"-{item} \t");

				Thread.Sleep(rnd.Next(1000));
			}
		}

		private static void StartProducer()
		{
			while(true)
			{
				cts.Token.ThrowIfCancellationRequested();

				var i = rnd.Next(100);
				if (bc.TryAdd(i))
					Console.WriteLine($"+{i} \t");
				else
					Console.WriteLine("Collection is full at the moment");

				Thread.Sleep(rnd.Next(100));
			}
		}
	}
}
