using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace EmojiChat.UI
{
    public class ResultPopup : Window
    {
        public static event Action ContinueClicked = delegate { };

        [SerializeField]
        private Button _continueButton = null;

        [SerializeField]
        private TextMeshProUGUI _levelLabel = null;

        [SerializeField]
        private UIProgress _progress = null;

        [SerializeField]
        private Image _resultEmoji = null;

        [SerializeField]
        private BaseTweenController _popupController = null;

        public override bool IsPopup => true;

        protected override void Start()
        {
            base.Start();

            _continueButton.onClick.AddListener(OnContinueButtonClick);
        }

        public override void OnShow()
        {
            base.OnShow();

            _levelLabel.text = $"LEVEL {LocalConfig.LevelIndex}";

            _resultEmoji.sprite = AssetsManager.Instance.ResultEmojies.RandomElement();

            _progress.Setup(true);

            _popupController.Play();
        }

        private void OnContinueButtonClick()
        {
            UISystem.ShowWindow<MainWindow>();
        }
    }
}