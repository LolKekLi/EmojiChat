using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using System.Collections;

namespace EmojiChat.UI
{
    public class UIAnswerSlot : MonoBehaviour, IDropHandler
    {
        public static Action<UIEmojiItem, UIAnswerSlot> Dropped = delegate { };
        public static Action<UIEmojiItem, UIAnswerSlot> Moved = delegate { };
        public static Action<UIEmojiItem> Freed = delegate { };

        public const float Delay = 0.2f;

        [SerializeField]
        private Image _icon = null;

        [SerializeField]
        private BaseTweenController _tutorialController = null;

        [SerializeField]
        private BaseTweenController _setupController = null;

        private RectTransform _transform = null;

        private UIEmojiItem _item = null;

        public bool IsFree => ReferenceEquals(_item, null);

        private void Awake()
        {
            _transform = (RectTransform)transform;
        }

        private void OnEnable()
        {
            UIFreeArea.Dropped += UIFreeArea_Dropped;
            UIEmojiItem.Dropped += UIEmojiItem_Dropped;
            UIEmojiItem.Clicked += UIEmojiItem_Clicked;
            GameManager.Failed += GameManager_Failed;
            Moved += UIAnswerSlot_Moved;
            TutorialManager.Invoked += TutorialManager_Invoked;
        }

        private void OnDisable()
        {
            UIFreeArea.Dropped -= UIFreeArea_Dropped;
            UIEmojiItem.Dropped -= UIEmojiItem_Dropped;
            UIEmojiItem.Clicked -= UIEmojiItem_Clicked;
            GameManager.Failed -= GameManager_Failed;
            Moved -= UIAnswerSlot_Moved;
            TutorialManager.Invoked -= TutorialManager_Invoked;
        }

        public void Reset()
        {
            if (_item != null)
            {
                Freed(_item);
            }

            _item = null;
        }

        public void ForceSetup(UIEmojiItem item)
        {
            Freed(_item);

            _item = item;

            Dropped(_item, this);

            StartCoroutine(MoveCor(_item));
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (IsFree)
            {
                if (eventData.pointerDrag != null)
                {
                    _item = eventData.pointerDrag.GetComponent<UIEmojiItem>();

                    if (!IsFree)
                    {
                        Moved(_item, this);
                        Dropped(_item, this);

                        StartCoroutine(MoveCor(_item));
                    }
                }
            }
        }

        private void ItemDrop(UIEmojiItem item)
        {
            if (ReferenceEquals(_item, item))
            {
                _item = null;
                item.ForceSetup(null);
            }
        }

        private void UIFreeArea_Dropped(UIEmojiItem item)
        {
            ItemDrop(item);
        }

        private void UIEmojiItem_Dropped(UIEmojiItem item)
        {
            ItemDrop(item);
        }

        private void UIEmojiItem_Clicked(UIEmojiItem item)
        {
            if (IsFree && !item.IsSetupped)
            {
                ForceSetup(item);
                item.ForceSetup(this);
            }
        }

        private void UIAnswerSlot_Moved(UIEmojiItem item, UIAnswerSlot slot)
        {
            if (ReferenceEquals(_item, item) && !ReferenceEquals(this, slot))
            {
                _item = null;

                Freed(item);
            }
        }

        private void TutorialManager_Invoked()
        {
            if (IsFree)
            {
                _tutorialController.Play();
            }
        }

        private void GameManager_Failed()
        {
            Action action = () =>
            {
                Color32 customColor = new Color32(254, 62, 97, 255);
                _icon.DOColor(customColor, Delay);

                this.InvokeWithDelay(Delay, () =>
                {
                    _icon.DOColor(Color.white, Delay);
                });
            };

            var preset = GameManager.Instance.GetMessagePreset();

            if (!ReferenceEquals(_item, null) && !preset.EmojiSequence.Contains(_item.Type))
            {
                action.Invoke();

                this.InvokeWithDelay(2 * Delay, () =>
                {
                    action.Invoke();
                });
            }
        }

        private IEnumerator MoveCor(UIEmojiItem item)
        {
            float time = 0;
            float progress = 0f;
            Vector3 startPos = item.Transform.position;

            while (time < Delay)
            {
                yield return null;

                time += Time.deltaTime;
                progress = time / Delay;

                item.Transform.position = Vector3.Lerp(startPos, _transform.position, progress);
            }

            _setupController.Play();
        }
    }
}
