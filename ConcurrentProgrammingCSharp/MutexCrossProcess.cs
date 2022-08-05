namespace ConcurrentProgrammingCSharp
{
	internal class MutexCrossProcess
	{
		public static void RunExample()
		{
			var appName = "MyApp";
			try
			{
				var mu = Mutex.OpenExisting(appName);
				Console.WriteLine($"Sorry, {appName} is already running.");
				return;
			}
			catch (WaitHandleCannotBeOpenedException)
			{
				Console.WriteLine("We can run the program just fine.");
				var mu = new Mutex(false, appName);
			}

			Console.ReadKey();
		}
	}
}
