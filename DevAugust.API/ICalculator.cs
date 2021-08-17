namespace DevAugust.API
{

	/// <summary>
	/// Basic calculator interface
	/// </summary>
	public interface ICalculator
	{
		/// <summary>
		/// Adds two integers
		/// </summary>
		/// <param name="a">First Number</param>
		/// <param name="b">Second Number</param>
		/// <returns>Result of addition</returns>
		int Add(int a, int b);

		/// <summary>
		/// Multiplies two integers
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns>Result of multiplication</returns>
		int Multiply(int a, int b);
	}
}
