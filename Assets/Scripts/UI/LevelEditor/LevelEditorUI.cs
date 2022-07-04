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
            /*GridData gridData = new GridData();
            GridCellData griCellData = new GridCellData();
            griCellData.pos = new Vector3(1,1,1);
            //gridData.gridCells.Add(griCellData);
            gridData.gridCells = new GridCellData[1];
            gridData.gridCells[0] = griCellData;

            //string strOutput = JsonUtility.ToJson(gridData);
            string strOutput = JsonConvert.SerializeObject(griCellData, Formatting.Indented);
            print(strOutput);
            File.WriteAllText(Application.dataPath + "/gridGenerated.json", strOutput);*/

            string toto = "abc";
            int tata = 2;
            Vector3Int tonton = new Vector3Int(1,1,1);
            JObject root = new JObject();
            string json = @"{ CPU: 'Intel',
            Drives: [
                'DVD read/writer',
                '500 gigabyte hard drive'
            ]
            }";
            root["a"] = tonton.ToJson();
            root["b"] = JObject.Parse(json);

            Vector3Int gridGeneratorSize = gridGenerator.GetComponent<TerrainGrid>().GetSize();

            print(root.ToString());
            print(gridGeneratorSize);

            File.WriteAllText(Application.dataPath + "/gridGenerated.json", root.ToString());

        }

    }
}