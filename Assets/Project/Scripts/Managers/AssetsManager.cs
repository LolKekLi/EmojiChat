using System.Collections.Generic;
using UnityEngine;

namespace EmojiChat
{
    public partial class AssetsManager : GameObjectSingleton<AssetsManager>
    {
        [SerializeField]
        private LevelPreset[] _presets = null;

        [SerializeField]
        private ProgressPreset[] _progressPresets = null;

        [SerializeField]
        private Material _uiFast = null;

        [SerializeField]
        private Material _uiGray = null;

        [SerializeField]
        private int _percentPerCompletedLevel = 3;

        [SerializeField]
        private Sprite[] _resultEmojies = null;

        [SerializeField]
        private float _otherMessageDelay = .2f;

        [SerializeField]
        private float _myMessageDelay = 0.5f;

        public Material UIFast => _uiFast;
        public Material UIGray => _uiGray;

        public int PercentPerCompletedLevel => _percentPerCompletedLevel;

        public float OtherMessageDelay => _otherMessageDelay;
        public float MyMessageDelay => _myMessageDelay;

        public Sprite[] ResultEmojies => _resultEmojies;

        public LevelPreset GetLevel(int index)
        {
            if (index == 0)
            {
                return _presets[0]; 
            }

            return _presets[index % _presets.Length];
        }

        public ProgressPreset GetCurrentProgress(int levelIndex, out int percent)
        {
            int progress = levelIndex * _percentPerCompletedLevel;
            int index = progress / ProgressPreset.MaxPercent;
            percent = progress % ProgressPreset.MaxPercent;

            if (levelIndex == 0)
            {
                return _progressPresets[0];
            }

            return _progressPresets[index % _progressPresets.Length];
        }

        public LevelPreset[] GetContextLevels(out int activeIndex)
        {
            int levelIndex = LocalConfig.LevelIndex;
            int remainderIndex = levelIndex % _presets.Length;
            List<LevelPreset> levels = new List<LevelPreset>();

            activeIndex = levelIndex % UI.MainWindow.MaxStageCount;

            int offset = -activeIndex;

            for (int i = 0; i < UI.MainWindow.MaxStageCount; i++)
            {
                levels.Add(GetLevel(levelIndex + offset));

                offset++;
            }

            return levels.ToArray();
        }
    }
}