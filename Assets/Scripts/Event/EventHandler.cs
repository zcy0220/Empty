/**
 * 事件句柄
 */

using System;

/// <summary>
/// 事件接收接口
/// </summary>
public interface IEventReceiver { }
/// <summary>
/// 事件发送接口
/// </summary>
public interface IEventDispatcher { }

/// <summary>
/// 事件句柄
/// </summary>
public class EventHandler
{
    /// <summary>
    /// 接受者
    /// </summary>
    public IEventReceiver Receiver { get; private set; }
    /// <summary>
    /// 接受者触发的回调
    /// </summary>
    public Action<Object[]> Callback { get; private set; }

    /// <summary>
    /// 构造
    /// </summary>
    public EventHandler(IEventReceiver receiver, Action<Object[]> callback)
    {
        Receiver = receiver;
        Callback = callback;
    }
}

