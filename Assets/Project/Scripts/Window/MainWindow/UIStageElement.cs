using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace EmojiChat.UI
{
    public class UIStageElement : MonoBehaviour
    {
        [Header("Main Group")]
        [SerializeField]
        private Image _emojiIcon = null;

        [SerializeField]
        private Image _emojiBackground = null;

        [Header("Inactive Group")]
        [SerializeField]
        private GameObject _inactiveGroup = null;

        [Header("Active Group")]
        [SerializeField]
        private GameObject _activeGroup = null;

        [SerializeField]
        private TextMeshProUGUI _levelLabel = null;

        [SerializeField]
        private Button _startButton = null;

        [SerializeField]
        private GameObject _incommingMessageBubble = null;

        [Header("Conpleted Group")]
        [SerializeField]
        private GameObject _completedLink = null;

        [SerializeField]
        private GameObject[] _completedGroups = null;

        [Header("Quiz Group")]
        [SerializeField]
        private GameObject _quizGroup = null;

        private int _index = 0;

        private void Start()
        {
            _startButton.onClick.AddListener(OnStartButtonClick);
        }

        public void Setup(AssetsManager.LevelPreset level, int currentIndex, int activeIndex)
        {
            _index = currentIndex;

            bool isActive = _index == activeIndex;
            bool isCompleted = _index < activeIndex;
            bool isLocked = _index > activeIndex;

            _emojiIcon.sprite = level.EmojiSprite;
            _emojiIcon.material = isCompleted || (level.IsQuiz && isLocked) ? AssetsManager.Instance.UIGray : AssetsManager.Instance.UIFast;
            _emojiIcon.enabled = isActive || isCompleted || level.IsQuiz;

            var color = level.BackgroundColor;
            color.a = 255;
            _emojiBackground.color = color;

            _emojiBackground.enabled = isActive;

            _activeGroup.SetActive(isActive);
            _completedLink.SetActive(isActive || isCompleted);
            _completedGroups.Do(group => group.SetActive(isCompleted));
            _inactiveGroup.SetActive(isLocked);
            _quizGroup.SetActive(level.IsQuiz && (isActive || isLocked));
            _incommingMessageBubble.gameObject.SetActive(isActive && !level.IsQuiz);

            _startButton.enabled = isActive;

            _levelLabel.gameObject.SetActive(isLocked && !level.IsQuiz);

            _levelLabel.text = $"{LocalConfig.LevelIndex + currentIndex - activeIndex + 1}";
        }

        private void OnStartButtonClick()
        {
            UISystem.ShowWindow<GameWindow>();
        }
    }
}