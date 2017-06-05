using NUnit.Framework;

namespace AndroidAltBeaconLibrary.UnitTests
{
	public static class AssertEx
	{
		public static void AreEqual(string message, int expected, int actual)
		{
			Assert.AreEqual(expected, actual, message);
		}
		
		public static void AreEqual(string message, long expected, long actual)
		{
			Assert.AreEqual(expected, actual, message);
		}
		
		public static void AreEqual(string message, object expected, object actual)
		{
			Assert.AreEqual(expected, actual, message);
		}
	}
}