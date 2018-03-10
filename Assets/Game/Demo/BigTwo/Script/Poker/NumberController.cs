using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberController : MonoBehaviour {

	public Text _numberText = null;
	public int _currentNumber = 0;
	private int _toNumber = 0;
	public float _updateInterval = 0.12f;

	// Use this for initialization
	void Start () {
		UpdateNumber (_currentNumber, true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateNumber (int nextNumber, bool isNoAnim = false) {
		CancelInvoke ();
		if (true == isNoAnim) {
			_currentNumber = nextNumber;
			_numberText.text = string.Format ("{0}", _currentNumber);
			return;
		}
		_toNumber = nextNumber;
		InvokeRepeating ("UpdateAnim", 0f, _updateInterval);
	}

	private void UpdateAnim () {
		if (_toNumber == _currentNumber) {
			CancelInvoke ();
			return;
		}
		_currentNumber += _toNumber > _currentNumber ? 1 : -1;
		_numberText.text = string.Format ("{0}", _currentNumber);
	}
}
