using System.Collections;
using UnityEngine;

public class LoseCollider : MonoBehaviour {

	private LevelManager _levelManager;

	void OnTriggerEnter2D (Collider2D trigger) {
		_levelManager = GameObject.FindObjectOfType<LevelManager> ();
		_levelManager.LoadLevel ("BlockBreakerLose");
	}

	void OnCollisionEnter2D (Collision2D collision) {
	}

}
