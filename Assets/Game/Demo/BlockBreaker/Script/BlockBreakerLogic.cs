using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlockBreakerLogic : MonoBehaviour {

	public CanvasScaler _canvasScaler;
	public Transform _leftBound;
	public Transform _rightBound;

	public void Init () {
		UpdateBounds ();
	}

	private void UpdateBounds () {
		Vector2 refSize = _canvasScaler.referenceResolution;
		Vector2 screenSize = new Vector2 (Screen.width, Screen.height);
		float halfWidth = screenSize.x / screenSize.y * refSize.y / 100f / 2f;
		Vector3 leftBoundPos = _leftBound.transform.localPosition;
		Vector3 rightBoundPos = _rightBound.transform.localPosition;
		_leftBound.transform.localPosition = new Vector3 (-halfWidth, leftBoundPos.y, leftBoundPos.z);
		_rightBound.transform.localPosition = new Vector3 (halfWidth, rightBoundPos.y, rightBoundPos.z);
	}

}
