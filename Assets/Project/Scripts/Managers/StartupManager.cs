using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using EmojiChat.UI;

namespace EmojiChat
{
    public class StartupManager : MonoBehaviour
    {
        public const string StartupScene = "Startup";
        public const string TutorialLevel = "TutorialLevel";
        public const string MainUI = "UICommon";

        IEnumerator Start()
        {
            DontDestroyOnLoad(gameObject);

            Init();

            yield return StartCoroutine(LoadScene());

            UISystem.ShowWindow<MainWindow>();

            Application.targetFrameRate = 60;

            Destroy(gameObject);
        }

        private void Init()
        {
            AssetsManager.GetInstance();
            PoolManager.GetInstance();
            IconManager.GetInstance();
            HapticFeedbackManager.GetInstance();

            if (LocalConfig.BasicTutorialNeeded)
            {
                TutorialUIManager.GetInstance();
            }
        }

        IEnumerator LoadScene()
        {
            yield return SceneManager.LoadSceneAsync(MainUI);

            yield return SceneManager.LoadSceneAsync(TutorialLevel);
        }
    }
}