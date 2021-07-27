using UnityEngine;
using EmojiChat.UI;

namespace EmojiChat
{
    public static class RectTransformExtensions
    {
        public static void SetViewportPosition(this RectTransform rect, Vector2 viewportPosition)
        {
            rect.anchorMax = viewportPosition;
            rect.anchorMin = viewportPosition;

            rect.anchoredPosition = viewportPosition;
        }

        public static bool IsOverlapsOne(this RectTransform rect, RectTransform other, float delta)
        {

            return rect.rect.height <
                Mathf.Abs(other.anchoredPosition.y - delta);
        }

        public static bool IsRectOverlaps(this RectTransform rectTrans1, RectTransform rectTrans2)
        {
            Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
            Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

            return rect1.Overlaps(rect2);
        }

        public static bool IsOverlaps(this RectTransform a, RectTransform b)
        {
            //Debug.Log(a.GetWorldRect().yMax + " : " + b.GetWorldRect().yMin);
            //Vector3[] v = new Vector3[4];
            //a.GetWorldCorners(v);

            //Debug.Log("World Corners");
            //for (var i = 0; i < 4; i++)
            //{
            //    Debug.Log("World Corner " + i + " : " + v[i]);
            //}

            //b.GetWorldCorners(v);

            //Debug.Log("World Corners");
            //for (var i = 0; i < 4; i++)
            //{
            //    Debug.Log("World Corner " + i + " : " + v[i]);
            //}

            return a.WorldRect().Overlaps(b.WorldRect());
        }

        public static Rect GetWorldRect(this RectTransform rt, Vector2 scale)
        {
            // Convert the rectangle to world corners and grab the top left
            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);
            Vector3 topLeft = corners[0];

            // Rescale the size appropriately based on the current Canvas scale
            Vector2 scaledSize = new Vector2(scale.x * rt.rect.size.x, scale.y * rt.rect.size.y);

            return new Rect(topLeft, scaledSize);
        }

        public static Rect WorldRect(this RectTransform rectTransform)
        {
            Vector2 sizeDelta = rectTransform.sizeDelta;
            float rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
            float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

            Vector3 position = rectTransform.position;
            return new Rect(position.x - rectTransformWidth / 2f, position.y - rectTransformHeight / 2f, rectTransformWidth, rectTransformHeight);
        }
    }
}