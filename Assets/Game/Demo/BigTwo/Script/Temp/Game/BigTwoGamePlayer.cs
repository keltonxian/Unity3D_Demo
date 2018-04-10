using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public class BigTwoGamePlayer : BigTwoPlayer {

	public enum PLAYER_SEAT {
		DOWN, LEFT, UP, RIGHT,
	}
	private PLAYER_SEAT _seat;
	public BigTwoDeck _deck;
	public BigTwoAI _gameAI;
	public bool _isHandInput = false;
	public BigTwoPokerHandInput _handInput;
	public GameObject _roundBtns;
	public RectTransform _roundTipNotValid;
	public Transform _pokerHandParent;
	private List<BigTwoPoker> _listPokerInHand = new List<BigTwoPoker> ();
	public List<BigTwoPoker> ListPokerInHand {
		get {
			return _listPokerInHand;
		}
	}
	public NumberController _listCountText = null;
	public bool _isShowFace = false;
	public bool _isOverlap = false;
	public Transform _pokerSetParent;
	private List<BigTwoPoker> _listPokerInSet = new List<BigTwoPoker> ();
	public List<BigTwoPoker> ListPokerInSet {
		get {
			return _listPokerInSet;
		}
	}
	private CallbackType.CallbackS _nextPlayCMDCallback;

	public void Init (PLAYER_SEAT seat) {
		_seat = seat;
		if (_isHandInput) {
			_handInput.Init ();
		}
	}

	public void OnClickPlayHand () {
		List<BigTwoPoker> listPoker = new List<BigTwoPoker> ();
		for (int i = 0; i < _listPokerInHand.Count; i++) {
			BigTwoPoker poker = _listPokerInHand [i];
			if (poker.IsSelected) {
				listPoker.Add (poker);
			}
		}
		if (0 == listPoker.Count && (_deck.IsFirstPlay () || _deck.IsRoundPassed ())) {
			TipNotValid ();
			return;
		}
		if (!_deck.IsFirstPlay () && !_deck.IsRoundPassed ()) {
			List<BigTwoPoker> lastSet = _deck.GetLastValidPlayPokerList ();
			if (lastSet.Count > 0) {
				BigTwoRule.CompareResult result = _deck.Rule.IsPokerListBigger (listPoker, lastSet);
				if (result != BigTwoRule.CompareResult.BIGGER) {
					TipNotValid ();
					return;
				}
			}
		}
		string cmd = _deck.CombinePokerListToCMD (listPoker);
		_roundBtns.SetActive (false);
		SendPlayHandCMD (cmd);
	}

	private void TipNotValid () {
		_roundTipNotValid.gameObject.SetActive (true);
		_roundTipNotValid.localScale = Vector3.zero;
		Sequence seq = DOTween.Sequence ();
		seq.Append (_roundTipNotValid.DOScale (1f, 0.5f).SetEase (Ease.OutBack));
		seq.AppendCallback (() => {
			_roundTipNotValid.gameObject.SetActive (false);
		});
	}

	public void OnClickPass () {
		if (_deck.IsFirstPlay () || _deck.IsRoundPassed ()) {
			TipNotValid ();
			return;
		}
		_roundBtns.SetActive (false);
		SendPlayHandCMD ("");
	}

	public void OnClickTipHand () {
		string cmd = _gameAI.GetPlayCommand (ListPokerInHand, _deck);
		List<string> listStr = BigTwoCommandQueue.Instance.SplitCMDToList (cmd);
		List<BigTwoPoker> listPoker = _deck.GetPokerListFromPokerCMDListStr (listStr);
		for (int i = 0; i < _listPokerInHand.Count; i++) {
			BigTwoPoker pokerInHand = _listPokerInHand [i];
			for (int j = 0; j < listPoker.Count; j++) {
				BigTwoPoker pokerInTip = listPoker [j];
				if (pokerInHand == pokerInTip) {
					pokerInHand.IsSelected = true;
					break;
				}
			}
		}
	}

	private void SendPlayHandCMD (string handCMD) {
		string cmd = string.Format ("{0} {1} {2}", BigTwoCommandQueue.CMD_TYPE.PLAY_HAND.ToString (), _seat.ToString (), handCMD);
		_nextPlayCMDCallback (cmd);
	}

	public void NextPlayCMD (CallbackType.CallbackS callback) {
		ClearListPokerInSet ();
		_nextPlayCMDCallback = callback;
		if (_isHandInput) {
			_roundBtns.SetActive (true);
			return;
		}
		string cmd = _gameAI.GetPlayCommand (ListPokerInHand, _deck);
		SendPlayHandCMD (cmd);
		// List<Poker> hand = new List<Poker> ();
		// List<Poker> retPoker = prevPlayerHand;
		// bool isPass = true;
		// _pokerAi.GetHand (prevPlayerHand, _pokerList.GetPokerList, ref hand);
		// if (hand.Count > 0) {
		// 	for (int i = 0; i < hand.Count; i++) {
		// 		Poker poker = hand [i];
		// 		poker.PrintInfo ();
		// 	}
		// 	retPoker = _pokerList.SendToTable (ref hand).PokerList ();
		// 	isPass = false;
		// } else {
		// 	_pokerList.ClearTable ();
		// }
		// if (null != callback) {
		// 	callback (retPoker, isPass);
		// }
	}

	public bool HasPlayAll () {
		Debug.Log (string.Format ("Name[{0}] Hand[{1}]", gameObject.name, ListPokerInHand.Count));
		return (0 == ListPokerInHand.Count);
	}

	public void PlayHandToSet (List<string> listStr, CallbackType.CallbackV doneCallback) {
		List<BigTwoPoker> listPoker = _deck.GetPokerListFromPlayCMDListStr (listStr);
		float timeWait = 0.2f;
		if (listPoker.Count > 0) {
			timeWait = 1.2f;
		}
		Sequence seq = DOTween.Sequence ();
		seq.AppendCallback (() => {
			ClearListPokerInSet ();
			for (int i = 0; i < listPoker.Count; i++) {
				BigTwoPoker poker = listPoker [i];
				InsertPokerToSet (poker);
			}
			SortListPokerInHand (true);
			ShowListPokerInSet ();
		});
		seq.AppendInterval (timeWait);
		seq.AppendCallback (() => {
			doneCallback ();
		});
	}

	private void ClearListPokerInHand () {
		for (int i = 0; i < _listPokerInHand.Count; i++) {
			BigTwoPoker poker = _listPokerInHand [i];
			poker.transform.SetParent (_deck.transform);
			poker.gameObject.SetActive (false);
		}
		_listPokerInHand.Clear ();
		_listCountText.UpdateNumber (_listPokerInHand.Count, true);
	}

	public void InsertPokerToHand (BigTwoPoker poker) {
		poker.transform.SetParent (_pokerHandParent);
		poker.transform.localPosition = new Vector3 (0f, 0f, 0);
		_listPokerInHand.Add (poker);
	}

	public void SortListPokerInHand (bool isSortFromCurrentPos = false) {
		int pokerCount = _listPokerInHand.Count;
		Vector2 size = _pokerHandParent.GetComponent<BoxCollider2D> ().size;
		float startX = -size.x / 2f;
		float startY = 0f;
		float gapx = 0f;
		_deck.GetRule ().SortPokerList (_listPokerInHand);
		for (int i = 0; i < pokerCount; i++) {
			BigTwoPoker poker = _listPokerInHand [i];
			poker.transform.localScale = Vector3.one * size.y / poker.transform.GetComponent<BoxCollider2D> ().size.y;
			if (i == 0) {
				float width = poker.transform.GetComponent<BoxCollider2D> ().size.x * poker.transform.localScale.x;
				if (_isOverlap) {
					startX = 0;
					gapx = 0f;
				} else {
					startX += width / 2;
					gapx = (size.x - width) / (pokerCount - 1);
					if (gapx > width) {
						gapx = width;
						startX = startX + size.x / 2 - gapx * (pokerCount / 2 + 0.5f);
					}
				}
			}
			float toX = startX + gapx * i;
			if (false == isSortFromCurrentPos) {
				poker.transform.localPosition = new Vector3 (toX, startY, pokerCount - i);
				if (_isShowFace && !_isOverlap) {
					poker.transform.DOLocalMoveX (0f, Math.Abs(toX/1.3f)).From ().SetEase (Ease.Linear);
				}
			} else {
				if (_isShowFace && !_isOverlap) {
					poker.transform.DOLocalMoveX (toX, 0.5f).SetEase (Ease.Linear);
				}
			}
			poker.SetBackVisible (!_isShowFace);
		}	
		_listCountText.UpdateNumber (_listPokerInHand.Count);
	}

	public void ClearPokerList () {
		ClearListPokerInHand ();
		ClearListPokerInSet ();
	}

	private void ClearListPokerInSet () {
		for (int i = 0; i < _listPokerInSet.Count; i++) {
			BigTwoPoker poker = _listPokerInSet [i];
			poker.transform.SetParent (_deck.transform);
			poker.gameObject.SetActive (false);
		}
		_listPokerInSet.Clear ();
	}

	public void InsertPokerToSet (BigTwoPoker poker) {
		poker.transform.SetParent (_pokerSetParent);
		poker.transform.localPosition = Vector3.zero;
		poker.transform.localScale = Vector3.one;
		_listPokerInSet.Add (poker);
		_listPokerInHand.Remove (poker);

		_deck.GetRule ().SortPokerList (_listPokerInSet);
	}

	public void ShowListPokerInSet (bool noAnim = false) {
		int pokerCount = _listPokerInSet.Count;
		Vector2 size = _pokerSetParent.GetComponent<BoxCollider2D> ().size;
		float startX = -size.x / 2f;
		float startY = 0f;
		float gapx = 0f;
		for (int i = 0; i < pokerCount; i++) {
			BigTwoPoker poker = _listPokerInSet [i];
			poker.transform.localScale = Vector3.one * size.y / poker.transform.GetComponent<BoxCollider2D> ().size.y;
			if (i == 0) {
				float width = poker.transform.GetComponent<BoxCollider2D> ().size.x * poker.transform.localScale.x;
				startX += width / 2;
				gapx = (size.x - width) / (pokerCount - 1);
				if (gapx > width) {
					gapx = width;
					startX = startX + size.x / 2 - gapx * (pokerCount / 2 + 0.5f);
				}
			}
			float toX = startX + gapx * i;
			poker.transform.localPosition = new Vector3 (toX, startY, pokerCount - i);
			if (false == noAnim) {
				poker.transform.DOLocalMoveX (0f, Math.Abs (toX / 1.3f)).From ().SetEase (Ease.Linear);
			}
			poker.SetBackVisible (false);
		}	
	}

	public bool RemovePokerInListPokerInSet (BigTwoPoker targetPoker) {
		for (int i = 0; i < _listPokerInSet.Count; i++) {
			BigTwoPoker poker = _listPokerInSet [i];
			if (poker._pokerType == targetPoker._pokerType && poker._pokerFace == targetPoker._pokerFace) {
				_listPokerInSet.Remove (poker);
				Destroy (poker.gameObject);
				return true;
			}
		}
		return false;
	}

	#if UNITY_EDITOR
	void OnDrawGizmos () {
		if (null != _pokerHandParent) {
			Vector2 size = _pokerHandParent.GetComponent<BoxCollider2D> ().size;
			Gizmos.color = new Color (0.3f, 1f, 0.3f, 0.5f);
			Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
			Gizmos.matrix = _pokerHandParent.localToWorldMatrix;
			Gizmos.DrawWireCube (Vector3.zero, new Vector3 (size.x, size.y, 1f));
			Gizmos.matrix = oldGizmosMatrix;
		}
		if (null != _pokerSetParent) {
			Vector2 size = _pokerSetParent.GetComponent<BoxCollider2D> ().size;
			Gizmos.color = new Color (1f, 0.3f, 0.3f, 0.5f);
			Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
			Gizmos.matrix = _pokerSetParent.localToWorldMatrix;
			Gizmos.DrawWireCube (Vector3.zero, new Vector3 (size.x, size.y, 1f));
			Gizmos.matrix = oldGizmosMatrix;
		}
	}
	#endif

}
