using UnityEngine;

namespace EmojiChat
{
	public abstract class GameObjectSingleton<T> : MonoBehaviour where T : Component
	{
		#region Fields

		private static T _instance = null;

		#endregion

		#region Properties

		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
#if UNITY_EDITOR
					Debug.LogException(new System.Exception($"{nameof(GameObjectSingleton<T>)} with type {nameof(T)} is null: go to startup manager and setup"));
#endif
				}

				return _instance;
			}
		}

		#endregion

		protected virtual void Awake()
		{
			if (_instance == null)
			{
				_instance = this as T;

				DontDestroyOnLoad(gameObject);
				Init();
			}
			else
			{
				Destroy(gameObject);
			}
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

		protected virtual void Init()
		{

		}

		protected virtual void DeInit()
		{

		}

		public static void GetInstance()
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<T>();
				if (_instance == null)
				{
					var obj = Resources.Load<T>($"Singletons/{typeof(T).Name}");
					Instantiate(obj);
				}
			}
			else
			{
#if UNITY_EDITOR
				Debug.LogException(new System.Exception($"Trying to instance {nameof(GameObjectSingleton<T>)} duplicate with type {nameof(T)}"));
#endif
			}
		}

		private void OnDestroy()
		{
			DeInit();
		}
	}
}