using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Native2DLayerScaler : MonoBehaviour {

	public CanvasScaler _referenceCanvas;

	void Start () {
		if (_referenceCanvas) {
			transform.localScale = _referenceCanvas.transform.localScale * 100f;
		}
	}

	#if UNITY_EDITOR
	void Update () {
		if (_referenceCanvas) {
			transform.localScale = _referenceCanvas.transform.localScale * 100f;
		}
	}
	#endif
}
