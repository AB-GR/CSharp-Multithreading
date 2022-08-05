namespace ConcurrentProgrammingCSharp
{
	public class BankAccountSp
	{
		private object padLock = new object();
		private int balance;

		public int Balance { get => balance; private set => balance = value; }

		public void Deposit(int amount) => Balance += amount;

		public void Withdraw(int amount) => Balance -= amount;
	}

	internal class SpinLocks
	{
		static SpinLock spGlobal = new SpinLock();

		public static void LockRecursion(int max)
		{
			var lockTaken = false;
			try
			{
				spGlobal.Enter(ref lockTaken);
				Console.WriteLine("Max is " + max);
				LockRecursion(max-1);
			}
			catch (LockRecursionException ex)
			{
				Console.WriteLine("Exception " + ex);
			}
			finally
			{
				if(lockTaken) spGlobal.Exit();
			}
		}

		public static void RunExample()
		{
			var tasks = new List<Task>();
			var ba = new BankAccountSp();
			var sp = new SpinLock();

			for (int i = 0; i < 1000; i++)
				tasks.Add(Task.Factory.StartNew(() => 
				{
					var lockTaken = false;
					try
					{
						sp.Enter(ref lockTaken);
						ba.Deposit(100);
					}
					finally
					{
;						if(lockTaken) sp.Exit();
					}
				}));

			for (int i = 0; i < 1000; i++)
				tasks.Add(Task.Factory.StartNew(() => {
					var lockTaken = false;
					try
					{
						sp.Enter(ref lockTaken);
						ba.Withdraw(100);
					}
					finally
					{
						; if (lockTaken) sp.Exit();
					}
				}));

			Task.WaitAll(tasks.ToArray());
			Console.WriteLine($"The final balance is {ba.Balance}");

			LockRecursion(5);
		}
	}
}
