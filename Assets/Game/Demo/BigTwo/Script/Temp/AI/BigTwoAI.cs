using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTwoAI : MonoBehaviour {

	public enum WEIGHT {
		ONE = 1, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, ELEVEN, TWELVE, THIRTEEN,
	}

	public virtual string GetPlayCommand (List<BigTwoPoker> currentHand, BigTwoDeck deck) {
		return null;
	}

	protected void GetBiggerSetForSingle (WEIGHT weight, ref List<BigTwoPoker> currentSet, List<BigTwoPoker> currentHand, List<BigTwoPoker> lastSet, BigTwoDeck deck) {
		if (1 != lastSet.Count) {
			return;
		}
		BigTwoPoker lastPoker = lastSet [0];
		List<BigTwoPoker> listTargetPoker = new List<BigTwoPoker> ();
		for (int i = 0; i < currentHand.Count; i++) {
			BigTwoPoker poker = currentHand [i];
			if (BigTwoRule.CompareResult.BIGGER == deck.Rule.IsPokerBigger (poker, lastPoker)) {
				int insertPos = 0;
				for (int j = listTargetPoker.Count - 1; j >= 0; j--) {
					BigTwoPoker pokerInList = listTargetPoker [j];
					if (BigTwoRule.CompareResult.BIGGER == deck.Rule.IsPokerBigger (poker, pokerInList)) {
						insertPos = j + 1;
						break;
					}

				}
				listTargetPoker.Insert (insertPos, poker);
			}
		}
		if (0 == listTargetPoker.Count) {
			return;
		}
		int weightIndex = Mathf.RoundToInt ((int)weight * 1.0f / (int)WEIGHT.THIRTEEN * listTargetPoker.Count) - 1;
		weightIndex = Mathf.Clamp (weightIndex, 0, listTargetPoker.Count - 1);
		BigTwoPoker targetPoker = listTargetPoker [weightIndex];
		currentSet.Add (targetPoker);
	}

	protected void GetBiggerSetForPair (WEIGHT weight, ref List<BigTwoPoker> currentSet, List<BigTwoPoker> currentHand, List<BigTwoPoker> lastSet, BigTwoDeck deck) {
		if (2 != lastSet.Count) {
			return;
		}
		if (lastSet [0]._pokerFace != lastSet [1]._pokerFace) {
			return;
		}
		List<List<BigTwoPoker>> listTargetPair = new List<List<BigTwoPoker>> ();
		for (int i = 0; i < currentHand.Count; i++) {
			BigTwoPoker poker1 = currentHand [i];
			for (int j = i + 1; j < currentHand.Count; j++) {
				BigTwoPoker poker2 = currentHand [j];
				if (poker1._pokerFace == poker2._pokerFace) {
					List<BigTwoPoker> listPoker = new List<BigTwoPoker> ();
					listPoker.Add (poker1);
					listPoker.Add (poker2);
					// PrintListPoker (listPoker);
					// PrintListPoker (lastSet);
					if (BigTwoRule.CompareResult.BIGGER == deck.Rule.IsPairBigger (listPoker, lastSet)) {
						int insertPos = 0;
						for (int k = listTargetPair.Count - 1; k >= 0; k--) {
							List<BigTwoPoker> pairInList = listTargetPair [k];
							if (BigTwoRule.CompareResult.BIGGER == deck.Rule.IsPairBigger (listPoker, pairInList)) {
								insertPos = k + 1;
								break;
							}

						}
						listTargetPair.Insert (insertPos, listPoker);
					}
				}
			}
		}
		if (0 == listTargetPair.Count) {
			return;
		}
		int weightIndex = Mathf.RoundToInt ((int)weight * 1.0f / (int)WEIGHT.THIRTEEN * listTargetPair.Count) - 1;
		weightIndex = Mathf.Clamp (weightIndex, 0, listTargetPair.Count - 1);
		List<BigTwoPoker> targetPair = listTargetPair [weightIndex];
		for (int i = 0; i < targetPair.Count; i++) {
			BigTwoPoker poker = targetPair [i];
			currentSet.Add (poker);
		}
	}

	protected void GetBiggerSetForThree (WEIGHT weight, ref List<BigTwoPoker> currentSet, List<BigTwoPoker> currentHand, List<BigTwoPoker> lastSet, BigTwoDeck deck) {
		if (3 != lastSet.Count) {
			return;
		}
		if (lastSet [0]._pokerFace != lastSet [1]._pokerFace || lastSet [0]._pokerFace != lastSet [2]._pokerFace) {
			return;
		}
		List<List<BigTwoPoker>> listTargetPair = new List<List<BigTwoPoker>> ();
		for (int i = 0; i < currentHand.Count; i++) {
			BigTwoPoker poker1 = currentHand [i];
			for (int j = i + 1; j < currentHand.Count; j++) {
				BigTwoPoker poker2 = currentHand [j];
				if (poker1._pokerFace == poker2._pokerFace) {
					for (int k = j + 1; k < currentHand.Count; k++) {
						BigTwoPoker poker3 = currentHand [k];
						if (poker1._pokerFace == poker3._pokerFace) {
							List<BigTwoPoker> listPoker = new List<BigTwoPoker> ();
							listPoker.Add (poker1);
							listPoker.Add (poker2);
							listPoker.Add (poker3);
							if (BigTwoRule.CompareResult.BIGGER == deck.Rule.IsThreeBigger (listPoker, lastSet)) {
								int insertPos = 0;
								for (int l = listTargetPair.Count - 1; l >= 0; l--) {
									List<BigTwoPoker> pairInList = listTargetPair [l];
									if (BigTwoRule.CompareResult.BIGGER == deck.Rule.IsThreeBigger (listPoker, pairInList)) {
										insertPos = l + 1;
										break;
									}

								}
								listTargetPair.Insert (insertPos, listPoker);
							}
						}
					}
				}
			}
		}
		if (0 == listTargetPair.Count) {
			return;
		}
		int weightIndex = Mathf.RoundToInt ((int)weight * 1.0f / (int)WEIGHT.THIRTEEN * listTargetPair.Count) - 1;
		weightIndex = Mathf.Clamp (weightIndex, 0, listTargetPair.Count - 1);
		List<BigTwoPoker> targetPair = listTargetPair [weightIndex];
		for (int i = 0; i < targetPair.Count; i++) {
			BigTwoPoker poker = targetPair [i];
			currentSet.Add (poker);
		}
	}

	private void PrintListPoker (List<BigTwoPoker> listPoker) {
		string str = string.Format ("PokerList[{0}]:", listPoker.Count);
		for (int i = 0; i < listPoker.Count; i++) {
			BigTwoPoker poker = listPoker [i];
			str = string.Format ("{0} [{1}][{2}]", str, poker._pokerType.ToString (), poker._pokerFace.ToString ());
		}
		Debug.Log (str);
	}

}
