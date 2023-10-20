using System;
using System.Collections.Generic;

namespace Events
{
    public class EventManager
    {
        private static Dictionary<EventKey, Delegate[]> eventHandlers = new Dictionary<EventKey, Delegate[]>(new EventKeyComparer());

        private static EventKey GetKey(Enum key) => new EventKey(key);

        public static void AddListener(Enum key, EventHandlerDelegate handler)
        {
            EventKey key1 = GetKey(key);
            Delegate[] delegateArray;
            if (eventHandlers.TryGetValue(key1, out delegateArray))
            {
                eventHandlers[key1][0] = Delegate.Combine(delegateArray[0], handler);
            }
            else
            {
                eventHandlers.Add(key1, new Delegate[2]);
                eventHandlers[key1][0] = handler;
            }
        }

        public static void AddListener<T>(Enum key, EventHandlerDelegate<T> handler)
        {
            EventKey key1 = GetKey(key);
            Delegate[] delegateArray;
            if (eventHandlers.TryGetValue(key1, out delegateArray))
            {
                eventHandlers[key1][1] = Delegate.Combine(delegateArray[1], handler);
            }
            else
            {
                eventHandlers.Add(key1, new Delegate[2]);
                eventHandlers[key1][1] = handler;
            }
        }

        public static void Dispatch(Enum key)
        {
            Delegate[] delegateArray;
            if (!eventHandlers.TryGetValue(GetKey(key), out delegateArray) || delegateArray[0] == null || !(delegateArray[0] is EventHandlerDelegate eventHandlerDelegate))
                return;
            eventHandlerDelegate();
        }

        public static void Dispatch<T>(Enum key, T eventData)
        {
            EventKey key1 = GetKey(key);
            Delegate[] delegateArray;
            if (!eventHandlers.TryGetValue(key1, out delegateArray))
                return;
            if (delegateArray[0] != null && delegateArray[0] is EventHandlerDelegate eventHandlerDelegate1)
                eventHandlerDelegate1();
            if (delegateArray[1] == null)
                return;
            Type genericTypeArgument = delegateArray[1].GetType().GenericTypeArguments[0];
            if (typeof(T) != genericTypeArgument)
                throw new ArgumentException("event " + key1.EventType.Name + "." + key1.ValueName + " expected: " + genericTypeArgument.Name + " but received: " + typeof(T).Name);
            if (delegateArray[1] is EventHandlerDelegate<T> eventHandlerDelegate2)
                eventHandlerDelegate2(eventData);
        }

        public static void RemoveListener(Enum key, EventHandlerDelegate handler)
        {
            EventKey key1 = GetKey(key);
            Delegate[] delegateArray;
            if (!eventHandlers.TryGetValue(key1, out delegateArray) || (object)delegateArray[0] == null)
                return;
            Delegate @delegate = Delegate.Remove(delegateArray[0], handler);
            eventHandlers[key1][0] = @delegate;
        }

        public static void RemoveListener<T>(Enum key, EventHandlerDelegate<T> handler)
        {
            EventKey key1 = GetKey(key);
            Delegate[] delegateArray;
            if (!eventHandlers.TryGetValue(key1, out delegateArray) || (object)delegateArray[1] == null)
                return;
            Delegate @delegate = Delegate.Remove(delegateArray[1], handler);
            eventHandlers[key1][1] = @delegate;
        }

        public static void RemoveAllListeners() => eventHandlers.Clear();
    }
}