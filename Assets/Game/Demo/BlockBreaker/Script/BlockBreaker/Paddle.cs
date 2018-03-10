using System.Collections;
using UnityEngine;

public class Paddle : MonoBehaviour {

	public bool _isAutoPlay = false;
	public float _minX, _maxX;

	private Ball ball;

	void Start () {
		ball = GameObject.FindObjectOfType<Ball> ();
	}
	
	void Update () {
		if (!_isAutoPlay) {
			MoveWithMouse ();
		} else {
			AutoPlay ();
		}
	}

	void AutoPlay () {
		Vector3 paddlePos = new Vector3 (0.5f, this.transform.position.y, 0f);
		Vector3 ballPos = ball.transform.position;
		paddlePos.x = Mathf.Clamp (ballPos.x, _minX, _maxX);
		this.transform.position = paddlePos;
	}

	void MoveWithMouse () {
		Vector3 paddlePos = new Vector3 (0.5f, this.transform.position.y, 0f);
		float mousePosInBlocks = Input.mousePosition.x / Screen.width * 16;
		paddlePos.x = Mathf.Clamp (mousePosInBlocks, _minX, _maxX);
		this.transform.position = paddlePos;
	}

}
