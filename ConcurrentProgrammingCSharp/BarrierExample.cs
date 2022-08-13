namespace ConcurrentProgrammingCSharp
{
	public class BarrierExample
	{
		static Barrier b = new Barrier(2, b =>
		{
			Console.WriteLine($"Phase {b.CurrentPhaseNumber} is Finished.");
		});

		private static void Water()
		{
			Console.WriteLine("Find the kettle");
			Console.WriteLine("Pour water into the kettle and boil (takes some time)");
			Thread.Sleep(1000);
			b.SignalAndWait();
			Console.WriteLine("Pour boiling water into the cup");
			b.SignalAndWait();
			Console.WriteLine("Put the kettle away");
		}

		private static void Cup()
		{
			Console.WriteLine("Find the cup");
			b.SignalAndWait();
			Console.WriteLine("Add tea powder");
			b.SignalAndWait();
			Console.WriteLine("Add sugar");
		}

		public static void RunExample()
		{
			var t1 = Task.Factory.StartNew(Water);
			var t2 = Task.Factory.StartNew(Cup);

			var tea = Task.Factory.ContinueWhenAll(new[] { t1, t2 }, tasks =>
			{
				Console.WriteLine("Enjoy your cup of tea.");
			});

			tea.Wait();
		}
	}
}
