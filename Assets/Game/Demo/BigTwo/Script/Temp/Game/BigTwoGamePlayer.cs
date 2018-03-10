using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public class BigTwoGamePlayer : BigTwoPlayer {

	public BigTwoDeck _deck;
	public BigTwoAI _gameAI;
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

	public string NextPlayCMD () {
		string cmd = _gameAI.GetPlayCommand (ListPokerInHand, _deck);
		return cmd;
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
		return (0 == ListPokerInHand.Count);
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
			if (i == 0) {
				float width = poker.transform.GetComponent<BoxCollider2D> ().size.x;
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

	public void ClearTable () {
//		_set.ClearPokerList ();
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
			if (i == 0) {
				float width = poker.transform.GetComponent<BoxCollider2D> ().size.x;
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
