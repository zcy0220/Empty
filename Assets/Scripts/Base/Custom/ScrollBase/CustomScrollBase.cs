/**
 * 自定义滚动层基类
 */

using UnityEngine;
using UnityEngine.EventSystems;

namespace Base.Custom
{
    public class CustomScrollBase : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public virtual void OnBeginDrag(PointerEventData eventData) { }

        public virtual void OnDrag(PointerEventData eventData) { }

        public virtual void OnEndDrag(PointerEventData eventData) { }
    }
}
