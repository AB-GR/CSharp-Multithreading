namespace ConcurrentProgrammingCSharp
{
	internal class ContinuationExample
	{
		public static void Run()
		{
			var t1 = Task.Factory.StartNew(() => {
				Console.WriteLine($"In Task {Task.CurrentId}");
			});

			var t2 = t1.ContinueWith(t => { 
				Console.WriteLine($"Task {t.Id} is {t.Status}");
				Console.WriteLine($"Continuing with Task {Task.CurrentId}");
			});

			t2.Wait();

			var t3 = Task.Factory.StartNew(() => {
				Console.WriteLine($"In Task {Task.CurrentId}");
			});

			var t4 = Task.Factory.StartNew(() => {
				Console.WriteLine($"In Task {Task.CurrentId}");
			});

			var t5 = Task.Factory.ContinueWhenAll(new[] { t3, t4 }, (tasks) =>
			{
				foreach (var t in tasks)
					Console.WriteLine($"Task {t.Id} is {t.Status}");
				Console.WriteLine($"Continuing with Task {Task.CurrentId}");
			});

			t5.Wait();

			var t6 = Task.Factory.StartNew(() => {
				Console.WriteLine($"In Task {Task.CurrentId}");
			});

			var t7 = Task.Factory.StartNew(() => {
				Console.WriteLine($"In Task {Task.CurrentId}");
			});

			var t8 = Task.Factory.ContinueWhenAny(new[] { t6, t7 }, (t) =>
			{
				Console.WriteLine($"Task {t.Id} is {t.Status}");
				Console.WriteLine($"Continuing with Task {Task.CurrentId}");
			});

			t8.Wait();

			var parent = Task.Factory.StartNew(() => {
				var child = Task.Factory.StartNew(() =>
				{
					Console.WriteLine($"Child task started {Task.CurrentId}");
					Thread.Sleep(1000);
					throw new Exception();
					Console.WriteLine($"Child task completed {Task.CurrentId}");
				}, TaskCreationOptions.AttachedToParent);

				child.ContinueWith((t) => {
					Console.WriteLine($"Hooray Task {t.Id}'s status is {t.Status}");
				}, TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnRanToCompletion);

				child.ContinueWith((t) => {
					Console.WriteLine($"Oops Task {t.Id}'s status is {t.Status}");
				}, TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnFaulted);
			});

			try
			{
				parent.Wait();
			}
			catch (AggregateException ae)
			{
				ae.Handle(e => true);
			}
		}
	}
}
