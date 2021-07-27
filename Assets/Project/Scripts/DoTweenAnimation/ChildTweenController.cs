using DG.Tweening;

namespace EmojiChat.UI
{
    public class ChildTweenController : BaseTweenController
    {
        protected override void Awake()
        {
            base.Awake();

            _animations = GetComponentsInChildren<DOTweenAnimation>();
        }
    }
}