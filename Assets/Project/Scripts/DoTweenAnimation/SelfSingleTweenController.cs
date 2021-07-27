namespace EmojiChat.UI
{
    public class SelfSingleTweenController : SelfTweenController
    {
        protected override void OnEnable()
        {
            base.OnEnable();

            Play();
        }
    }
}