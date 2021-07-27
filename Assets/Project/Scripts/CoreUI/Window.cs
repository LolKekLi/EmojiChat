using System;
using UnityEngine;
using DG.Tweening;

namespace EmojiChat.UI
{
    public abstract class Window : MonoBehaviour
    {
        [SerializeField]
        private DOTweenAnimation _appearAnimation = null;

        [SerializeField]
        private DOTweenAnimation _disappearAnimation = null;

        public abstract bool IsPopup
        {
            get;
        }

        protected virtual void Start()
        {

        }

        protected virtual void OnEnable()
        {
            
        }

        protected virtual void OnDisable()
        {
            
        }

        public virtual void Preload()
        {
            gameObject.SetActive(false);
        }

        public virtual void BeforeShow(Action action = null)
        {
            if (_appearAnimation != null)
            {
                _appearAnimation.tween.OnComplete(() =>
                {
                    action?.Invoke();
                });

                _appearAnimation.Play();
            }
            else
            {
                action?.Invoke();
            }
        }

        public virtual void OnShow()
        {
            gameObject.SetActive(true);
        }

        public virtual void OnHide()
        {
            Action action = () =>
            {
                gameObject.SetActive(false);
            };

            if (_disappearAnimation != null)
            {
                _disappearAnimation.tween.OnComplete(() =>
                {
                    action?.Invoke();
                });

                _disappearAnimation.Play();
            }
            else
            {
                action?.Invoke();
            }
        }

        public virtual void Refresh()
        {

        }
    }
}