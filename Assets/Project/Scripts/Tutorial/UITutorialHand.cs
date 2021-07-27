using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace EmojiChat
{
    public class UITutorialHand : MonoBehaviour
    {
        private DOTweenAnimation[] _animations = null;

        private float _delay = 1.4f;

        private Vector2 _targetPos;
        private Vector2 _endPos;
        private Vector2 _offset;

        private RectTransform _transform = null;

        private void Awake()
        {
            _animations = GetComponentsInChildren<DOTweenAnimation>();
        }

        public void SetupAchorePos(Vector2 pos, Vector2 offset, float rotate)
        {
            _targetPos = pos;
            _offset = offset;

            _transform = (RectTransform)transform;

            _transform.SetViewportPosition(_targetPos);
            _transform.anchoredPosition = _transform.anchoredPosition + _offset;
            _transform.SetAsLastSibling();
        }

        public void SetupAnimatedHand(Vector2 endPos)
        {
            _endPos = endPos;

            _transform.DOKill();

            _transform.DOAnchorMax(_endPos, _delay).SetUpdate(true).SetLoops(-1);
            _transform.DOAnchorMin(_endPos, _delay).SetUpdate(true).SetLoops(-1);
            _transform.DOAnchorPos(_endPos, _delay).SetUpdate(true).SetLoops(-1);

            StartCoroutine(HandCor());
        }

        public void Free()
        {
            _transform.DOKill();

            Destroy(gameObject);
        }

        private IEnumerator HandCor()
        {
            var waiter = new WaitForSeconds(_delay);

            while (true)
            {
                for (int i = 0; i < _animations.Length; i++)
                {
                    _animations[i].Play();
                }

                yield return waiter;
            }
        }
    }
}