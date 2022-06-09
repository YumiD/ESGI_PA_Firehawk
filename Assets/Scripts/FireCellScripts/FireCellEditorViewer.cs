using UnityEngine;

namespace FireCellScripts
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(FireCell))]
	public class FireCellEditorViewer : MonoBehaviour
	{
		private FireCell _cell;

		private void Start()
		{
			_cell = GetComponent<FireCell>();
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireSphere(transform.position, _cell.Radius);
		}
	}
}
