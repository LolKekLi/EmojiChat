using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace EmojiChat.UI
{
    public class EmojiMessageItem : MessageItem
    {
        private const float Delay = 0.2f;
        private const float EffectTime = 0.2f;
        private const float FinalScale = 0.5566591f;

        [SerializeField]
        private Image[] _emojies = null;

        [SerializeField]
        private Image _messageIcon = null;

        [SerializeField]
        private BaseTweenController _emojiAppearController = null;

        public override void Preload()
        {
            base.Preload();

            _emojies.Do(emoji => emoji.transform.localScale = Vector3.zero);
            _messageIcon.transform.localScale = Vector3.zero;
        }

        public override void Appear()
        {
            base.Appear();

            if (_preset.IsSpecial)
            {
                this.InvokeWithDelay(Delay, () =>
                {
                    _messageIcon.transform.DOScale(FinalScale, EffectTime);
                });
            }
            else
            {
                _emojiAppearController.Play();
            }
        }

        public override void Setup(bool isMe, Sprite faceSprite = null)
        {
            base.Setup(isMe, faceSprite);

            _emojies.Do(emoji => emoji.gameObject.SetActive(!_preset.IsSpecial));
            _messageIcon.gameObject.SetActive(_preset.IsSpecial);

            if (_preset.IsSpecial)
            {
                _messageIcon.sprite = _preset.MessageIcon;
                _messageIcon.SetNativeSize();
            }
            else
            {
                var emojiSequence = _preset.EmojiSequence;

                if (emojiSequence.Length == 0)
                {
                    gameObject.SetActive(false);
                    return;
                }

                int i;
                for (i = 0; i < emojiSequence.Length; i++)
                {
                    _emojies[i].gameObject.SetActive(true);
                    _emojies[i].sprite = IconManager.Instance.GetSprite(emojiSequence[i]);
                }

                for (; i < _emojies.Length; i++)
                {
                    _emojies[i].gameObject.SetActive(false);
                }
            }

            _typingController.Play();
        }
    }
}