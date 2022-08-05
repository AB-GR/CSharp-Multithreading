namespace ConcurrentProgrammingCSharp
{
	internal class ReaderWriterLockSlimExample
	{
		// recursion is not recommended and can lead to deadlocks
		static ReaderWriterLockSlim padlock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

		public static void RunExample()
		{
			int x = 0;
			var tasks = new List<Task>();	

			for(int i = 0; i < 10; i++)
			{
				tasks.Add(Task.Factory.StartNew(() =>
				{
					//padlock.EnterReadLock();
					padlock.EnterUpgradeableReadLock();

					//if (i % 2 == 0)
					//{
					//	padlock.EnterWriteLock();
					//	x++;
					//	padlock.ExitWriteLock();
					//}
					Console.WriteLine($"Entered read lock, x = {x}, pausing for 5sec");
					Thread.Sleep(5000);
					padlock.ExitUpgradeableReadLock();
					Console.WriteLine($"Exited read lock, x = {x}.");
				}));
			}

			try
			{
				Task.WaitAll(tasks.ToArray());
			}
			catch (AggregateException ae)
			{
				ae.Handle(e =>
				{
					Console.WriteLine(e);
					return true;
				});
			}

			Random random = new Random();
			while(true)
			{
				Console.ReadKey();
				padlock.EnterWriteLock();
				Console.WriteLine("Writelock acquired");

				int newValue = random.Next(10);
				x = newValue;
				Console.WriteLine($"Set x = {x}");
				padlock.ExitWriteLock();
				Console.WriteLine("Write lock released");
			}
		}
	}
}
