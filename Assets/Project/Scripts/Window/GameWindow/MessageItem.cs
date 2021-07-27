using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace EmojiChat.UI
{
    public abstract class MessageItem : MonoBehaviour
    {
        [SerializeField]
        protected GameObject _contentGroup = null;

        [SerializeField]
        protected Image _myFaceIcon = null;

        [SerializeField]
        protected Image _faceIcon = null;

        [SerializeField]
        private Image _avatarBackground = null;

        [SerializeField]
        protected Image _backgroundImage = null;

        [SerializeField]
        protected GameObject _typingGroup = null;

        [SerializeField]
        protected BaseTweenController _appearController = null;

        [SerializeField]
        protected BaseTweenController _typingController = null;

        protected RectTransform _transform = null;

        protected AssetsManager.MessagePreset _preset = null;

        public RectTransform Transform => _transform;

        protected virtual void Awake()
        {
            _transform = (RectTransform)transform;
        }

        public virtual void Preload()
        {
            _contentGroup.SetActive(false);
            _appearController.Reset();
        }

        public virtual void Appear()
        {
            _typingGroup.SetActive(false);
        }

        public virtual void Setup(bool isMe, Sprite faceSprite = null)
        {
            gameObject.SetActive(true);

            _contentGroup.SetActive(true);

            this.InvokeWithDelay(isMe ? AssetsManager.Instance.MyMessageDelay : AssetsManager.Instance.OtherMessageDelay, () =>
            {
                _appearController.Play();
            });

            if (faceSprite)
            {
                _faceIcon.sprite = faceSprite;
            }

            _preset = GameManager.Instance.GetMessagePreset();
            var level = AssetsManager.Instance.GetLevel(LocalConfig.LevelIndex);
            if (isMe)
            {
                _myFaceIcon.sprite = level.MyFaceSprite;
                _avatarBackground.color = level.MyBackgroundColor;
            }
            else
            {
                _faceIcon.sprite = level.EmojiSprite;
                _avatarBackground.color = level.BackgroundColor;
            }

            _typingGroup.SetActive(true);

            if (!isMe)
            {
                Appear();
            }
        }
    }
}