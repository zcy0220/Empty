/**
 * 字节buffer对象池
 */

using Base.Common;
using System.Collections.Generic;

public class ByteBufferPool : Singleton<ByteBufferPool>
{
    /// <summary>
    /// 多容量ByteBuffer队列
    /// </summary>
    private Dictionary<int, Queue<ByteBuffer>> mByteBufferQueueDict = new Dictionary<int, Queue<ByteBuffer>>();

    /// <summary>
    /// 分配对应容量的ByteBuffer
    /// </summary>
    /// <param name="capacity"></param>
    public ByteBuffer Spawn(int capacity)
    {
        if (mByteBufferQueueDict.ContainsKey(capacity))
        {
            var queue = mByteBufferQueueDict[capacity];
            if (queue.Count > 0)
            {
                return queue.Dequeue();
            }
        }
        return new ByteBuffer(capacity);
    }

    /// <summary>
    /// 回收ByteBuffer
    /// </summary>
    /// <param name="byteBuffer"></param>
    public void Recycle(ByteBuffer byteBuffer)
    {
        lock(mByteBufferQueueDict)
        {
            byteBuffer.Clear();
            if (mByteBufferQueueDict.ContainsKey(byteBuffer.Capacity))
            {
                var queue = mByteBufferQueueDict[byteBuffer.Capacity];
                queue.Enqueue(byteBuffer);
            }
            else
            {
                var queue = new Queue<ByteBuffer>();
                queue.Enqueue(byteBuffer);
                mByteBufferQueueDict.Add(byteBuffer.Capacity, queue);
            }
        }
    }
}
