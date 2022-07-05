using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;
using Grid;
using Newtonsoft.Json;

namespace UI.LevelEditor
{
    public class LevelEditorUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject gridGenerator;
        private GameObject _gridGeneratorInstance;
        
        public void GenerateLevel(int dropdownValue) {
            _gridGeneratorInstance = Instantiate(gridGenerator, new Vector3(0, 0, 0), Quaternion.identity);
        }
        public void QuitLevelEditor() {
            SceneManager.LoadScene(0);
        }
        public void ExportTerrainJSON(){

            JObject root = _gridGeneratorInstance.GetComponent<TerrainGrid>().Serialize();

            print(root.ToString());

            File.WriteAllText(Application.dataPath + "/gridGenerated.json", root.ToString(Formatting.None));

        }
    }
}
