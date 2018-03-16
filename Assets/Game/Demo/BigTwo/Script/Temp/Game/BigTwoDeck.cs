using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BigTwoRule))]
public class BigTwoDeck : MonoBehaviour {

	private List<BigTwoPoker> _listPoker = new List<BigTwoPoker> ();
	public List<BigTwoPoker> ListPoker {
		get {
			return _listPoker;
		}
	}
	public GameObject _pokerPrefab;
	public bool _isDoShuffleBeforeDealCard = true;
	public bool _isAutoDeal = false;
	private BigTwoRule _rule;
	public BigTwoRule Rule {
		get {
			return _rule;
		}
	}

	public void Init () {
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

	public float DealCard (List<BigTwoGamePlayer> listPlayer) {
		if (_isDoShuffleBeforeDealCard) {
			ShuffleDeck ();
		}

		for (int i = 0; i < listPlayer.Count; i++) {
			BigTwoGamePlayer player = listPlayer [i];
			player.ClearPokerList ();
		}
		int pokerNumPerPlayer = _listPoker.Count / listPlayer.Count;
		int startIndex = 0;
		int endIndex = pokerNumPerPlayer;
		for (int i = 0; i < listPlayer.Count; i++) {
			BigTwoGamePlayer player = listPlayer [i];
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

	public List<BigTwoPoker> GetLastPokerList () {
		string cmd = BigTwoCommandQueue.Instance.GetLastCMD ();	
		List<string> listStr = BigTwoCommandQueue.Instance.SplitCMDToList (cmd);
		return GetPokerListFromListStr (listStr);
	}

	private List<BigTwoPoker> GetPokerListFromListStr (List<string> listStr) {
		List<BigTwoPoker> listPoker = new List<BigTwoPoker> ();
		if (listStr.Count < 3) {
			return listPoker;
		}
		for (int i = 0; i < listStr.Count; i++) {
			string pokerStr = listStr [i];
			string[] pokerData = pokerStr.Split (BigTwoCommandQueue.SPACE_CHAR_POKER);
			if (pokerData.Length < 2) {
				continue;
			}
			string pokerType = pokerData [0];
			string pokerFace = pokerData [1];
			BigTwoPoker poker = GetPokerByData (pokerType, pokerFace);
			if (null != poker) {
				listPoker.Add (poker);
			}
		}
		return listPoker;
	}

	private BigTwoPoker GetPokerByData (string pokerType, string pokerFace) {
		 for (int i = 0; i < _listPoker.Count; i++) {
			 BigTwoPoker poker = _listPoker [i];
			 if (pokerType == poker._pokerType.ToString () && pokerFace == poker._pokerFace.ToString ()) {
				 return poker;
			 }
		 }
		 return null;
	}

	public string CombinePokerListToCMD (List<BigTwoPoker> listPoker) {
		string cmd = "";
		for (int i = 0; i < listPoker.Count; i++) {
			BigTwoPoker poker = listPoker [i];
			string pokerStr = string.Format ("{0}{1}{2}", poker._pokerType.ToString (), BigTwoCommandQueue.SPACE_CHAR_POKER, poker._pokerFace.ToString ());
			cmd = string.Format ("{0} {1}", cmd, pokerStr);
		}
		return cmd;
	}

}
