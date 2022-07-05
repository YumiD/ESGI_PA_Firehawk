using System;
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
        private TerrainGrid _terrainGridInstance;
        
        public void GenerateLevel(int dropdownValue) {
            _terrainGridInstance = Instantiate(gridGenerator, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<TerrainGrid>();
            _terrainGridInstance.Create(dropdownValue switch
            {
                0 => new Vector3Int(10, 10, 10),
                1 => new Vector3Int(20, 20, 10),
                2 => new Vector3Int(30, 30, 10),
                _ => throw new ArgumentOutOfRangeException()
            });
        }
        public void QuitLevelEditor() {
            SceneManager.LoadScene(0);
        }
        public void ExportTerrainJSON(){

            JObject root = _terrainGridInstance.Serialize();

            File.WriteAllText(Application.dataPath + "/gridGenerated.json", root.ToString(Formatting.None));
        }
    }
}
