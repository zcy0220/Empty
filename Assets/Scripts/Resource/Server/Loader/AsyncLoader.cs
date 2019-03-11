/**
 * 基础的异步加载器基类
 * 提供对协程操作的支持，但是异步操作的进行不依赖于协程，可以在Update等函数中查看进度值
 */

using System;
using System.Collections;

namespace Resource
{
    public abstract class AsyncLoader : IEnumerator, IDisposable
    {
        protected bool mIsDone;

        public object Current { get; }

        public bool MoveNext() { return mIsDone; }

        public void Reset() { }

        public virtual float Progress() { return 0; }

        public virtual void Update() { }

        public virtual void Dispose() { }
    }
}


