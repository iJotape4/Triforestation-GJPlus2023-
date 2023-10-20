using System;

namespace Events
{
    public class EventKey
    {
        public Type EventType { get; private set; }
        public int EventTypeValue { get; private set; }
        public string ValueName { get; private set; }

        public EventKey(Enum @enum)
        {
            EventType = @enum.GetType();
            EventTypeValue = Convert.ToInt32(@enum);
            ValueName = @enum.ToString();
        }
    }
}