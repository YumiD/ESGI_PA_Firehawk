using UnityEngine;

namespace Helper
{
	public static class VecHelper
	{
		public static Vector3 Add(this Vector3 left, Vector3 right)
		{
			return new Vector3
			{
				x = left.x + right.x,
				y = left.y + right.y,
				z = left.z + right.z
			};
		}
		
		public static Vector3 Substract(this Vector3 left, Vector3 right)
		{
			return new Vector3
			{
				x = left.x - right.x,
				y = left.y - right.y,
				z = left.z - right.z
			};
		}
		
		public static Vector3 Multiply(this Vector3 left, Vector3 right)
		{
			return new Vector3
			{
				x = left.x * right.x,
				y = left.y * right.y,
				z = left.z * right.z
			};
		}
		
		public static Vector3 Divide(this Vector3 left, Vector3 right)
		{
			return new Vector3
			{
				x = left.x / right.x,
				y = left.y / right.y,
				z = left.z / right.z
			};
		}

		public static Vector3Int RoundToInt(this Vector3 vec)
		{
			return new Vector3Int
			{
				x = Mathf.RoundToInt(vec.x),
				y = Mathf.RoundToInt(vec.y),
				z = Mathf.RoundToInt(vec.z)
			};
		}

		public static Vector3Int FloorToInt(this Vector3 vec)
		{
			return new Vector3Int
			{
				x = Mathf.FloorToInt(vec.x),
				y = Mathf.FloorToInt(vec.y),
				z = Mathf.FloorToInt(vec.z)
			};
		}

		public static Vector3Int CeilToInt(this Vector3 vec)
		{
			return new Vector3Int
			{
				x = Mathf.CeilToInt(vec.x),
				y = Mathf.CeilToInt(vec.y),
				z = Mathf.CeilToInt(vec.z)
			};
		}

		public static Vector3 Clamp(this Vector3 vec, Vector3 min, Vector3 max)
		{
			return new Vector3
			{
				x = Mathf.Clamp(vec.x, min.x, max.x),
				y = Mathf.Clamp(vec.y, min.y, max.y),
				z = Mathf.Clamp(vec.z, min.z, max.z)
			};
		}

		public static bool IsBetween(this Vector3Int vec, Vector3Int min, Vector3Int max)
		{
			return vec.x.IsBetween(min.x, max.x) &&
			       vec.y.IsBetween(min.y, max.y) &&
			       vec.y.IsBetween(min.y, max.y);
		}
	}
}