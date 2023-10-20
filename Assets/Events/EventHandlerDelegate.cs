namespace Events
{
    public delegate void EventHandlerDelegate();

    public delegate void EventHandlerDelegate<in TEvent>(TEvent eventData);
}
