namespace ConcurrentProgrammingCSharp
{
	public class ThreadLocalStorage
	{
		public static void Run()
		{
			var sum = 0;
			Parallel.For(1, 1001, () => 0, (x, state, tls) =>
			{
				tls += x;
				return tls;
			}, partialSum =>
			{
				Interlocked.Add(ref sum, partialSum);
			});

			Console.WriteLine(sum);
			Console.ReadLine();
		}
	}
}
