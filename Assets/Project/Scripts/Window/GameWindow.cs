using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using DG.Tweening;
using System;

namespace EmojiChat.UI
{
    public class GameWindow : Window
    {
        public static event Action<GameWindow> Started = delegate { };

        public const int MessagesLimit = 6;

        [SerializeField]
        private RectTransform _scalableContainer = null;

        [Header("Header")]
        [SerializeField]
        private Image _senderIcon = null;

        [SerializeField]
        private Image _senderSmallBackground = null;

        [SerializeField]
        private GameObject _onlineGroup = null;

        [SerializeField]
        private Button _backButton = null;

        [Header("Message")]
        [SerializeField]
        private TextMeshProUGUI _senderNameTitle = null;

        [SerializeField]
        private TextMeshProUGUI _levelTitle = null;

        [Header("Quiz")]
        [SerializeField]
        private TextMeshProUGUI _quizNameLabel = null;

        [Header("Main Stuff")]
        [SerializeField]
        private TextMeshProUGUI _levelDescription = null;

        [Header("Message")]
        [SerializeField]
        private GameObject[] _messageGroups = null;

        [SerializeField]
        private TextMeshProUGUI _answerTitle = null;

        [SerializeField]
        private UIEmojiItem[] _items = null;

        [SerializeField]
        private UIAnswerSlot[] _answerSlots = null;

        [SerializeField]
        private RectTransform _parent = null;

        [SerializeField]
        private HorizontalLayoutGroup[] _groups = null;

        [SerializeField]
        private VerticalLayoutGroup _scrollGroup = null;

        [SerializeField]
        private RectTransform _backgroundTransform = null;

        [SerializeField]
        private BaseTweenController _bubbleAppearController = null;

        [SerializeField]
        private BaseTweenController _bubbleDisapperController = null;

        [Header("Quiz")]
        [SerializeField]
        private GameObject[] _quizGroups = null;

        [SerializeField]
        private Transform _quizProgressParent = null;

        [SerializeField]
        private UIProgressQuizItem _progressQuizItem = null;

        [SerializeField]
        private Image _quizIcon = null;

        [SerializeField]
        private TextMeshProUGUI _quizDescription = null;

        [SerializeField]
        private RectTransform _quizCard = null;

        [SerializeField]
        private BaseTweenController _quizAppearController = null;

        [SerializeField]
        private BaseTweenController _quizDisappearController = null;

        [Header("Preloaded Prefabs")]
        [SerializeField]
        private MessageItem _textMessagePrefab = null;

        [SerializeField]
        private MessageItem _emojiMessagePrefab = null;

        [Header("Completed Group")]
        [SerializeField]
        private GameObject[] _finishFxs = null;

        private List<MessageItem> _myMessages = new List<MessageItem>();
        private List<MessageItem> _otherMessages = new List<MessageItem>();

        private List<UIProgressQuizItem> _quizProgresses = new List<UIProgressQuizItem>();

        public UIAnswerSlot[] AnswerSlots => _answerSlots;
        public UIEmojiItem[] EmojiItems => _items;

        public override bool IsPopup => false;

        protected override void Start()
        {
            base.Start();

            _backButton.onClick.AddListener(OnBackButtonClick);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            GameManager.Prepared += GameManager_Prepared;
            GameManager.Finished += GameManager_Finished;
            GameManager.Started += GameManager_Started;
            GameManager.Completed += GameManager_Completed;
            TextMessageItem.Changed += TextMessageItem_Changed;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            GameManager.Prepared -= GameManager_Prepared;
            GameManager.Finished -= GameManager_Finished;
            GameManager.Started -= GameManager_Started;
            GameManager.Completed -= GameManager_Completed;
            TextMessageItem.Changed -= TextMessageItem_Changed;
        }

        public override void Preload()
        {
            base.Preload();

            for (int i = 0; i < MessagesLimit; i++)
            {
                _otherMessages.Add(Instantiate(_textMessagePrefab, _parent));
                _myMessages.Add(Instantiate(_emojiMessagePrefab, _parent));
                _quizProgresses.Add(Instantiate(_progressQuizItem, _quizProgressParent));
            }

            _scalableContainer.sizeDelta = UIUtils.GetScreenOffset();
        }

