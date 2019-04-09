/**
 * 字节buffer
 */

using System;

public class ByteBuffer
{
    /// <summary>
    /// 容量
    /// </summary>
    private int mCapacity;
    /// <summary>
    /// 长度
    /// </summary>
    private int mLength;
    /// <summary>
    /// 字节数组
    /// </summary>
    private byte[] mBuffer;
    //=========================================================
    public int Capacity { get { return mCapacity; } }
    public int Length { get { return mLength; } }

    /// <summary>
    /// 构造
    /// </summary>
    public ByteBuffer(int capacity)
    {
        mCapacity = capacity;
        mLength = 0;
        mBuffer = new byte[capacity];
    }

    /// <summary>
    /// 写入int
    /// </summary>
    public void WriteInt(int value)
    {
        var pos = UpdateLength(4);
        mBuffer[pos + 0] = (byte)(value >> 24 & 0xFF);
        mBuffer[pos + 1] = (byte)(value >> 16 & 0xFF);
        mBuffer[pos + 2] = (byte)(value >> 08 & 0xFF);
        mBuffer[pos + 3] = (byte)(value >> 00 & 0xFF);
    }

    /// <summary>
    /// 写入字节数组
    /// </summary>
    public void WriteBytes(byte[] value)
    {
        var pos = UpdateLength(value.Length);
        Buffer.BlockCopy(value, 0, mBuffer, pos, value.Length);
    }

    /// <summary>
    /// 更新长度
    /// </summary>
    public int UpdateLength(int writeLen)
    {
        if (mLength + writeLen > mCapacity)
        {
            throw new Exception("ByteBuffer out of capacity");
        }
        var pos = mLength;
        mLength += writeLen;
        return pos;
    }

    /// <summary>
    /// 获得字节数组
    /// </summary>
    public byte[] GetBytes()
    {
        return mBuffer;
    }
}
