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
        private GameObject _cloneTerrainGrid;
        private bool _hasStarted;

        public void StartFire()
        {
            if (!_hasStarted)
            {
                _hasStarted = true;
                startBtn.text = "■";
                StartSimulation();
                
                triggerFireEvent.Raise();
            }
            else
            {
                startBtn.text = "▶";
                triggerFireEvent.Raise();
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

            _originalTerrainGrid.gameObject.SetActive(false);
            _originalTerrainGrid.transform.position = new Vector3(0, -200, 0);
            GameObject clone = GameManager.Instance.ImportTerrainJson();
            _cloneTerrainGrid = clone;
            ReserveObjectManager.Instance.InstantiateReserve();
        }

        private void StopSimulation()
        {
            Destroy(_cloneTerrainGrid);
            ReserveObjectManager.Instance.HideDuplicate();
            _originalTerrainGrid.transform.position = Vector3.zero;
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