using System;
using UnityEngine;

namespace EmojiChat
{
    public partial class AssetsManager
    {
        [Serializable]
        public class MessagePreset
        {
            [SerializeField]
            private string _message = string.Empty;
            [SerializeField]
            private string _answer = string.Empty;
            [SerializeField]
            private EmojiType[] _emojiSequence;
            [SerializeField]
            private EmojiType[] _forcedEmojies;
            [SerializeField]
            private int _maxEmojiSize = 0;
            [SerializeField]
            private Sprite _messageIcon = null;
            [SerializeField]
            private Sprite _quizIcon = null;

            public string Message => _message;
            public string Answer => _answer;
            public EmojiType[] EmojiSequence => _emojiSequence;
            public EmojiType[] ForcedEmojies => _forcedEmojies;
            public int MaxEmojiSize => _maxEmojiSize;
            public Sprite MessageIcon => _messageIcon;
            public bool IsSpecial => _messageIcon != null;
            public Sprite QuizIcon => _quizIcon;
            public bool IsQuiz => _quizIcon != null;
        }

        [Serializable]
        public class LevelPreset
        {
            [SerializeField]
            private Sprite _myFaceSprite = null;

            [SerializeField]
            private Color _myBackgroundColor = default;

            [SerializeField]
            private Sprite _emojiSprite = null;

            [SerializeField]
            private Color _backgroundColor = default;

            [SerializeField]
            private string _senderName = string.Empty;

            [SerializeField]
            private string _levelDescription = string.Empty;

            [SerializeField]
            private MessagePreset[] _presets = null;

            [SerializeField]
            private bool _isQuiz = false;

            public MessagePreset[] Presets => _presets;
            public Sprite EmojiSprite => _emojiSprite;
            public Sprite MyFaceSprite => _myFaceSprite;
            public Color MyBackgroundColor => _myBackgroundColor;
            public Color BackgroundColor => _backgroundColor;
            public string SenderName => _senderName;
            public string LevelDescription => _levelDescription;
            public bool IsQuiz => _isQuiz;

            public MessagePreset GetMessagePreset(int index)
            {
                if (index >= _presets.Length)
                {
#if UNITY_EDITOR
                    Debug.LogException(new Exception($"Not found {nameof(MessagePreset)} with index: {index}"));
#endif
                    return null;
                }

                return _presets[index];
            }
        }

        [Serializable]
        public class ProgressPreset
        {
            public const int MaxPercent = 100;

            [SerializeField]
            private string _progressName = string.Empty;

            [SerializeField]
            private Sprite _progressIcon = null;

            public string ProgressName => _progressName;
            public Sprite ProgressIcon => _progressIcon;
        }
    }
}