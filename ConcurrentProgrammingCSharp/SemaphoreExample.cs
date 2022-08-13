namespace ConcurrentProgrammingCSharp
{
	internal class SemaphoreExample
	{
		public static void RunExample()
		{
			var sem = new SemaphoreSlim(2, 10);
			for (int i = 0; i < 20; i++)
			{
				Task.Factory.StartNew(() =>
				{
					Console.WriteLine($"entering task {Task.CurrentId}");
					sem.Wait();
					Console.WriteLine($"processing task {Task.CurrentId}");
				});
			}

			while(sem.CurrentCount <= 2)
			{
				Console.WriteLine($"Current count is {sem.CurrentCount}");
				Console.ReadKey();
				sem.Release(2);
			}
		}
	}
}
