namespace ConcurrentProgrammingCSharp
{
	public class TaskRunExample
	{
		public static async void Run()
		{
			var unwrappedTask = Task.Factory.StartNew(async delegate {
				await Task.Delay(1000);
				return 123;
			},
			CancellationToken.None,
			TaskCreationOptions.DenyChildAttach,
			TaskScheduler.Default).Unwrap();

			var unwrappedI = await await Task.Factory.StartNew(async delegate {
				await Task.Delay(1000);
				return 123;
			},
			CancellationToken.None,
			TaskCreationOptions.DenyChildAttach,
			TaskScheduler.Default);

			//Task.WaitAll & Task.WaitAny are blocking
			//Task.WhenAll & Task.WhenAny are awaitable equivalents
		}
	}
}
