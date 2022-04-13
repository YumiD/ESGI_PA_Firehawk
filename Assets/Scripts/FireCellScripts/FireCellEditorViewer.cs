using UnityEngine;

[ExecuteInEditMode]
public class FireCellEditorViewer : MonoBehaviour
{
	[SerializeField]
	private FireCell _cell;
	
	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, _cell.Radius);
	}
}
