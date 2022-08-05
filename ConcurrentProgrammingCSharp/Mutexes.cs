namespace ConcurrentProgrammingCSharp
{
	public class BankAccountMutexes
	{
		private int balance;

		public int Balance { get => balance; private set => balance = value; }

		public void Deposit(int amount) => Balance += amount;

		public void Withdraw(int amount) => Balance -= amount;

		public void Transfer(BankAccountMutexes b, int amount)
		{
			Balance -= amount;
			b.balance += amount;
		}
	}

	internal class Mutexes
	{
		public static void RunExample()
		{
			var tasks = new List<Task>();
			var ba = new BankAccountMutexes();
			var ba2 = new BankAccountMutexes();
			var mu = new Mutex();
			var mu2 = new Mutex();

			for(int i = 0; i < 10; i++)
			{
				tasks.Add(Task.Factory.StartNew(() =>
				{
					for (int j = 0; j < 1000; j++)
					{
						var hasLock = mu.WaitOne();
						try
						{
							ba.Deposit(1);
						}
						finally
						{
							if(hasLock) mu.ReleaseMutex();
						}
						
					}
				}));

				tasks.Add(Task.Factory.StartNew(() => {
					for (int j = 0; j < 1000; j++)
					{
						var hasLock = mu2.WaitOne();
						try
						{
							ba2.Deposit(1);
						}
						finally
						{
							if (hasLock) mu2.ReleaseMutex();
						}
					}
				}));

				tasks.Add(Task.Factory.StartNew(() =>
				{
					for (int j = 0; j < 1000; j++)
					{
						var hasLocks = Mutex.WaitAll(new[] { mu, mu2 });
						try
						{
							ba.Transfer(ba2, 1);
						}
						finally
						{
							if(hasLocks)
							{
								mu.ReleaseMutex();
								mu2.ReleaseMutex();
							}
						}
					}
				}));
			}

			Task.WaitAll(tasks.ToArray());
			Console.WriteLine($"The final balance is ba: {ba.Balance} ba2: {ba2.Balance}");
		}
	}
}
