using System.Collections.Generic;
using UnityEngine;

namespace Events.Trigger
{
    [CreateAssetMenu(fileName = "Event", menuName = "Events/Event Trigger", order = 1)]
    public class EventTrigger : ScriptableObject
    {
        private readonly List<GameEventListener> _listeners = new List<GameEventListener>();
        public void RegisterListener(GameEventListener listener)
        {
            _listeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            _listeners.Remove(listener);
        }

        public void Raise()
        {
            for (var i = _listeners.Count - 1; i >= 0; --i)
            {
                _listeners[i].RaiseEvent();
            }
        }
    }
}
