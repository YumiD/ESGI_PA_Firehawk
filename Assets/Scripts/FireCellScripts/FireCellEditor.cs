using UnityEditor;
using UnityEngine;

namespace FireCellScripts
{
#if UNITY_EDITOR
	[CustomEditor(typeof(FireCell))]
	public class FireCellEditor : Editor
	{
		private float _temperature = 0;

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
		
			FireCell fireCell = (FireCell)target;

			_temperature = EditorGUILayout.FloatField("Temperature", _temperature);
		
			if (GUILayout.Button("Apply"))
			{
				fireCell.DebugSetTemperature(_temperature);
			}
		}
	}
#endif
}
