/**
 * Update管理
 */

using Base.Common;
using UnityEngine;

namespace Base.Update
{
    public delegate void UpdateEvent(float deltaTime);
    public delegate void LateUpdateEvent();
    public delegate void FixedUpdateEvent(float fixedDeltaTime);

    public class UpdateManager : MonoSingleton<UpdateManager>
    {
        /// <summary>
        /// Update监听事件
        /// </summary>
        private event UpdateEvent mUpdateEvent;
        /// <summary>
        /// LateUpdate监听事件
        /// </summary>
        private event LateUpdateEvent mLateUpdateEvent;
        /// <summary>
        /// FixedUpdate监听事件
        /// </summary>
        private event FixedUpdateEvent mFixedUpdateEvent;
        
        /// <summary>
        /// 添加Update监听
        /// </summary>
        public void AddUpdateListener(UpdateEvent listener)
        {
            mUpdateEvent += listener;
        }
        
        /// <summary>
        /// 添加LateUpdate监听
        /// </summary>
        public void AddLateUpdateListener(LateUpdateEvent listener)
        {
            mLateUpdateEvent += listener;
        }
        
        /// <summary>
        /// 添加FixedUpdate
        /// </summary>
        public void AddFixedUpdateListener(FixedUpdateEvent listener)
        {
            mFixedUpdateEvent += listener;
        }
        
        /// <summary>
        /// 删除Update监听
        /// </summary>
        public void RemoveUpdateListener(UpdateEvent listener)
        {
            mUpdateEvent -= listener;
        }
        
        /// <summary>
        /// 删除LateUpdate监听
        /// </summary>
        public void RemoveLateUpdateListener(LateUpdateEvent listener)
        {
            mLateUpdateEvent -= listener;
        }
        
        /// <summary>
        /// 删除FixedUpdate监听
        /// </summary>
        public void RemoveFixedUpdateListener(FixedUpdateEvent listener)
        {
            mFixedUpdateEvent -= listener;
        }

        /// <summary>
        /// Update
        /// </summary>
        private void Update()
        {
            if (mUpdateEvent != null) mUpdateEvent(Time.deltaTime);
        }
        
        /// <summary>
        /// LateUpdate
        /// </summary>
        private void LateUpdate()
        {
            if (mLateUpdateEvent != null) mLateUpdateEvent();
        }
        
        /// <summary>
        /// FixedUpdate
        /// </summary>
        private void FixedUpdate()
        {
            if (mFixedUpdateEvent != null) mFixedUpdateEvent(Time.fixedDeltaTime);
        }
    }
}
