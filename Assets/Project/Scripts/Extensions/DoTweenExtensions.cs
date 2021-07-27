using DG.Tweening;

namespace EmojiChat
{
    public static class DoTweenExtensions
    {
        public static void Play(this DOTweenAnimation animation)
        {
            animation.tween.Rewind();
            animation.tween.Play();
        }
    }
}