using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	}

	public void SetBackVisible (bool isVisible) {
		_backSR.gameObject.SetActive (isVisible);
	}

	public void Reset () {
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

}
