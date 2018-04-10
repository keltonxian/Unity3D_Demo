using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BigTwoPoker : MonoBehaviour {

	public enum TYPE {
		NONE, SPADES, HEARTS, CLUBS, DIAMONDS
	}
	public enum FACE {
		NONE = 0, ACE = 1, TWO = 2, THREE = 3, FOUR = 4, FIVE = 5, SIX = 6, SEVEN = 7, EIGHT = 8, NINE = 9, TEN = 10, JACK = 11, QUEEN = 12, KING = 13
	}

	public TYPE _pokerType = TYPE.NONE;
	public FACE _pokerFace = FACE.NONE;

	public SpriteRenderer _faceSR = null;
	public SpriteRenderer _backSR = null;

	private bool _isSelected = false;
	public bool IsSelected {
		get {
			return _isSelected;
		}
		set {
			SetSelected (value);
		}
	}
	private bool _isAnimating = false;
	private Vector3 _faceDefaultPos;
	private bool _isTouchEnabled = true;
	public bool IsTouchEnabled {
		get {
			return _isTouchEnabled;
		}
		set {
			_isTouchEnabled = value;
		}
	}

	public void Init () {
		SetFace ();
		_faceDefaultPos = _faceSR.transform.localPosition;
	}

	public void SetBackVisible (bool isVisible) {
		_backSR.gameObject.SetActive (isVisible);
	}

	public void Reset () {
		IsSelected = false;
		IsTouchEnabled = false;
	}

	private void SetFace () {
		string path = "";
		if (_pokerType == TYPE.SPADES) {
			path += "Spades/";
		} else if (_pokerType == TYPE.HEARTS) {
			path += "Hearts/";
		} else if (_pokerType == TYPE.CLUBS) {
			path += "Clubs/";
		} else if (_pokerType == TYPE.DIAMONDS) {
			path += "Diamonds/";
		} else {
			return;
		}
		path += string.Format ("{0}", (int)_pokerFace);
		ResManager.Instance.LoadSpriteFromResourceAsync (path, (Sprite sprite) => {
			_faceSR.GetComponent<SpriteRenderer> ().sprite = sprite; 
		}, () => {
		});
	}

	public void SetSelected (bool isSelected) {
		if (!IsTouchEnabled) {
		}
		if (_isAnimating) {
			return;
		}
		if (isSelected == _isSelected) {
			return;
		}
		_isAnimating = true;
		float toY = _faceDefaultPos.y;
		if (!_isSelected) {
			toY += 0.2f;
		}
		_faceSR.transform.DOKill ();
		_faceSR.transform.DOLocalMoveY (toY, 0.15f).SetEase (Ease.Linear).OnComplete (() => {
			_isAnimating = false;
		});
		_isSelected = !_isSelected;
	}

}
