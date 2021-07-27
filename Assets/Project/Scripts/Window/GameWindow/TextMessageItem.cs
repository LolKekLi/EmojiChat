using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace EmojiChat.UI
{
    public class TextMessageItem : MessageItem
    {
        public static event Action Changed = delegate { };

        private const float MaxSize = 650f;

        [SerializeField]
        private TextMeshProUGUI _messageTitle = null;

        [SerializeField]
        private LayoutElement _layoutElement = null;

        public override void Setup(bool isMe, Sprite faceSprite = null)
        {
            base.Setup(isMe, faceSprite);

            if (_preset.Message.Equals(string.Empty))
            {
                gameObject.SetActive(false);
                return;
            }

            _layoutElement.preferredWidth = -1;

            if (isMe)
            {

            }
            else
            {
                _messageTitle.text = _preset.Message;
            }

            this.InvokeWithFrameDelay(() =>
            {
                this.InvokeWithFrameDelay(() =>
                {
                    if (_messageTitle.textBounds.size.x > MaxSize)
                    {
                        _layoutElement.preferredWidth = MaxSize;
                    }

                    this.InvokeWithFrameDelay(() =>
                    {
                        _transform.sizeDelta = new Vector2(_transform.sizeDelta.x, _backgroundImage.rectTransform.sizeDelta.y);

                        Changed();
                    });
                });
            });
        }
    }
}