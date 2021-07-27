using DG.Tweening;

namespace EmojiChat.UI
{
    public class SelfTweenController : BaseTweenController
    {
        protected override void Awake()
        {
            base.Awake();

            _animations = GetComponents<DOTweenAnimation>();
        }
    }
}