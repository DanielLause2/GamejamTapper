using System.Collections.Generic;

public class Eventbus
{
    public delegate void EventFunction(params object[] data);

    private static Dictionary<EventbusEvent, List<EventFunction>> EventListener { get; set; }

    static Eventbus()
    {
        EventListener = new Dictionary<EventbusEvent, List<EventFunction>>();
    }

    public static void Register(EventbusEvent eventType, EventFunction function)
    {
        List<EventFunction> eventFunctions;
        EventListener.TryGetValue(eventType, out eventFunctions);

        if (eventFunctions == null)
        {
            eventFunctions = new List<EventFunction>();
            EventListener.Add(eventType, eventFunctions);
        }

        eventFunctions.Add(function);
    }

    public static void Unregister(EventbusEvent eventType, EventFunction function)
    {
        List<EventFunction> eventFunctions;
        EventListener.TryGetValue(eventType, out eventFunctions);

        if (eventFunctions == null)
            return;

        eventFunctions.Remove(function);
    }

    public static void Push(EventbusEvent eventType, params object[] data)
    {
        List<EventFunction> eventFunctions;
        EventListener.TryGetValue(eventType, out eventFunctions);

        if (eventFunctions == null)
            return;

        List<EventFunction> deepCopy = new List<EventFunction>(eventFunctions);

        for (int i = 0; i < deepCopy.Count; i++)
            deepCopy[i](data);
    }
}

public class EventbusEvent
{
}
