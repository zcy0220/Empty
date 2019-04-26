/**
 * 定时器
 */

using System;
using UnityEngine;

namespace Base.Timer
{
    public class Timer
    {
        /// <summary>
        /// 循环次数 -1表示无限循环
        /// </summary>
        private int mLoop;
        /// <summary>
        /// 持续时间
        /// </summary>
        private float mDuration;
        /// <summary>
        /// 剩余时间
        /// </summary>
        private float mLeftTime;
        /// <summary>
        /// 时间缩放
        /// 一般情况战斗逻辑为flase, UI定时器为true
        /// </summary>
        private bool mUnScale;
        /// <summary>
        /// 控制运行
        /// </summary>
        private bool mRunning;
        /// <summary>
        /// 是否结束
        /// </summary>
        private bool mIsOver;
        /// <summary>
        /// 真实时间的开始时刻
        /// </summary>
        private float mStartRealTime;
        /// <summary>
        /// 定时更新行为 返回剩余时间
        /// </summary>
        private Action mFinishAction;
        //========================================================
        public bool IsOver { get { return mIsOver; } }
        
        /// <summary>
        /// 定时器参数
        /// </summary>
        public void Init(float duration, Action finishAction, int loop = 1, bool unScale = false)
        {
            mDuration = duration;
            mLoop = loop;
            mUnScale = unScale;
            mFinishAction = finishAction;
            mRunning = false;
            mIsOver = false;
        }
        
        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            mLeftTime = mDuration;
            mStartRealTime = Time.realtimeSinceStartup;
            mRunning = true;
            mIsOver = false;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            mRunning = false;
        }
        
        /// <summary>
        /// 恢复
        /// </summary>
        public void Resume()
        {
            mRunning = true;
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            mLeftTime = 0;
            mRunning = false;
            mIsOver = true;
        }

        /// <summary>
        /// 定时
        /// </summary>
        public void Update()
        {
            if (!mRunning) return;

            // 要用真实时间时：unscaledDeltaTime 和 通过Time.realtimeSinceStartup - mStartRealTime
            // 测试结果，二者几乎就是一样的
            float deltaTime = mUnScale ? Time.realtimeSinceStartup - mStartRealTime : Time.deltaTime;
            mStartRealTime = Time.realtimeSinceStartup;

            mLeftTime -= deltaTime;
            if (mLeftTime <= 0)
            {
                if (mFinishAction != null) mFinishAction();
                if (mLoop > 0)
                {
                    mLoop--;
                    mLeftTime += mDuration;
                }
                
                if (mLoop == 0)
                {
                    Stop();
                    return;
                }

                if (mLoop < 0)
                {
                    mLeftTime += mDuration;
                }
            }
        }
    }
}
