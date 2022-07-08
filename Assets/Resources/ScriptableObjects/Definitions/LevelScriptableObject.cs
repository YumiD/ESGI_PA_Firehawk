using System.Collections.Generic;
using Scriptable_Objects;
using UnityEngine;

namespace ScriptableObjects.Definitions
{
	[CreateAssetMenu(menuName = "ScriptableObjects/Level")]
	public class LevelScriptableObject : ScriptableObject
	{
		public List<ItemDictionary> Objects;
		public TextAsset JsonData;
	}
}
