using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public class BigTwoPokerHandInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	private enum MouseMoveOrientation {
		NONE,
		LEFTWARD,
		RIGHTWARD,
	}
	private MouseMoveOrientation _mouseMoveOrientation = MouseMoveOrientation.NONE;
	private Vector3 _lastMousePos;
	private bool _isTouching = false;
	private bool _isTouchUp = false;

	[Serializable]
	public class OnSelectPokerEvent : UnityEvent <BigTwoPoker> {}
	[SerializeField]
	protected OnSelectPokerEvent _onSelectPoker = new OnSelectPokerEvent();
	public OnSelectPokerEvent OnSelectPoker {
		get {
			return _onSelectPoker;
		}
	}
	public BigTwoGamePlayer _player;
	public bool _isTouchEnabled = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// if (_isTouching) {

		// 	MouseMoveOrientation mouseMoveOrientation = _mouseMoveOrientation;
		// 	if (Input.mousePosition.x - _lastMousePos.x < 0f) {
		// 		mouseMoveOrientation = MouseMoveOrientation.LEFTWARD;
		// 	} else if (Input.mousePosition.x - _lastMousePos.x > 0f) {
		// 		mouseMoveOrientation = MouseMoveOrientation.RIGHTWARD;
		// 	}
		// 	if (_mouseMoveOrientation == MouseMoveOrientation.NONE) {
		// 		_mouseMoveOrientation = mouseMoveOrientation;
		// 	}
		// 	_lastMousePos = Input.mousePosition;
		// 	if (mouseMoveOrientation != _mouseMoveOrientation) {
		// 		_mouseMoveOrientation = mouseMoveOrientation;
		// 		ResetPokerTouchState (true);
		// 	}
		// 	BigTwoPoker selectedCollider = null;
		// 	float minZ = 1000f;
		// 	Collider2D[] colliders = Physics2D.OverlapPointAll (Camera.main.ScreenToWorldPoint (Input.mousePosition));

		// 	if (colliders.Length > 0) {
		// 		foreach (Collider2D collider in colliders) {
		// 			if (collider.GetComponent<BigTwoPoker> ()) {
		// 				if (collider.transform.localPosition.z < minZ) {
		// 					minZ = collider.transform.localPosition.z;
		// 					selectedCollider = collider.GetComponent<BigTwoPoker> ();
		// 				}
		// 			}
		// 		}
		// 	}
		// 	if (selectedCollider) {
		// 		BigTwoPoker poker = selectedCollider.GetComponent<BigTwoPoker> ();
		// 		if (poker.IsTouchEnabled) {
		// 			bool lastState = poker.IsSelected;
		// 			poker.IsSelected = !poker.IsSelected;
		// 			if (lastState != poker.IsSelected) {
		// 				poker.IsTouchEnabled = false;
		// 				OnSelectPoker.Invoke (poker);
		// 			}
		// 		}
		// 	}
		// 	if (_isTouchUp) {
		// 		ResetPokerTouchState (false);
		// 		_isTouching = false;
		// 	}
		// }
	}

	public virtual void OnPointerDown (PointerEventData eventData) {
		if (false == _isTouchEnabled) {
			return;
		}
		if (transform.childCount == 0) {
			return;
		}
		ResetPokerTouchState (true);
		_mouseMoveOrientation = MouseMoveOrientation.NONE;
		_lastMousePos = Input.mousePosition;
		_isTouching = true;
		_isTouchUp = false;
	}

	public virtual void OnPointerUp (PointerEventData eventData) {
		if (false == _isTouchEnabled) {
			return;
		}
		_isTouchUp = true;
	}

	private void ResetPokerTouchState (bool isTouchEnabled) {
		List<BigTwoPoker> listPoker = _player.ListPokerInHand;
		for (int i = 0; i < listPoker.Count; i++) {
			BigTwoPoker poker = listPoker [i];
			poker.IsTouchEnabled = isTouchEnabled;
		}
	}

//	public void SetSelected (bool isSelected) {
//		if (!IsTouchEnabled) {
//		}
//		if (_isAnimating) {
//			return;
//		}
//		if (isSelected == _isSelected) {
//			return;
//		}
//		_isAnimating = true;
//		float toY = _faceDefaultPos.y;
//		if (!_isSelected) {
//			toY += 0.2f;
//		}
//		_face.DOLocalMoveY (toY, 0.15f).SetEase (Ease.Linear).OnComplete (() => {
//			_isAnimating = false;
//		});
//		_isSelected = !_isSelected;
//	}

}
