using System.Collections.Generic;

namespace Events
{
    public class EventKeyComparer : IEqualityComparer<EventKey>
    {
        bool IEqualityComparer<EventKey>.Equals(EventKey x, EventKey y) => x?.EventType == y?.EventType && x.EventTypeValue == y.EventTypeValue;

        int IEqualityComparer<EventKey>.GetHashCode(EventKey x) => x.EventType.GetHashCode() + x.EventTypeValue.GetHashCode();
    }
}