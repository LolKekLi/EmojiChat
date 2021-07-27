using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

namespace EmojiChat.UI
{
    public class UIEmojiItem : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IDropHandler
    {
        public static event Action<UIEmojiItem> Dropped = delegate { };
        public static event Action<UIEmojiItem> Clicked = delegate { };
        public static event Action Picked = delegate { };
        public static event Action Freed = delegate { };
        public static event Action DragEnded = delegate { };

        public const float Delay = 0.25f;

        [SerializeField]
        private Image _image = null;

        [SerializeField]
        private EmptyGraphics _graphic = null;

        [SerializeField]
        private BaseTweenController _tutorialController = null;

        private RectTransform _transform = null;
        private UIAnswerSlot _slot = null;

        private float _speed = 15f;
        private int _index = 0;

        private Vector3 _startPos = Vector3.zero;
        private Vector3 _initialPos = Vector3.zero;

        private Vector3 _finalPos = Vector3.zero;

        private bool _isDropped = false;
        private bool _isDragged = false;
        private bool _isDragCanceled = true;

        private Coroutine _moveCor = null;

        public EmojiType Type
        {
            get;
            private set;
        }

        public bool IsSetupped => !ReferenceEquals(_slot, null);

        public RectTransform Transform => _transform;

        private void Awake()
        {
            _transform = (RectTransform)transform;
        }

        private void OnEnable()
        {
            UIAnswerSlot.Dropped += AnswerSlot_Dropped;
            UIFreeArea.Dropped += UIFreeArea_Dropped;
            TutorialManager.Invoked += TutorialManager_Invoked;
        }

        private void OnDisable()
        {
            UIAnswerSlot.Dropped -= AnswerSlot_Dropped;
            UIFreeArea.Dropped -= UIFreeArea_Dropped;
            TutorialManager.Invoked -= TutorialManager_Invoked;
        }

        private void FixedUpdate()
        {
            if (_isDragged)
            {
                transform.position = Vector3.Lerp(transform.position, _finalPos, Time.deltaTime * _speed);
            }
        }

        public void Hide()
        {
            _transform.DOScale(0f, Delay);
        }

        public void ForceSetup(UIAnswerSlot slot)
        {
            _slot = slot;
        }

        public void Setup(EmojiType type, int index)
        {
            gameObject.SetActive(true);
            _index = index;
            _isDropped = false;
            _isDragged = false;
            _isDragCanceled = true;
            _slot = null;

            if (_moveCor != null)
            {
                StopCoroutine(_moveCor);
                _moveCor = null;
            }

            Type = type;

            Action action = () =>
            {
                _image.sprite = IconManager.Instance.GetSprite(type);
                _image.SetNativeSize();

                this.InvokeWithFrameDelay(() =>
                {
                    _initialPos = _transform.anchoredPosition;
                });
            };

            this.InvokeWithFrameDelay(() =>
            {
                action?.Invoke();
                _transform.localScale = Vector3.zero;
                _transform.DOScale(1f, Delay);
            });
        }

        private void ResetToInitial()
        {
            if (_moveCor != null)
            {
                StopCoroutine(_moveCor);
                _moveCor = null;
            }

            _moveCor = StartCoroutine(MoveToPointCor(_initialPos));

            _isDropped = false;
            _slot = null;
        }

        private void AnswerSlot_Dropped(UIEmojiItem emojiItem, UIAnswerSlot slot)
        {
            if (ReferenceEquals(emojiItem, this))
            {
                _isDropped = true;
                _slot = slot;
            }
        }

        private void UIFreeArea_Dropped(UIEmojiItem item)
        {
            if (ReferenceEquals(item, this))
            {
                ResetToInitial();
            }
        }

        private void TutorialManager_Invoked()
        {
            if (!_isDropped)
            {
                this.InvokeWithDelay(_index * .1f, () =>
                {
                    _tutorialController.Play();
                });
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            var item = eventData.pointerDrag.GetComponent<UIEmojiItem>();

            if (item != null)
            {
                if (!ReferenceEquals(_slot, null))
                {
                    if (item._slot != null)
                    {
                        item._slot.Reset();
                    }

                    _slot.ForceSetup(item);

                    ResetToInitial();
                }
                else
                {
                    if (item._slot != null)
                    {
                        item._slot.Reset();
                    }

                    item.ResetToInitial();
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDragCanceled)
            {
                _isDragged = true;

                _finalPos = UISystem.Instance.Camera.ScreenToWorldPoint(Input.mousePosition).ChangeZ(0);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_isDropped)
            {
                if (_moveCor == null)
                {
                    _moveCor = StartCoroutine(MoveToPointCor(_startPos));

                    DragEnded();
                }
            }

            _isDragged = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Picked();

            _isDragCanceled = false;

            _graphic.raycastTarget = false;
            _startPos = _transform.anchoredPosition;

            _transform.DOScale(.8f, Delay);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _graphic.raycastTarget = true;

            if (!_isDragCanceled)
            {
                if (!_isDragged)
                {
                    if (_moveCor != null)
                    {
                        StopCoroutine(_moveCor);
                        _moveCor = null;
                    }

                    _moveCor = StartCoroutine(MoveToPointCor(_initialPos));

                    if (_slot != null)
                    {
                        Dropped(this);
                    }
                    else
                    {
                        if (_moveCor != null)
                        {
                            StopCoroutine(_moveCor);
                            _moveCor = null;
                        }

                        Clicked(this);
                    }
                }
            }

            _transform.DOScale(1f, Delay);

            Freed();
        }

        private IEnumerator MoveToPointCor(Vector3 finalPoint)
        {
            float moveTime = 0.5f;
            float time = 0f;
            float progress = 0f;

            while (time < moveTime)
            {
                yield return null;
                time += Time.deltaTime;
                progress = time / moveTime;
                _transform.anchoredPosition = Vector3.Lerp(_transform.anchoredPosition, finalPoint, progress);
            }

            _moveCor = null;
        }
    }
}