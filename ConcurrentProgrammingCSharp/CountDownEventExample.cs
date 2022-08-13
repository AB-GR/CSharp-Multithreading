namespace ConcurrentProgrammingCSharp
{
	internal class CountDownEventExample
	{
		static int taskCount = 5;
		static CountdownEvent cte = new CountdownEvent(taskCount);
		static Random random = new Random();

		public static void RunExample()
		{
			var tasks = new Task[taskCount];
			for (int i = 0; i < taskCount; i++)
			{
				tasks[i] = Task.Factory.StartNew(() =>
				{
					Console.WriteLine($"Entering task {Task.CurrentId}");
					Thread.Sleep(3000);

					Console.WriteLine($"Exiting task {Task.CurrentId}");
					cte.Signal();
				});
			}

			var finalTask = Task.Factory.StartNew(() =>
			{
				Console.WriteLine($"Waiting for other tasks in Task {Task.CurrentId}");
				cte.Wait();
				Console.WriteLine("All tasks completed");
			});

			finalTask.Wait();
			Console.ReadLine();
		}
	}
}
