namespace ConcurrentProgrammingCSharp
{
	internal class ResetEvents
	{
		
		public static void RunExample()
		{
			AutoResetExample();
			Console.ReadLine();

		}

		private static void ManualResetExample()
		{
			var m = new ManualResetEventSlim();

			var t1 = Task.Factory.StartNew(() =>
			{
				Console.WriteLine("Boiling water...");
				for (int i = 0; i < 30; i++)
				{
					Thread.Sleep(100);
				}
				Console.WriteLine("Water is ready.");

				m.Set();
			});

			var t2 = Task.Factory.StartNew(() =>
			{
				Console.WriteLine("Waiting for water...");
				m.Wait(5000);
				Console.WriteLine("Here is your tea!");
				Console.WriteLine($"Is the event set? {m.IsSet}");

				m.Wait(5000); // already set!
				Console.WriteLine("Not waiting");
				m.Reset();
				Console.WriteLine($"Is the event set? {m.IsSet}");
				m.Wait(5000); // already set!
				Console.WriteLine("That was a nice cup of tea!");
			});

			t2.Wait();
		}

		private static void AutoResetExample()
		{
			var m = new AutoResetEvent(false);

			var t1 = Task.Factory.StartNew(() =>
			{
				Console.WriteLine("Boiling water...");
				for (int i = 0; i < 30; i++)
				{
					Thread.Sleep(100);
				}
				Console.WriteLine("Water is ready.");

				m.Set();
			});

			var t2 = Task.Factory.StartNew(() =>
			{
				Console.WriteLine("Waiting for water...");
				m.WaitOne(5000);
				Console.WriteLine("Here is your tea!");
				var ok = m.WaitOne(1000); // already set!
				if(ok)
					Console.WriteLine("That was a nice cup of tea!");
				else
					Console.WriteLine("No tea for you");
			});

			t2.Wait();
		}
	}
}
