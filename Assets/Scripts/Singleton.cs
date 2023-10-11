using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
	public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
	{
		private static T instance;

		public static T Instance
		{
			get
			{
				if(instance == null)
				{
					instance = FindObjectOfType<T>();
				}
				return instance;
			}
		}

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}