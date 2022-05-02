using UnityEngine;
using UnityEngine.Events;

namespace Events.Trigger
{
    public class GameEventListener : MonoBehaviour
    {
        // The game event instance to register to.
        public EventTrigger gameEvent;
        // The unity event response created for the event.
        public UnityEvent response;

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public void RaiseEvent()
        {
            response.Invoke();
        }
    }
}
