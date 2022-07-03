using JetBrains.Annotations;
using UnityEngine;

namespace Grid
{
	public class GridCell : MonoBehaviour
	{
		public enum CellSurface
		{
			Flat,
			Slide
		}

		public GameObject Anchor;

		[CanBeNull]
		[HideInInspector]
		public GameObject Object;

		public CellSurface Surface;

		[HideInInspector]
		public Vector3Int GridPosition;
	}
}