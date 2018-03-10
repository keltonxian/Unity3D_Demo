using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BigTwoCommandQueue))]
[RequireComponent(typeof(BigTwoRule))]
public class BigTwoDeck : MonoBehaviour {

	private List<BigTwoPoker> _listPoker = new List<BigTwoPoker> ();
	public List<BigTwoPoker> ListPoker {
		get {
			return _listPoker;
		}
	}
	public GameObject _pokerPrefab;
	public List<BigTwoGamePlayer> _listPlayer = new List<BigTwoGamePlayer> ();
	public bool _isDoShuffleBeforeDealCard = true;
	public bool _isAutoDeal = false;

	private BigTwoCommandQueue _commandQueue;
	public BigTwoCommandQueue CommandQueue {
		get {
			return _commandQueue;
		}
	}
	private BigTwoRule _rule;
	public BigTwoRule Rule {
		get {
			return _rule;
		}
	}

	public void InitDeck (CallbackType.CallbackV callback) {
		_commandQueue = this.GetComponent<BigTwoCommandQueue> ();
		_commandQueue.Init (this);
		_rule = this.GetComponent<BigTwoRule> ();
		_rule.Init ();

		BigTwoPoker.TYPE[] typeArray = { BigTwoPoker.TYPE.SPADES, BigTwoPoker.TYPE.HEARTS, BigTwoPoker.TYPE.CLUBS, BigTwoPoker.TYPE.DIAMONDS };
		BigTwoPoker.FACE[] faceArray = { 
			BigTwoPoker.FACE.ACE, BigTwoPoker.FACE.TWO, BigTwoPoker.FACE.THREE, BigTwoPoker.FACE.FOUR, BigTwoPoker.FACE.FIVE, 
			BigTwoPoker.FACE.SIX, BigTwoPoker.FACE.SEVEN, BigTwoPoker.FACE.EIGHT, BigTwoPoker.FACE.NINE, BigTwoPoker.FACE.TEN, 
			BigTwoPoker.FACE.JACK, BigTwoPoker.FACE.QUEEN, BigTwoPoker.FACE.KING
		};
		for (int i = 0; i < typeArray.Length; i++) {
			BigTwoPoker.TYPE pokerType = typeArray [i];
			for (int j = 0; j < faceArray.Length; j++) {
				BigTwoPoker.FACE pokerFace = faceArray [j];
				BigTwoPoker poker = Instantiate (_pokerPrefab).GetComponent<BigTwoPoker> ();
				poker._pokerType = pokerType;
				poker._pokerFace = pokerFace;
				poker.transform.SetParent (transform);
				poker.Init ();
				poker.SetBackVisible (true);
				_listPoker.Add (poker);
			}
		}
		callback ();
	}

	private void ShuffleDeck () {
		List<BigTwoPoker> list = new List<BigTwoPoker> ();
		while (_listPoker.Count > 0) {
			int index = Random.Range (0, _listPoker.Count);
			list.Add (_listPoker [index]);
			_listPoker.RemoveAt (index);
		}
		for (int i = 0; i < list.Count; i++) {
			_listPoker.Add (list [i]);
		}
		list.Clear ();
	}

	public float DealCard () {
		if (_isDoShuffleBeforeDealCard) {
			ShuffleDeck ();
		}

		for (int i = 0; i < _listPlayer.Count; i++) {
			BigTwoGamePlayer player = _listPlayer [i];
			player.ClearPokerList ();
		}
		int pokerNumPerPlayer = _listPoker.Count / _listPlayer.Count;
		int startIndex = 0;
		int endIndex = pokerNumPerPlayer;
		for (int i = 0; i < _listPlayer.Count; i++) {
			BigTwoGamePlayer player = _listPlayer [i];
			for (int j = startIndex; j < endIndex; j++) {
				BigTwoPoker poker = _listPoker [j];
				poker.gameObject.SetActive (true);
				poker.Reset ();
				player.InsertPokerToHand (poker);
			}
			player.SortListPokerInHand ();
			startIndex += pokerNumPerPlayer;
			endIndex += pokerNumPerPlayer;
		}
		return 2f;
	}

	public BigTwoRule GetRule () {
		return _rule;
	}

}
