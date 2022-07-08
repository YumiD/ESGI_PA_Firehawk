using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Grid;
using Newtonsoft.Json.Linq;
using SimpleFileBrowser;

namespace UI.LevelEditor
{
    public class LevelEditorUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject gridGenerator;

        [SerializeField]
        private GameObject levelUiCanvas;

        [SerializeField]
        private GameObject selectSize;

        [SerializeField] 
        private CameraManager cameraManager;
        
        public void GenerateLevel(int dropdownValue) {
            GameObject gridInstantiated = Instantiate(gridGenerator, new Vector3(0, 0, 0), Quaternion.identity);
            TerrainGrid terrainGridInstance = gridInstantiated.GetComponent<TerrainGrid>();
            terrainGridInstance.Create(dropdownValue switch
            {
                0 => new Vector3Int(10, 10, 10),
                1 => new Vector3Int(20, 20, 10),
                2 => new Vector3Int(30, 30, 10),
                _ => throw new ArgumentOutOfRangeException()
            });
            levelUiCanvas.SetActive(true);
            selectSize.SetActive(false);
        }
        public void QuitLevelEditor() {
            SceneManager.LoadScene(0);
        }

        public void ImportTerrainJson()
        {
            FileBrowser.SetFilters(false, new FileBrowser.Filter("Level data", ".json"));
            FileBrowser.ShowLoadDialog(path => {
                JObject jsonData = JObject.Parse(File.ReadAllText(path[0]));
                
                TerrainGrid terrainGridInstance = Instantiate(gridGenerator, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<TerrainGrid>();
                terrainGridInstance.Deserialize(jsonData);
                levelUiCanvas.SetActive(true);
                selectSize.SetActive(false);
            }, () =>
            {
                cameraManager.MoveCamera();
            }, FileBrowser.PickMode.Files);
        }
    }
}
