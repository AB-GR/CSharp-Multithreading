using System.Collections.Concurrent;

namespace ConcurrentProgrammingCSharp
{
	internal class ConcurrentDictionaryExamples
	{
		static ConcurrentDictionary<string, string> capitals = new ConcurrentDictionary<string, string>();

		static void AddParis()
		{
			var success = capitals.TryAdd("France", "Paris");
			string who = Task.CurrentId.HasValue ? ("Task " + Task.CurrentId) : "Main thread";
			Console.WriteLine($"{who} {(success ? "added" : "did not add")} the element.");
		}

		public static void RunExample()
		{
			AddParis();
			Task.Factory.StartNew(() => AddParis());

			//capitals["Russia"] = "Leningrad";
			capitals.AddOrUpdate("Russia", "Moscow", (key, old) => $"{old} --> Moscow");
			Console.WriteLine("The capital of Russia is " + capitals["Russia"]);

			//capitals["Sweden"] = "Uppsala";
			var swedenCap = capitals.GetOrAdd("Sweden", "Stockholm");
			Console.WriteLine("The capital of Sweden is " + capitals["Sweden"]);

			var toRemove = "Russia1";
			var removed = string.Empty;
			var didRemove = capitals.TryRemove(toRemove, out removed);
			if(didRemove)
				Console.WriteLine($"We just removed {removed}");
			else
				Console.WriteLine($"Failed to remove capital of {toRemove}");

			foreach (var kv in capitals)
			{
				Console.WriteLine($"Country - {kv.Key} -- Capital - {kv.Value}");
			}

			var cQ = new ConcurrentQueue<int>();
			cQ.Enqueue(1);
			cQ.Enqueue(2);

			int queueItem;
			if (cQ.TryDequeue(out queueItem))
			{
				Console.WriteLine($"Following item {queueItem} has been dequeued");
			}

			var cStack = new ConcurrentStack<int>();
			cStack.Push(1);
			cStack.Push(2);
			cStack.Push(3);
			cStack.Push(4);

			int stackPeekItem;
			if (cStack.TryPeek(out stackPeekItem))
			{
				Console.WriteLine($"Following item {stackPeekItem} has been peeked");
			}

			int stackItem;
			if(cStack.TryPop(out stackItem))
			{
				Console.WriteLine($"Following item {stackItem} has been destacked");
			}

			var intArr = new int[5];
			if(cStack.TryPopRange(intArr, 0, 5) > 0)
			{
				foreach (var item in intArr)
				{
					Console.Write(" " + item + " ");
				}

				Console.WriteLine();
			}

			var bag = new ConcurrentBag<int>();
			var tasks = new List<Task>();
			for (int i = 0; i < 10; i++)
			{
				var i1 = i;
				tasks.Add(Task.Factory.StartNew(() =>
				{
					bag.Add(i1);
					Console.WriteLine($"Task {Task.CurrentId} added {i1} to bag");

					int peek;
					if(bag.TryPeek(out peek))
					{
					Console.WriteLine($"Task {Task.CurrentId} peeked {peek} from bag");
					}
				}));
			}

			Task.WaitAll(tasks.ToArray());

			int last;
			if(bag.TryTake(out last))
			{
				Console.WriteLine($"I got {last}");
			}

			Console.ReadKey();
		}
	}
}
