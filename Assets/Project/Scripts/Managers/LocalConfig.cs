using UnityEngine;

namespace EmojiChat
{
    public static class LocalConfig
    {
        private class Keys
        {
            public const string LevelIndex = "LevelIndex";
            public const string BasicTutorialNeeded = "BasicTutorialNeeded";
        }

        public static int LevelIndex
        {
            get { return PlayerPrefs.GetInt(Keys.LevelIndex, 0); }
            set { PlayerPrefs.SetInt(Keys.LevelIndex, value); }
        }

        public static bool BasicTutorialNeeded
        {
            get { return GetBoolValue(Keys.BasicTutorialNeeded, true); }
            set { SetBoolValue(Keys.BasicTutorialNeeded, value); }
        }

        private static void SetBoolValue(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        private static bool GetBoolValue(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1 ? true : false;
        }
    }
}