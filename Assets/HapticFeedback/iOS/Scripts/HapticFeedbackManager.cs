using HapticFeedback.Android;

#if (UNITY_IOS && !UNITY_EDITOR)
using System.Runtime.InteropServices;
#endif

namespace EmojiChat
{
    public class HapticFeedbackManager : GameObjectSingleton<HapticFeedbackManager>
    {
        public enum FeedbackType { SelectionChange, ImpactLight, ImpactMedium, ImpactHeavy, Success, Warning, Failure, None };
        /// <summary>
        /// Defines which feedback generators will be used
        /// This prevents the instantiation of feedback generators that are not used.
        /// </summary>
        public FeedbackTypeSettings usedFeedbackTypes = new FeedbackTypeSettings();

        protected override void Init()
        {
            base.Init();

            for (int i = 0; i < 7; i++)
            {
                if (FeedbackIdSet(i))
                    InstantiateFeedbackGenerator(i);
            }

            GameManager.Finished += GameManager_Finished;
            GameManager.Completed += GameManager_Completed;
        }

        protected override void DeInit()
        {
            base.DeInit();

            for (int i = 0; i < 7; i++)
            {
                if (FeedbackIdSet(i))
                    ReleaseFeedbackGenerator(i);
            }

            GameManager.Finished -= GameManager_Finished;
            GameManager.Completed -= GameManager_Completed;
        }

        [System.Serializable]
        public class FeedbackTypeSettings
        {
            public bool SelectionChange = true, ImpactLight = true, ImpactMedium = true, ImpactHeavy = true;
            public bool NotificationSuccess = true, NotificationWarning = true, NotificationFailure = true;

            public bool Notifications
            {
                get
                {
                    return NotificationSuccess || NotificationWarning || NotificationFailure;
                }
            }
        }

        protected bool FeedbackIdSet(int id)
        {
            return ((id == 0 && usedFeedbackTypes.SelectionChange)
                 || (id == 1 && usedFeedbackTypes.ImpactLight)
                 || (id == 2 && usedFeedbackTypes.ImpactMedium)
                 || (id == 3 && usedFeedbackTypes.ImpactHeavy)
                || ((id == 4 || id == 5 || id == 6) && usedFeedbackTypes.Notifications));
        }

        //link to native functions
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport ("__Internal")]
    private static extern void _instantiateFeedbackGenerator(int id);

    [DllImport ("__Internal")]
    private static extern void _prepareFeedbackGenerator(int id);

    [DllImport ("__Internal")]
    private static extern void _triggerFeedbackGenerator(int id, bool advanced);

    [DllImport ("__Internal")]
    private static extern void _releaseFeedbackGenerator(int id);

#else
        //Instantiate placeholders that do nothing for other platforms than iOS
        private void _instantiateFeedbackGenerator(int id) { }
        private void _prepareFeedbackGenerator(int id) { }
        private void _triggerFeedbackGenerator(int id, bool advanced) { }
        private void _releaseFeedbackGenerator(int id) { }

#endif

        protected void InstantiateFeedbackGenerator(int id)
        {
            _instantiateFeedbackGenerator(id);
        }

        protected void PrepareFeedbackGenerator(int id)
        {
            _prepareFeedbackGenerator(id);
        }

        protected void TriggerFeedbackGenerator(int id, bool advanced)
        {
            _triggerFeedbackGenerator(id, advanced);
        }

        protected void ReleaseFeedbackGenerator(int id)
        {
            _releaseFeedbackGenerator(id);
        }

        /// <summary>
        /// Triggers one of the haptic feedbacks available on iOS.
        /// </summary>
        public void Trigger(FeedbackType feedbackType)
        {
#if UNITY_IOS
            if (FeedbackIdSet((int)feedbackType))
                TriggerFeedbackGenerator((int)feedbackType, false);
            else
                UnityEngine.Debug.LogError("[iOSHapticFeedback] You cannot trigger a feedback generator without instantiating it first");

#elif UNITY_ANDROID
            MMVibrationManager.Haptic(HapticTypes.Selection);
#endif
        }

        private void TriggerHaptic()
        {
            //if (LocalConfig.IsHapticEnable)
            {
                Trigger(FeedbackType.SelectionChange);
            }
        }

        #region IncommingEvents


        private void GameManager_Finished()
        {
            TriggerHaptic();
        }

        private void GameManager_Completed(int obj)
        {
            TriggerHaptic();
        }

        #endregion
    }
}