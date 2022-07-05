using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;
using Helper;
using Grid;

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
        public void ExportTerrainJSON(){

            JObject root = gridGenerator.GetComponent<TerrainGrid>().Serialize();

            print(root.ToString());

            File.WriteAllText(Application.dataPath + "/gridGenerated.json", root.ToString());

        }
    }
}
