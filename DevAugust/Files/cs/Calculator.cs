using DevAugust.API;
using Terrasoft.Core;
using Terrasoft.Core.Factories;

namespace DevAugust
{
	
	[DefaultBinding(typeof(ICalculator), Name = "One")]
	public class CalculatorOne : ICalculator
	{
		private readonly UserConnection _userConnection;

		public CalculatorOne(UserConnection userConnection)
		{
			_userConnection = userConnection;
		}

		public int Add(int a, int b)
		{
			string message = "{\"event\":\"event body goes here\"}";
			var conf = ClassFactory.Get<IConfToClio>();
			conf.PostMessage(_userConnection, GetType().Name, message);
			return a + b;

		}

		public int Multiply(int a, int b)
		{
			return a * b;
		}
	}
	
	[DefaultBinding(typeof(ICalculator), Name = "Two")]
	public class CalculatorTwo : ICalculator
	{
		public int Add(int a, int b)
		{
			return a + b;
		}

		public int Multiply(int a, int b)
		{
			return a * b;
		}
	}
}
