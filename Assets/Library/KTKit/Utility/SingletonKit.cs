using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonKit<T> : MonoBehaviour where T : Component {

	private static T _instance;
	public static T Instance {
		get {
			if (null == _instance) {
				_instance = FindObjectOfType (typeof(T)) as T;
				if (null == _instance) {
					GameObject obj = new GameObject ();
//					obj.hideFlags = HideFlags.HideAndDontSave;
					_instance = obj.AddComponent (typeof(T)) as T;
				}
			}
			return _instance;
		}
	}

	protected virtual void Awake () {
		DontDestroyOnLoad (this.gameObject);
		if (null == _instance) {
			_instance = this as T;
		} else {
			Destroy (this.gameObject);
		}
	}

}
