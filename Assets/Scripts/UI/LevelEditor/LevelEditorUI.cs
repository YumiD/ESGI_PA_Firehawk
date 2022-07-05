using System.Net;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;
using Helper;
using Grid;

namespace UI.LevelEditor{
    public class LevelEditorUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject gridGenerator;

        private class GridData{
            /*public string test1 = "letsgoooo";
            public int test2 = 80085;*/
            //public GridCellData[] gridCells;
            //public List<GridCellData> gridCells;

            public GridCellData[] gridCells;
        }

        private class GridCellData{
            public Vector3 pos = new Vector3();
        }

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