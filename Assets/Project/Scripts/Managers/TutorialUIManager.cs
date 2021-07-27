using UnityEngine;
using EmojiChat.UI;
using System.Linq;

namespace EmojiChat
{
    public class TutorialUIManager : GameObjectSingleton<TutorialUIManager>
    {
        [SerializeField]
        private GameObject _handPrefab = null;

        private UITutorialHand _spawnedHand = null;

        private GameWindow _window = null;

        protected override void OnEnable()
        {
            base.OnEnable();

            GameWindow.Started += GameWindow_Started;
            GameManager.Finished += GameManager_Finished;
            GameManager.Completed += GameManager_Completed;
            UISystem.OnShown += UISystem_OnShown;
            UIEmojiItem.Picked += UIEmojiItem_Picked;
            UIFreeArea.Dropped += UIFreeArea_Dropped;
            UIEmojiItem.DragEnded += UIEmojiItem_DragEnded;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            GameWindow.Started -= GameWindow_Started;
            GameManager.Finished -= GameManager_Finished;
            GameManager.Completed -= GameManager_Completed;
            UISystem.OnShown -= UISystem_OnShown;
            UIEmojiItem.Picked -= UIEmojiItem_Picked;
            UIFreeArea.Dropped -= UIFreeArea_Dropped;
            UIEmojiItem.DragEnded -= UIEmojiItem_DragEnded;
        }

        private void GameManager_Completed(int obj)
        {
            FreeHand();

            LocalConfig.BasicTutorialNeeded = false;
        }

        private void UIEmojiItem_DragEnded()
        {
            StartTutorial();
        }

        private void UIFreeArea_Dropped(UIEmojiItem obj)
        {
            StartTutorial();
        }

        private void UIEmojiItem_Picked()
        {
            FreeHand();
        }

        private void UISystem_OnShown(Window window)
        {
            var wndType = window.GetType();

            if (wndType == typeof(MainWindow))
            {
                FreeHand();
            }
        }

        private void GameManager_Finished()
        {
            FreeHand();

            LocalConfig.BasicTutorialNeeded = false;
        }

        private void StartTutorial()
        {
            if (LocalConfig.BasicTutorialNeeded)
            {
                var preset = GameManager.Instance.GetMessagePreset();
                var emojiType = preset.EmojiSequence.FirstOrDefault();

                this.InvokeWithDelay(.3f, () =>
                {
                    var answerSlot = _window.AnswerSlots.FirstOrDefault();
                    var emojiItem = _window.EmojiItems.FirstOrDefault(item => item.Type == emojiType);

                    SpawnAnimatedHand(UISystem.Instance.Camera.WorldToViewportPoint(emojiItem.transform.position), UISystem.Instance.Camera.WorldToViewportPoint(answerSlot.transform.position));
                });
            }
        }

        private void GameWindow_Started(GameWindow window)
        {
            if (LocalConfig.BasicTutorialNeeded)
            {
                _window = window;

                StartTutorial();
            }
        }

        public UITutorialHand SpawnAnimatedHand(Vector2 pos, Vector2 endPos, Vector2 offset = default, float rotate = 0)
        {
            FreeHand();
            var hand = SetupHand(pos, offset, rotate);
            hand.SetupAnimatedHand(endPos);

            return hand;
        }

        private UITutorialHand SetupHand(Vector2 pos, Vector2 offset = default, float rotate = 0, Transform parent = null)
        {
            var handGO = Instantiate(_handPrefab, parent ? parent : transform);
            _spawnedHand = handGO.GetComponent<UITutorialHand>();

            if (_spawnedHand != null)
            {
                _spawnedHand.SetupAchorePos(pos, offset, rotate);
            }

            return _spawnedHand;
        }

        public static void FreeHand()
        {
            if (Instance._spawnedHand != null)
            {
                Instance._spawnedHand.Free();
                Instance._spawnedHand = null;
            }
        }
    }
}