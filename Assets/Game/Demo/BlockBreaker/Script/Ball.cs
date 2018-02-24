using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour {

	private Paddle _paddle;
	private bool _hasStarted = false;
	private Vector3 _paddleToBallVector;

	void Start () {
		_paddle = GameObject.FindObjectOfType<Paddle> ();
		_paddleToBallVector = this.transform.position - _paddle.transform.position;
	}
	
	void Update () {
		if (!_hasStarted) {
			this.transform.position = _paddle.transform.position + _paddleToBallVector;
			if (Input.GetMouseButtonDown (0)) {
				_hasStarted = true;
				this.GetComponent<Rigidbody2D> ().velocity = new Vector3 (2f, 10f);
			}
		}
	}

	void OnCollisionEnter2D (Collision2D collision) {
		Vector2 tweak = new Vector2 (Random.Range (0f, 0.2f), Random.Range (0f, 0.2f));
		if (_hasStarted) {
			GetComponent<AudioSource> ().Play ();
			GetComponent<Rigidbody2D> ().velocity += tweak;
		}
	}

}
