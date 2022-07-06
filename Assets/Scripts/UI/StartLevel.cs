using Events.Trigger;
using Grid;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class StartLevel : MonoBehaviour
    {
        [SerializeField] private EventTrigger triggerFireEvent;
        [SerializeField] private Text startBtn;

        private TerrainGrid _originalTerrainGrid;
        private TerrainGrid _cloneTerrainGrid;
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

            _cloneTerrainGrid = Instantiate(_originalTerrainGrid);
            _cloneTerrainGrid.gameObject.SetActive(false);
        }

        private void StopSimulation()
        {
            Destroy(_originalTerrainGrid.gameObject);
            _originalTerrainGrid = _cloneTerrainGrid;
            _originalTerrainGrid.gameObject.SetActive(true);
            _cloneTerrainGrid = null;
            _hasStarted = false;
        }

        public void Reset()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}