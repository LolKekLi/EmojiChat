using UnityEngine;
using System.Linq;
using DG.Tweening;
using System;

namespace EmojiChat
{
    public abstract class BaseTweenController : MonoBehaviour
    {
        [SerializeField]
        protected float _delayBeforePlaying = 0f;

        protected DOTweenAnimation[] _animations = null;

        protected virtual void Awake()
        {
            
        }

        protected virtual void OnEnable()
        {

        }

        public virtual void Play()
        {
            Action action = () =>
            {
                _animations.Do(anim =>
                {
                    anim.tween.Rewind();
                    anim.tween.Play();
                });
            };

            if (_delayBeforePlaying.AlmostEquals(0))
            {
                action.Invoke();
            }
            else
            {
                this.InvokeWithDelay(_delayBeforePlaying, () =>
                {
                    action?.Invoke();
                });
            }
        }

        public virtual void Reset()
        {
            _animations.Do(anim =>
            {
                anim.tween.Rewind();
            });
        }

        public float GetMaxAnimationTime()
        {
            return _animations.Max(anim => anim.duration);
        }
    }
}