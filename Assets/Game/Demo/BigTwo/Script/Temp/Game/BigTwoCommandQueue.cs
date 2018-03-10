using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTwoCommandQueue : MonoBehaviour {

	private BigTwoDeck _deck;

	private List<string> _listCMD = new List<string> ();

	public void Init (BigTwoDeck deck) {
		_deck = deck;
	}

	public string GetLastValidCMD () {
		if (0 == _listCMD.Count) {
			return null;
		}
		for (int i = _listCMD.Count - 1; i >= 0; i--) {
			string cmd = _listCMD [i];
			if (!string.IsNullOrEmpty (cmd)) {
				return cmd;
			}
		}
		return null;
	}

	public List<BigTwoPoker> GetLastValidPokerList () {
		string cmd = GetLastValidCMD ();	
		return SplitCMDToPokerList (cmd);
	}

	public string CombinePokerListToCMD (List<BigTwoPoker> listPoker) {
		string cmd = "";
		for (int i = 0; i < listPoker.Count; i++) {
			BigTwoPoker poker = listPoker [i];
			string pokerStr = string.Format ("{0}_{1}", poker._pokerType.ToString (), poker._pokerFace.ToString ());
			cmd = string.Format ("{0} {1}", cmd, pokerStr);
		}
		return cmd;
	}

	private List<BigTwoPoker> SplitCMDToPokerList (string cmd) {
		List<BigTwoPoker> listPoker = new List<BigTwoPoker> ();
		if (string.IsNullOrEmpty (cmd)) {
			return listPoker;
		}
		string[] arrayPoker = cmd.Split (' ');
		for (int i = 0; i < arrayPoker.Length; i++) {
			string pokerStr = arrayPoker [i];
			string[] pokerData = pokerStr.Split ('_');
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
		 List<BigTwoPoker> listPoker = _deck.ListPoker;
		 for (int i = 0; i < listPoker.Count; i++) {
			 BigTwoPoker poker = listPoker [i];
			 if (pokerType == poker._pokerType.ToString () && pokerFace == poker._pokerFace.ToString ()) {
				 return poker;
			 }
		 }
		 return null;
	}

	public bool IsRoundPassed () {
		if (_listCMD.Count < 4) {
			return false;
		}	
		bool isAllPassed = true;
		for (int i = _listCMD.Count - 1; i >= _listCMD.Count - 3; i--) {
			if (!string.IsNullOrEmpty (_listCMD [i])) {
				isAllPassed = false;
			}
		}
		return isAllPassed;
	}

}
