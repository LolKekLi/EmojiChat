using UnityEngine;
using EmojiChat.UI;
using System;
using System.Collections;

namespace EmojiChat
{
    public class TutorialManager : MonoBehaviour
    {
        public static event Action Invoked = delegate { };

        [SerializeField]
        private float _delay = 5f;

        private float _nextTutorialTime = 0f;

        private Coroutine _tutorialCor = null;

        private void OnEnable()
        {
            GameManager.Finished += GameManager_Finished;
            GameManager.Started += GameManager_Started;
            UIEmojiItem.Freed += UIEmojiItem_Freed;
            UIEmojiItem.Picked += UIEmojiItem_Picked;
        }

        private void OnDisable()
        {
            GameManager.Finished -= GameManager_Finished;
            GameManager.Started -= GameManager_Started;
            UIEmojiItem.Freed -= UIEmojiItem_Freed;
            UIEmojiItem.Picked -= UIEmojiItem_Picked;
        }

        private void StopCor()
        {
            if (_tutorialCor != null)
            {
                StopCoroutine(_tutorialCor);
                _tutorialCor = null;
            }
        }

        private void GameManager_Finished()
        {
            StopCor();
        }

        private void GameManager_Started()
        {
            StopCor();

            _tutorialCor = StartCoroutine(TutorialCor());
        }

        private void UIEmojiItem_Freed()
        {
            StopCor();

            _tutorialCor = StartCoroutine(TutorialCor());
        }

        private void UIEmojiItem_Picked()
        {
            StopCor();
        }

        private IEnumerator TutorialCor()
        {
            var waiter = new WaitForSeconds(1f);
            _nextTutorialTime = Time.realtimeSinceStartup;

            while (true)
            {
                yield return waiter;

                if (_nextTutorialTime + _delay < Time.realtimeSinceStartup)
                {
                    _nextTutorialTime = Time.realtimeSinceStartup;

                    Invoked();
                }
            }
        }
    }
}