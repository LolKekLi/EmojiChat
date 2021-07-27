using UnityEngine;
using UnityEngine.UI;

namespace EmojiChat.UI
{
    public class MainWindow : Window
    {
        public const int MaxStageCount = 6;

        [SerializeField]
        private UIStageElement[] _stages = new UIStageElement[MaxStageCount];

        [SerializeField]
        private UIProgress _progress = null;

        [SerializeField]
        private Button _startButton = null;
        [SerializeField]
        private Button _quizButton = null;

        [SerializeField]
        private GameObject _commonGroup = null;
        [SerializeField]
        private GameObject _quizGroup = null;

        [SerializeField]
        private BaseTweenController[] _appearController = null;

        public override bool IsPopup => false;

        protected override void Start()
        {
            base.Start();

            _startButton.onClick.AddListener(OnStartButtonClick);
            _quizButton.onClick.AddListener(OnStartButtonClick);
        }

        public override void OnShow()
        {
            base.OnShow();

            var levels = AssetsManager.Instance.GetContextLevels(out int activeIndex);

#if UNITY_EDITOR
            if (_stages.Length != MaxStageCount)
            {
                Debug.LogException(new System.Exception($"Stages length not equals MaxStageCount: {_stages.Length}!"));

                return;
            }

            if (levels.Length != MaxStageCount)
            {
                Debug.LogException(new System.Exception($"Levels length not equals MaxStageCount: {levels.Length}!"));

                return;
            }
#endif

            for (int i = 0; i < MaxStageCount; i++)
            {
                _stages[i].Setup(levels[i], i, activeIndex);
            }

            _commonGroup.SetActive(!levels[activeIndex].IsQuiz);
            _quizGroup.SetActive(levels[activeIndex].IsQuiz);

            _progress.Setup();

            _appearController.Do(anim => anim.Play());
        }

        private void OnStartButtonClick()
        {
             UISystem.ShowWindow<GameWindow>();
        }
    }
}