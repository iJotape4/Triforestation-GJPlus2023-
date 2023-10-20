using System;
using UnityEngine;

namespace Events
{
    public enum EventsExamples
    {
        Example1
    }

    public class EventsExample : MonoBehaviour
    {
        public event Action<string> ExampleReceived;

        private void Awake()
        {
            EventManager.AddListener<string>(EventsExamples.Example1, HandleExample1);
            ExampleReceived += HandleExample1;
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<string>(EventsExamples.Example1, HandleExample1);
            ExampleReceived -= HandleExample1;
        }

        void Start()
        {
            EventManager.Dispatch(EventsExamples.Example1, "Hola Mundo");
            ExampleReceived?.Invoke("Hola Mundo");
        }

        private void HandleExample1(string eventData)
        {
            Debug.Log($"{eventData}");
        }
    }
}
