/**
 * 事件管理
 */

using System;
using System.Collections.Generic;

public static class EventManager
{
    /// <summary>
    /// 事件映射列表
    /// </summary>
    private static readonly Dictionary<EventEnum, List<EventHandler>> mEventHandlerDict = new Dictionary<EventEnum, List<EventHandler>>();

    /// <summary>
    /// 添加事件监听
    /// </summary>
    public static void AddEventListener(this IEventReceiver self, EventEnum @event, Action<Object[]> callback)
    {
        if (callback == null) return;
        List<EventHandler> eventHandlerList;
        if (mEventHandlerDict.TryGetValue(@event, out eventHandlerList))
        {
            eventHandlerList = mEventHandlerDict[@event];
        }
        else
        {
            eventHandlerList = new List<EventHandler>();
            mEventHandlerDict.Add(@event, eventHandlerList);
        }
        eventHandlerList.Add(new EventHandler(self, callback));
    }

    /// <summary>
    /// 删除事件监听
    /// </summary>
    public static void RemoveEventListener(this IEventReceiver self, EventEnum @event, Action<Object[]> callback)
    {
        if (mEventHandlerDict.ContainsKey(@event))
        {
            var eventHandlerList = mEventHandlerDict[@event];
            for (var i = 0; i < eventHandlerList.Count; i++)
            {
                var handler = eventHandlerList[i];
                if (handler.Receiver == self)
                {
                    eventHandlerList.RemoveAt(i);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// 事件消息派发
    /// </summary>
    public static void DispatchEvent(EventEnum @event, params object[] args)
    {
        if (mEventHandlerDict.ContainsKey(@event))
        {
            var eventHandlerList = mEventHandlerDict[@event];
            for (var i = 0; i < eventHandlerList.Count; i++)
            {
                var callback = eventHandlerList[i].Callback;
                callback(args);
            }
        }
    }

    /// <summary>
    /// 扩展事件消息派发
    /// </summary>
    public static void DispatchEvent(this IEventDispatcher self, EventEnum @event, params object[] args)
    {
        DispatchEvent(@event, args);
    }
}
