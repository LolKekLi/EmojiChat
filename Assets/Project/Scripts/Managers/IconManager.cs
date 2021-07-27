using System;
using System.Linq;
using UnityEngine;

namespace EmojiChat
{
    public class IconManager : GameObjectSingleton<IconManager>
    {
        [Serializable]
        public class EmojiSetup
        {
            [SerializeField]
            private EmojiType _type;
            [SerializeField]
            private Sprite _sprite = null;

            public EmojiType Type => _type;
            public Sprite Sprite => _sprite;
        }

        [SerializeField]
        private EmojiSetup[] _setups = null;

        public Sprite GetSprite(EmojiType type)
        {
            EmojiSetup setup = _setups.FirstOrDefault(stp => stp.Type == type);

            if (setup == null)
            {
                Debug.LogException(new Exception($"Not found {nameof(EmojiType)} for type: {type}"));
            }

            return setup.Sprite;
        }
    }
}