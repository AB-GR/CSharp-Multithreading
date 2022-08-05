using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentProgrammingCSharp
{
	public class BankAccount
	{
		private object padLock = new object();

		public int Balance { get; private set; }

		public void Deposit(int amount)
		{
			lock(padLock)
			{
				Balance += amount;
			}
		}

		public void Withdraw(int amount)
		{
			lock (padLock)
			{
				Balance -= amount;
			}
		}
	}

	internal class CriticalSections
	{
		public static void RunExample()
		{
			var tasks = new List<Task>();
			var ba = new BankAccount();
			for(int i = 0; i < 1000; i++)
				tasks.Add(Task.Factory.StartNew(() => ba.Deposit(100)));

			for (int i = 0; i < 1000; i++)
				tasks.Add(Task.Factory.StartNew(() => ba.Withdraw(100)));

			Task.WaitAll(tasks.ToArray());
			Console.WriteLine($"The final balance is {ba.Balance}");
		}
	}
}
