namespace ConcurrentProgrammingCSharp
{
	public class Foo
	{
		private Foo()
		{
		}

		private async Task<Foo> InitAsync()
		{
			await Task.Delay(1000);
			return this;
		}

		public static Task<Foo> CreateFooAsync()
		{
			var foo = new Foo();
			return foo.InitAsync();
		}
	}

	public class AsyncFactory
	{
		public async Task Run()
		{
			var foo = await Foo.CreateFooAsync();
		}
	}
}
