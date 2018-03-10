using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTwoAISimple : BigTwoAI {

	public override string GetPlayCommand (List<BigTwoPoker> currentHand, BigTwoDeck deck) {
		List<BigTwoPoker> currentSet = new List<BigTwoPoker> ();
		GeneratePlaySet (ref currentSet, currentHand, deck);
		string cmd = deck.CommandQueue.CombinePokerListToCMD (currentSet);
		return cmd;
	}

	private void GeneratePlaySet (ref List<BigTwoPoker> currentSet, List<BigTwoPoker> currentHand, BigTwoDeck deck) {
		BigTwoCommandQueue commandQueue = deck.CommandQueue;
		if (commandQueue.IsRoundPassed ()) {
			GetBestInHand (ref currentSet, currentHand, deck);
			return;
		}
		List<BigTwoPoker> lastSet = commandQueue.GetLastValidPokerList ();
		if (0 == lastSet.Count) {
			GetBestInHand (ref currentSet, currentHand, deck);
			return;
		}
	}

	private void GetBestInHand (ref List<BigTwoPoker> currentSet, List<BigTwoPoker> currentHand, BigTwoDeck deck) {
		currentSet.Add (currentHand [0]);
		return;
	}

	private void GetBiggerSet (ref List<BigTwoPoker> currentSet, List<BigTwoPoker> currentHand, List<BigTwoPoker> lastSet, BigTwoDeck deck) {
		// TODO delete
		if (lastSet.Count != 1) {
			return;
		}
		// ----
		BigTwoPoker lastPoker = lastSet [0];
		BigTwoPoker targetPoker = null;
		for (int i = 0; i < currentHand.Count; i++) {
			BigTwoPoker poker = currentHand [i];
			if (BigTwoRule.CompareResult.BIGGER ==  deck.Rule.IsPokerBigger (poker, lastPoker)) {
				targetPoker = poker;
				break;
			}
		}
		if (null == targetPoker) {
			return;
		}
		currentSet.Add (targetPoker);
	}

}
