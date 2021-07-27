using UnityEngine;

namespace EmojiChat.UI
{
    public class UIProgressQuizItem : MonoBehaviour
    {
        [SerializeField]
        private GameObject _activeGroup = null;

        [SerializeField]
        private GameObject _completedGroup = null;

        public void Setup(bool isActive, bool isNext, bool isCompleted)
        {
            _activeGroup.SetActive(isActive);
            _completedGroup.SetActive(isCompleted);

            transform.localScale = isActive && !isNext ? new Vector3(1.4f, 1.4f, 1.4f) : Vector3.one;
        }
    }
}