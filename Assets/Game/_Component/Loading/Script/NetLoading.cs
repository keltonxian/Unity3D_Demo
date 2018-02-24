using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetLoading : MonoBehaviour {

	private static NetLoading _instance = null;

	void Awake () {
		gameObject.GetComponent<Canvas> ().worldCamera = Camera.main;
	}

	public static void Show () {
		if (null != _instance) {
			return;
		}
		_instance = Instantiate(Resources.Load<NetLoading>("NetLoading"));
	}

	public static void Remove (float delay = 0f) {
		if (null == _instance) {
			return;
		}
		if (delay > 0) {
			_instance.Invoke ("Dispose", delay);
		} else {
			_instance.Dispose ();
		}
	}

	public void Dispose(){
		Destroy(gameObject);
	}

	void OnDestroy () {
		_instance = null;
	}	

}
