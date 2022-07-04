using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.LevelEditor
{
    public class LevelEditorUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject gridGenerator;
        public void GenerateLevel(int dropdownValue) {
            Instantiate(gridGenerator, new Vector3(0, 0, 0), Quaternion.identity);
        }
        public void QuitLevelEditor() {
            SceneManager.LoadScene(0);
        }
    }
}
