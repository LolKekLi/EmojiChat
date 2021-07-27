using System;
using System.Collections.Generic;
using UnityEngine;
using EmojiChat.UI;
using System.Linq;

namespace EmojiChat
{
    public class GameManager : MonoBehaviour
    {
        public static event Action Started = delegate { };
        public static event Action Finished = delegate { };
        public static event Action<List<EmojiType>, AssetsManager.MessagePreset> Prepared = delegate { };
        public static event Action<int> Completed = delegate { };
        public static event Action Failed = delegate { };

        public const float DelayBetweenStage = 0.9f;
        public const float DelayBetweenFinish = 1.5f;

        private int _index = 0;
        private AssetsManager.LevelPreset _preset = null;

        private List<EmojiType> _emojies = new List<EmojiType>();
        private List<UIEmojiItem> _emojiItems = new List<UIEmojiItem>();

        public static GameManager Instance
        {
            get;
            private set;
        }

        public bool IsFinished => _index >= _preset.Presets.Length;
        public int LevelLength => _preset.Presets.Length;
        public int Index => _index;

        private void Start()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            UIAnswerSlot.Dropped += AnswerSlot_Dropped;
            UIAnswerSlot.Freed += UIAnswerSlot_Freed;
            UISystem.OnShown += UISystem_OnShown;
            UIFreeArea.Dropped += UIFreeArea_Dropped;
            UIEmojiItem.Dropped += UIEmojiItem_Dropped;
            ResultPopup.ContinueClicked += ResultPopup_ContinueClicked;
        }

        private void OnDisable()
        {
            UIAnswerSlot.Dropped -= AnswerSlot_Dropped;
            UIAnswerSlot.Freed -= UIAnswerSlot_Freed;
            UISystem.OnShown -= UISystem_OnShown;
            UIFreeArea.Dropped -= UIFreeArea_Dropped;
            UIEmojiItem.Dropped -= UIEmojiItem_Dropped;
            ResultPopup.ContinueClicked -= ResultPopup_ContinueClicked;
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                LocalConfig.LevelIndex++;
                LoadLevel();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                LocalConfig.LevelIndex--;
                LoadLevel();
            }
        }
#endif

        public AssetsManager.MessagePreset GetMessagePreset()
        {
            return _preset.GetMessagePreset(_index);
        }

        private void LoadLevel()
        {
            _preset = AssetsManager.Instance.GetLevel(LocalConfig.LevelIndex);

            _emojies.Clear();
            _emojiItems.Clear();
            _index = 0;

            PrepareStage();

            Started();
        }

        private void CompleteStage()
        {
            Completed(_index);

            this.InvokeWithDelay(DelayBetweenStage, () =>
            {
                _index++;
                _emojies.Clear();
                _emojiItems.Clear();

                if (_index < _preset.Presets.Length)
                {
                    PrepareStage();
                }
                else
                {
                    FinishLevel();
                }
            });
        }

        private void PrepareStage()
        {
            var messagePreset = GetMessagePreset();
            var emojiTypes = ((EmojiType[])Enum.GetValues(typeof(EmojiType))).ToList();
            List<EmojiType> emojies = new List<EmojiType>();
            EmojiType emoji;

            for (int i = 0; i < messagePreset.EmojiSequence.Length; i++)
            {
                emoji = messagePreset.EmojiSequence[i];

                emojies.Add(emoji);
                emojiTypes.Remove(emoji);
            }

            for (int index = 0; index < messagePreset.ForcedEmojies.Length; index++)
            {
                emoji = messagePreset.ForcedEmojies[index];

                emojies.Add(emoji);
                emojiTypes.Remove(emoji);
            }

            for (int i = emojies.Count; i < messagePreset.MaxEmojiSize; i++)
            {
                emoji = emojiTypes.RandomElement();

                emojies.Add(emoji);
                emojiTypes.Remove(emoji);
            }

            Prepared(emojies.Shuffle().ToList(), messagePreset);

            if (messagePreset.Answer.Equals(string.Empty))
            {
                this.InvokeWithDelay(.5f, () =>
                {
                    CompleteStage();
                });
            }
        }

        private void FinishLevel()
        {
            Finished();

            LocalConfig.LevelIndex++;

            this.InvokeWithDelay(DelayBetweenFinish, () =>
            {
                UISystem.ShowWindow<ResultPopup>();
            });
        }

        private void Process()
        {
            var messagePreset = GetMessagePreset();

            if (messagePreset.EmojiSequence.DuplicateIntersect(_emojies).Count() == messagePreset.EmojiSequence.Count())
            {
                CompleteStage();
            }
            else if (_emojies.Count() == messagePreset.EmojiSequence.Count())
            {
                this.InvokeWithDelay(UIAnswerSlot.Delay, () =>
                {
                    Failed();
                });
            }
        }

        private void AnswerSlot_Dropped(UIEmojiItem item, UIAnswerSlot slot)
        {
            if (!ReferenceEquals(item, null))
            {
                _emojies.Add(item.Type);
                _emojiItems.Add(item);

                Process();
            }
        }

        private void UIAnswerSlot_Freed(UIEmojiItem item)
        {
            if (item != null)
            {
                _emojies.Remove(item.Type);
            }
        }

        private void UISystem_OnShown(Window window)
        {
            var wndType = window.GetType();

            if (wndType == typeof(GameWindow))
            {
                LoadLevel();
            }
        }

        private void UIFreeArea_Dropped(UIEmojiItem obj)
        {
            _emojies.Remove(obj.Type);
        }

        private void UIEmojiItem_Dropped(UIEmojiItem obj)
        {
            _emojies.Remove(obj.Type);
        }

        private void ResultPopup_ContinueClicked()
        {
            LoadLevel();
        }
    }
}