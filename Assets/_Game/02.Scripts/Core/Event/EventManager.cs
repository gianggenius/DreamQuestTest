using System;
using System.Collections.Generic;

namespace _Game._02.Scripts.Core
{
    /// <summary>
    /// A simple event manager that allows you to subscribe to events and trigger them.
    /// </summary>
    public static class EventManager
    {
        /// <summary>
        /// Dictionary of all the subscribers with key is the event type and the value is a list of subscribers.
        /// </summary>
        private static Dictionary<Type, List<IEventListener>> _subscribers;

        static EventManager()
        {
            _subscribers = new Dictionary<Type, List<IEventListener>>();
        }

        /// <summary>
        /// Subscribe to an event type with a listener.
        /// </summary>
        /// <param name="listener">Listener want to subscribe to event</param>
        /// <typeparam name="TEvent">Event type</typeparam>
        public static void AddListener<TEvent>(IEventListener<TEvent> listener) where TEvent : struct
        {
            var eventType = typeof(TEvent);

            // If the event type is not in the dictionary, we add it.
            if (!_subscribers.ContainsKey(eventType))
            {
                _subscribers[eventType] = new List<IEventListener>();
            }

            // We only add the listener if it is not already subscribed.
            if (!SubscriptionExists(eventType, listener))
            {
                _subscribers[eventType].Add(listener);
            }
        }


        /// <summary>
        /// Remove a listener for an event type from the subscribers dictionary.
        /// </summary>
        /// <param name="listener"></param>
        /// <typeparam name="TEvent"></typeparam>
        public static void RemoveListener<TEvent>(IEventListener<TEvent> listener) where TEvent : struct
        {
            var eventType = typeof(TEvent);

            if (!_subscribers.ContainsKey(eventType))
            {
                return;
            }

            var subscriberList = _subscribers[eventType];

            for (int i = subscriberList.Count - 1; i >= 0; i--)
            {
                if (subscriberList[i] == listener)
                {
                    subscriberList.Remove(subscriberList[i]);

                    if (subscriberList.Count == 0)
                    {
                        _subscribers.Remove(eventType);
                    }

                    return;
                }
            }
        }

        /// <summary>
        /// Fire an event to all subscribers.
        /// </summary>
        /// <param name="newEvent"></param>
        /// <typeparam name="TEvent"></typeparam>
        public static void TriggerEvent<TEvent>(TEvent newEvent) where TEvent : struct
        {
            if (!_subscribers.TryGetValue(typeof(TEvent), out var list))
                return;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                (list[i] as IEventListener<TEvent>)?.OnReceiveEvent(newEvent);
            }
        }
    
        /// <summary>
        /// Checks if a subscription exists for a certain event type.
        /// </summary>
        /// <param name="type">Type of Event</param>
        /// <param name="receiver">Receiver that already subscribed in dictionary</param>
        /// <returns>if receiver is subscribed or not</returns>
        private static bool SubscriptionExists(Type type, IEventListener receiver)
        {
            if (!_subscribers.TryGetValue(type, out var receivers)) return false;

            var exists = false;
            // We check if the receiver is already subscribed to the event type.
            for (var i = receivers.Count - 1; i >= 0; i--)
            {
                if (receivers[i] != receiver) continue;
                exists = true;
                break;
            }

            return exists;
        }
    }
}
