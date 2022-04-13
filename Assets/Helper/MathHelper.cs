namespace Helper
{
	public static class MathHelper
	{
		public static bool IsBetween(this int v, int min, int max)
		{
			return v >= min && v <= max;
		}
	}
}