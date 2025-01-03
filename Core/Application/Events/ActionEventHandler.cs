﻿using Core.Contract.Application.Events;

namespace Core.Application.Events;

public class ActionEventHandler<T> : IEventHandler<T> where T : IEvent
{
    private readonly Action<T> _action;

    public ActionEventHandler(Action<T> action)
    {
        _action = action;
    }

    public void Handle(T eventToHandle)
    {
        _action.Invoke(eventToHandle);
    }
}
