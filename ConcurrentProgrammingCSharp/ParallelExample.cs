namespace ConcurrentProgrammingCSharp
{
	internal class ParallelExample
	{
		static IEnumerable<int> GenerateSteps(int start, int end, int step)
		{
			for (int i = start; i < end; i += step)
			{

				yield return i;
			}
		}
		
		static void Demo()
		{
			result = Parallel.For(1, 21, (i, state) =>
			{
				if(i == 10)
				{
					//throw new Exception();
					//state.Stop();
					state.Break();
				}

			});
		}

		private static ParallelLoopResult result;

		public static void Run()
		{
			var p = new ParallelOptions();
			var a = () => { Console.WriteLine($"Task a, Current Task Id is {Task.CurrentId}"); };
			var b = () => { Console.WriteLine($"Task b, Current Task Id is {Task.CurrentId}"); };
			var c = () => { Console.WriteLine($"Task c, Current Task Id is {Task.CurrentId}"); };

			Parallel.Invoke(a, b, c);

			//Parallel.For(1, 11, i => Console.WriteLine(i * i));

			var words = new string[] { "I", "Love", "You" };
			//Parallel.ForEach(words, x => Console.Write($"{x}\t"));

			//Parallel.ForEach(GenerateSteps(0,22,3), x => Console.WriteLine(x));

			try
			{
				Demo();
			}
			catch (AggregateException ae)
			{
				ae.Handle(ex =>
				{
					Console.WriteLine(ex.Message);
					return true;
				});
			}

			Console.WriteLine($"{result.IsCompleted} {result.LowestBreakIteration}");

			Console.ReadLine();
		}
	}
}
