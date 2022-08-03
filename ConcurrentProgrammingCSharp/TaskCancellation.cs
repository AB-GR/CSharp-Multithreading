using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentProgrammingCSharp
{
	internal class TaskCancellation
	{
		public static void RunExample()
		{
			var cts = new CancellationTokenSource();
			var token = cts.Token;
			token.Register(() => { Console.WriteLine("Cancellation Requested."); });

			var t = new Task(() =>
			{
				int i = 0;
				while (true)
				{
					//token.ThrowIfCancellationRequested();
					if (token.IsCancellationRequested)
						break;
					//throw new TaskCanceledException();
					else
						Console.WriteLine(i++);
				}
			}, token);

			t.Start();

			var t2 = new Task(() =>
			{
				token.WaitHandle.WaitOne();
				Console.WriteLine("Cancellation done");
			});
			t2.Start();

			Console.ReadLine();
			cts.Cancel();

			var emergencyCts = new CancellationTokenSource();
			var plannedCts = new CancellationTokenSource();
			var preventiveCts = new CancellationTokenSource();

			var paranoidCts = CancellationTokenSource.CreateLinkedTokenSource(emergencyCts.Token, plannedCts.Token, preventiveCts.Token);

			var t3 = new Task(() =>
			{
				int i = 0;
				while (true)
				{
					if (paranoidCts.Token.IsCancellationRequested)
						break;
					else
						Console.WriteLine(i++);

					Thread.Sleep(1000);

				}
			}, paranoidCts.Token);
			t3.Start();

			Console.ReadLine();
			emergencyCts.Cancel();

			var bombCts = new CancellationTokenSource();
			var t4 = new Task(() =>
			{
				Console.WriteLine("You have 5 secs before you can disarm the bomb");
				var cancelled = bombCts.Token.WaitHandle.WaitOne(5000);
				Console.WriteLine(cancelled ? "Bomb disarmed" : "Boom!!!");
			}, bombCts.Token);

			t4.Start();

			Console.ReadLine();
			bombCts.Cancel();

			var waitCts = new CancellationTokenSource();
			var t5 = new Task(() =>
			{
				Console.WriteLine("Task for 5 secs start");
				for (int i = 0; i < 5; i++)
				{
					waitCts.Token.ThrowIfCancellationRequested();
					Thread.Sleep(1000);
				}

				Console.WriteLine("Task for 5 secs Complete");
			});

			var t6 = new Task(() =>
			{
				Console.WriteLine("Task for 6 secs start");
				for (int i = 0; i < 6; i++)
				{
					waitCts.Token.ThrowIfCancellationRequested();
					Thread.Sleep(1000);
				}

				Console.WriteLine("Task for 6 secs Complete");
			});

			t5.Start();
			t6.Start();

			waitCts.Cancel();
			Task.WaitAny(new[] { t5, t6 });//, waitCts.Token aggregates the operation cancelled exception

			Console.WriteLine($"Task 5 status : {t5.Status}, Task 6 status : {t6.Status}");
			Console.ReadKey();
			try
			{
				Test();
			}
			catch (AggregateException ae)
			{

				foreach (var inner in ae.InnerExceptions)
				{
					Console.WriteLine(inner.Message);
				}
			}
			

			Console.ReadKey();
		}

		private static void Test()
		{
			var t7 = Task.Factory.StartNew(() =>
			{
				throw new InvalidOperationException("Invalid operation");
			});

			var t8 = Task.Factory.StartNew(() =>
			{
				throw new AccessViolationException("Access violation");
			});

			try
			{
				Task.WaitAll(new[] { t7, t8 });
			}
			catch (AggregateException ae)
			{
				ae.Handle((ex) => {
					if (ex is InvalidOperationException)
					{
						Console.WriteLine("InvalidAccess handled");
						return true;
					}

					return false;
				});
			}
		}
	}
}
