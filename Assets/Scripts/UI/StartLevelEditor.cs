using System.IO;
using Events.Trigger;
using Grid;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class StartLevelEditor : MonoBehaviour
    {
        [SerializeField] private EventTrigger triggerFireEvent;
        [SerializeField] private Text startBtn;
        [SerializeField] private GameObject gridGenerator;

        private TerrainGrid _originalTerrainGrid;
        private bool _hasStarted;

        public void StartFire()
        {
            if (!_hasStarted)
            {
                _hasStarted = true;
                startBtn.text = "■";
                triggerFireEvent.Raise();
                
                StartSimulation();
            }
            else
            {
                startBtn.text = "▶";
                StopSimulation();
            }
        }

        private void StartSimulation()
        {
            // Duplicate terrain grid
            if (_originalTerrainGrid == null)
            {
                _originalTerrainGrid = FindObjectOfType<TerrainGrid>();
            }
            JObject data = _originalTerrainGrid.Serialize();
            File.WriteAllText(Path.GetTempPath() + "/temp.json", data.ToString(Formatting.None));
        }

        private void StopSimulation()
        {
            Destroy(_originalTerrainGrid.gameObject);
         
            JObject jsonData = JObject.Parse(File.ReadAllText(Path.GetTempPath() + "/temp.json"));
            TerrainGrid terrainGridInstance = Instantiate(gridGenerator, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<TerrainGrid>();
            terrainGridInstance.Deserialize(jsonData);
            File.Delete(Path.GetTempPath() + "/temp.json");
            _hasStarted = false;
        }

        public void Reset()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
    }
}