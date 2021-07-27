using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace EmojiChat.UI
{
    public class UIProgress : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _progressNameLabel = null;

        [SerializeField]
        private TextMeshProUGUI _progressPercentLabel = null;

        [SerializeField]
        private Image _progressIcon = null;

        [SerializeField]
        private SlicedFilledImage _slider = null;

        [SerializeField]
        private Image _fillIcon = null;

        [SerializeField]
        private float _effectTime = 1f;

        public void Setup(bool isUpdate = false)
        {
            var progress = AssetsManager.Instance.GetCurrentProgress(LocalConfig.LevelIndex, out int percent);

            _progressNameLabel.text = progress.ProgressName;
            _progressIcon.sprite = progress.ProgressIcon;

            if (isUpdate)
            {
                StartCoroutine(ProgressCor(percent));
            }
            else
            {
                _progressPercentLabel.text = $"{percent}%";
                _slider.fillAmount = (float)percent / AssetsManager.ProgressPreset.MaxPercent;
            }
        }

        private IEnumerator ProgressCor(int toValue)
        {
            float time = 0f;
            float progress = 0;
            float fromValue = Mathf.Max(0, toValue - AssetsManager.Instance.PercentPerCompletedLevel);
            float currentValue = 0f;

            while (time < _effectTime)
            {
                yield return null;

                time += Time.deltaTime;
                progress = time / _effectTime;

                currentValue = Mathf.Lerp(fromValue, toValue, progress) / AssetsManager.ProgressPreset.MaxPercent;
                _slider.fillAmount = currentValue;
                _progressPercentLabel.text = $"{(int)(currentValue * 100)}%";
            }

            _progressPercentLabel.text = $"{toValue}%";
        }
    }
}