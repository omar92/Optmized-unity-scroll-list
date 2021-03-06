using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableSystem
{
    [CreateAssetMenu(fileName = "EventSO", menuName = "SO/Event")]
    public class EventSO : ScriptableObject
    {
        Dictionary<EventSOListener, UnityEvent<object>> callbacks = new Dictionary<EventSOListener, UnityEvent<object>>();

        public void Raise()
        {
            Raise(null);
        }
        public async void Raise(object value)
        {
            await Task.Delay(1); //this will cause event to act is if they were in a queue (they will be stacked after frame)
            RaiseSync(value);
        }

        public void RaiseSync()
        {
            RaiseSync(null);
        }
        public void RaiseSync(object value)
        {
            foreach (var kvp in callbacks.ToArray())
            {
                // Debug.Log($"<color=green>SoEvent</color> {name} <color=green>invoked</color> {kvp.Key.name}");
                kvp.Value.Invoke(value);
            }
        }
        /// <summary>
        /// register listtener in this even so it will be invoked when this event is raised
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="calback"></param>
        public void Subscribe(EventSOListener listener, UnityEvent<object> calback)
        {
            if (!callbacks.ContainsKey(listener))
            {
                callbacks.Add(listener, calback);
            }
            else
            {
                callbacks[listener] = calback;
            }
        }
        /// <summary>
        /// remove the listener from this event
        /// </summary>
        /// <param name="listener"></param>
        public void UnSubscribe(EventSOListener listener)
        {
            if (callbacks.ContainsKey(listener))
            {
                callbacks.Remove(listener);
            }
        }

        void OnAfterDeserialize()
        {
            callbacks.Clear();
        }
        void OnBeforeSerialize()
        {
            callbacks.Clear();
        }

    }
}