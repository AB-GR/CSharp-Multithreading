namespace ConcurrentProgrammingCSharp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			//Task.Factory.StartNew(() => Write('1'));
			//var t = new Task(() => Write('2'));
			//t.Start();
			//Write('3');

			//Task.Factory.StartNew(Write, '1');
			//var t = new Task(Write, 2);
			//t.Start();
			//Console.ReadKey();

			//var t1 = Task.Factory.StartNew<int>(TextLength, "Testing");
			//var t2 = new Task<int>(TextLength, "New");
			//t2.Start();

			//Console.WriteLine($"T1 result is {t1.Result}");
			//Console.WriteLine($"T2 result is {t2.Result}");

			//TaskCancellation.RunExample();
			//CriticalSections.RunExample();
			//SpinLocks.RunExample();
			//Mutexes.RunExample();
			//MutexCrossProcess.RunExample();
			//ReaderWriterLockSlimExample.RunExample();
			ConcurrentDictionaryExamples.RunExample();
		}

		static int TextLength(object arg)
		{
			Console.WriteLine($"The task with id {Task.CurrentId} with arg {arg} is processing..");
			return arg.ToString().Length;
		}

		static void Write(object arg)
		{
			int i = 1000;
			while (i --> 0)
			{
				Console.Write(arg);
			}
		}

		static void Write(char c)
		{
			int i = 1000;
			while(i --> 0)
			{
				Console.Write(c);
			}
		}
	}
}