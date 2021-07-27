using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EmojiChat.UI
{
    public class UIFreeArea : MonoBehaviour, IDropHandler
    {
        public static event Action<UIEmojiItem> Dropped = delegate { };

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                var item = eventData.pointerDrag.GetComponent<UIEmojiItem>();

                if (!ReferenceEquals(item, null))
                {
                    Dropped(item);
                }
            }
        }
    }
}