using UnityEngine;
using System.Collections;

namespace EmojiChat
{
    public class IOSHapticFeedbackExample : MonoBehaviour
    {
        void OnGUI()
        {
            for (int i = 0; i < 7; i++)
            {
                if (GUI.Button(new Rect(0, i * 60, 300, 50), "Trigger " + (HapticFeedbackManager.FeedbackType)i))
                {
                    HapticFeedbackManager.Instance.Trigger((HapticFeedbackManager.FeedbackType)i);
                }
            }
        }
    }
}