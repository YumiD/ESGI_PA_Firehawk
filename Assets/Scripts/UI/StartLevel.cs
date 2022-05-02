using System;
using Events.Trigger;
using TMPro;
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
                startBtn.text = "Pause";
                triggerFireEvent.Raise();
            }
            else
            {
                var currentGameState = startBtn.text.Equals("Pause");
                Time.timeScale = currentGameState ? 0f : 1f;
                startBtn.text =  currentGameState ? "Resume" : "Pause";
            }
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}