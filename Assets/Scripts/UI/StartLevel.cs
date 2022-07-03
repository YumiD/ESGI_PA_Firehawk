using System;
using Events.Trigger;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class StartLevel : MonoBehaviour
    {
        [SerializeField] private EventTrigger triggerFireEvent;
        [SerializeField] private Text startBtn;

        private bool _hasStarted;

        public void StartFire()
        {
            if (!_hasStarted)
            {
                _hasStarted = true;
                startBtn.text = "||";
                triggerFireEvent.Raise();
            }
            else
            {
                bool currentGameState = startBtn.text.Equals("||");
                Time.timeScale = currentGameState ? 0f : 1f;
                startBtn.text =  currentGameState ? "▶" : "||";
            }
        }

        public void Reset()
        {
            GameManager.Instance.IsEditMode = true;
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}