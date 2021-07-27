using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace EmojiChat.UI
{
	public class UIJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
	{
		public static Action DragBeginned = delegate { };
		public static Action<Vector3> Dragged = delegate { };
		public static Action<Vector3> Clicked = delegate { };
		public static Action DragEnded = delegate { };

		public static UIJoystick Instance = null;

		private Vector2 _swipeDelta = Vector2.zero;

		public bool IsDragging
		{
			get;
			private set;
		}

		private void Awake()
		{
			Instance = this;
		}

		public void OnBeginDrag(PointerEventData data)
		{
			DragBeginned();
		}

		public void OnDrag(PointerEventData data)
		{
			_swipeDelta += data.delta;
			
			Dragged(data.position);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			IsDragging = true;

			Clicked(eventData.position);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			IsDragging = false;

			_swipeDelta = Vector2.zero;
			
			DragEnded();
		}
	}
}