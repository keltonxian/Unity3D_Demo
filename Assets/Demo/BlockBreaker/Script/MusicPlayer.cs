using System.Collections;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

	private static MusicPlayer _instance = null;

	// Use this for initialization
	void Start () {
		if (null != _instance) {
			Destroy (gameObject);
			return;
		}
		_instance = this;
		GameObject.DontDestroyOnLoad (gameObject);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
