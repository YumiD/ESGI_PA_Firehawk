using ScriptableObjects.Definitions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Menu
{
	public class LevelSelect : MonoBehaviour
	{
		public static LevelScriptableObject LevelToLoad;

		public void StartLevel(LevelScriptableObject jsonFile)
		{
			LevelToLoad = jsonFile;
			SceneManager.LoadScene(2);
		}
	}
}