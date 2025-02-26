using System;
using System.Collections.Generic;

namespace SNGames.CommonModule
{
    public static class SNEventsController<T> where T : Enum
    {
        private static readonly Dictionary<T, Delegate> eventsInGame = new Dictionary<T, Delegate>();

        // Register an event with a specific data type
        public static void RegisterEvent<U>(T eventName, Action<U> actionCallBack)
        {
            if (eventsInGame.ContainsKey(eventName))
            {
                eventsInGame[eventName] = Delegate.Combine(eventsInGame[eventName], actionCallBack);
            }
            else
            {
                eventsInGame.Add(eventName, actionCallBack);
            }
        }

        // Trigger an event with a specific data type
        public static void TriggerEvent<U>(T eventName, U data)
        {
            if (eventsInGame.TryGetValue(eventName, out var del))
            {
                if (del is Action<U> callback)
                {
                    callback.Invoke(data);
                }
                else
                {
                    throw new InvalidOperationException($"Event {eventName} expects a different data type.");
                }
            }
        }

        // Deregister an event listener
        public static void DeregisterEvent<U>(T eventName, Action<U> listener)
        {
            if (eventsInGame.TryGetValue(eventName, out var del))
            {
                var currentDel = Delegate.Remove(del, listener);
                if (currentDel == null)
                {
                    eventsInGame.Remove(eventName);
                }
                else
                {
                    eventsInGame[eventName] = currentDel;
                }
            }
        }
    }
}
