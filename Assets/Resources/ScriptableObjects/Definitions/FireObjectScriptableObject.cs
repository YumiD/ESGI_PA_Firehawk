using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GridObject")]
public class FireObjectScriptableObject : ScriptableObject
{
	public string SerializedName;
	public GameObject Prefab;
}