        public override void OnShow()
        {
            base.OnShow();

            var level = AssetsManager.Instance.GetLevel(LocalConfig.LevelIndex);

            bool isQuiz = level.IsQuiz;

            _quizGroups.Do(group => group.SetActive(isQuiz));
            _messageGroups.Do(group => group.SetActive(!isQuiz));
            _onlineGroup.SetActive(!isQuiz);

            _senderIcon.sprite = level.EmojiSprite;
            var color = level.BackgroundColor;
            color.a = 255;
            _senderSmallBackground.color = color;

            if (isQuiz)
            {
                _quizNameLabel.text = level.SenderName;
                _quizAppearController.Reset();
            }
            else
            {
                _senderNameTitle.text = level.SenderName;
                _levelDescription.text = level.LevelDescription;

                _otherMessages.Do(message => message.Preload());
                _myMessages.Do(message => message.Preload());

                _bubbleAppearController.Reset();

                _parent.anchoredPosition = _parent.anchoredPosition.ChangeY(0);
            }

            _finishFxs.Do(fx => fx.SetActive(false));
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        private void RefreshQuizProgress()
        {
            var level = AssetsManager.Instance.GetLevel(LocalConfig.LevelIndex);

            int index;
            for (index = 0; index < level.Presets.Length; index++)
            {
                _quizProgresses[index].gameObject.SetActive(true);
                _quizProgresses[index].Setup(GameManager.Instance.Index <= index, GameManager.Instance.Index < index, GameManager.Instance.Index > index);
            }

            for (; index < _quizProgresses.Count; index++)
            {
                _quizProgresses[index].gameObject.SetActive(false);
            }
        }

        private void QuizCardScale()
        {
            _quizAppearController.Play();
        }

        private void OnBackButtonClick()
        {
            UISystem.ShowWindow<MainWindow>();
        }

        public void GameManager_Prepared(List<EmojiType> emojis, AssetsManager.MessagePreset preset)
        {
            var level = AssetsManager.Instance.GetLevel(LocalConfig.LevelIndex);
            int index = GameManager.Instance.Index;
            bool isQuiz = level.IsQuiz;
            int i = 0;

            Action emojiesAction = () =>
            {
                _groups.Do(group => group.enabled = false);

                for (i = 0; i < emojis.Count; i++)
                {
                    _items[i].Setup(emojis[i], i);
                }

                for (; i < _items.Length; i++)
                {
                    _items[i].gameObject.SetActive(false);
                }

                for (i = 0; i < _answerSlots.Length; i++)
                {
                    _answerSlots[i].Reset();
                    _answerSlots[i].gameObject.SetActive(i < preset.EmojiSequence.Length);
                }

                _groups.Do(group => group.enabled = true);
            };

            for (i = 0; i < emojis.Count; i++)
            {
                _items[i].Hide();
            }

            if (index == 0)
            {
                emojiesAction.Invoke();
            }
            else
            {
                this.InvokeWithDelay(UIEmojiItem.Delay, () =>
                {
                    emojiesAction.Invoke();
                });
            }

            if (isQuiz)
            {
                _quizDescription.text = preset.Answer;
                _quizIcon.sprite = preset.QuizIcon;

                RefreshQuizProgress();

                if (index == 0)
                {
                    QuizCardScale();
                }
            }
            else
            {
                Action action = () =>
                {
                    if (!preset.Answer.Equals(string.Empty))
                    {
                        _bubbleAppearController.Play();
                        _answerTitle.text = preset.Answer;
                    }
                };

                if (index == 0)
                {
                    action.Invoke();
                }
                else
                {
                    _bubbleDisapperController.Play();

                    this.InvokeWithDelay(.3f, () =>
                    {
                        action.Invoke();
                    });
                }

                _otherMessages[index].Setup(false);
                _myMessages[index].Setup(true);


                this.InvokeWithDelay(.1f, () =>
                {
                    if (_backgroundTransform.IsOverlapsOne(_myMessages[GameManager.Instance.Index].Transform, _parent.anchoredPosition.y + _scrollGroup.spacing * 2))
                    {
                        StartCoroutine(ScrollCor());
                    }
                });
            }

            Started(this);
        }

        private void GameManager_Finished()
        {
            var level = AssetsManager.Instance.GetLevel(LocalConfig.LevelIndex);

            if (level.IsQuiz)
            {
                RefreshQuizProgress();
            }

            _finishFxs.Do(fx => fx.SetActive(true));
        }

        private void GameManager_Started()
        {
            _levelTitle.text = $"LEVEL {LocalConfig.LevelIndex + 1}";
        }

        private void GameManager_Completed(int index)
        {
            var level = AssetsManager.Instance.GetLevel(LocalConfig.LevelIndex);

            if (!level.IsQuiz)
            {
                var preset = GameManager.Instance.GetMessagePreset();

                if (!preset.Answer.Equals(string.Empty))
                {
                    _myMessages[index].Appear();
                }
            }
            else
            {
                if (!(GameManager.Instance.LevelLength <= index + 1))
                {
                    _quizDisappearController.Play();

                    this.InvokeWithDelay(GameManager.DelayBetweenStage, () =>
                    {
                        QuizCardScale();
                    });
                }
            }
        }

        private void TextMessageItem_Changed()
        {
            _scrollGroup.enabled = false;

            _scrollGroup.enabled = true;
        }

        private IEnumerator ScrollCor()
        {
            float effectTime = 0.3f;
            float time = 0f;
            float progress = 0f;
            int index = GameManager.Instance.Index;

            float startPosY = _parent.anchoredPosition.y;
            float endPos = Mathf.Abs(_myMessages[index].Transform.anchoredPosition.y) - _backgroundTransform.rect.height;

            if (endPos < 0)
            {
                yield break;
            }

            while (time < effectTime)
            {
                yield return null;

                time += Time.deltaTime;

                progress = time / effectTime;
                _parent.anchoredPosition = _parent.anchoredPosition.ChangeY(Mathf.Lerp(startPosY, endPos, progress));
            }
        }
    }
}