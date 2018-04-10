using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTwoRule : MonoBehaviour {

	public enum CompareResult {
		INVALID,
		BIGGER,
		SMALLER,
	}

	public void Init () {
		
	}

	private int PokerTypeGrade (BigTwoPoker poker) {
		if (poker._pokerType == BigTwoPoker.TYPE.DIAMONDS) {
			return 1;
		}
		if (poker._pokerType == BigTwoPoker.TYPE.CLUBS) {
			return 2;
		}
		if (poker._pokerType == BigTwoPoker.TYPE.HEARTS) {
			return 3;
		}
		if (poker._pokerType == BigTwoPoker.TYPE.SPADES) {
			return 4;
		}
		return 0;
	}

	private int PokerFaceGrade (BigTwoPoker poker) {
		if (poker._pokerFace == BigTwoPoker.FACE.THREE) {
			return 1;
		}
		if (poker._pokerFace == BigTwoPoker.FACE.FOUR) {
			return 2;
		}
		if (poker._pokerFace == BigTwoPoker.FACE.FIVE) {
			return 3;
		}
		if (poker._pokerFace == BigTwoPoker.FACE.SIX) {
			return 4;
		}
		if (poker._pokerFace == BigTwoPoker.FACE.SEVEN) {
			return 5;
		}
		if (poker._pokerFace == BigTwoPoker.FACE.EIGHT) {
			return 6;
		}
		if (poker._pokerFace == BigTwoPoker.FACE.NINE) {
			return 7;
		}
		if (poker._pokerFace == BigTwoPoker.FACE.TEN) {
			return 8;
		}
		if (poker._pokerFace == BigTwoPoker.FACE.JACK) {
			return 9;
		}
		if (poker._pokerFace == BigTwoPoker.FACE.QUEEN) {
			return 10;
		}
		if (poker._pokerFace == BigTwoPoker.FACE.KING) {
			return 11;
		}
		if (poker._pokerFace == BigTwoPoker.FACE.ACE) {
			return 12;
		}
		if (poker._pokerFace == BigTwoPoker.FACE.TWO) {
			return 13;
		}
		return 0;
	}

	public CompareResult IsPokerBigger (BigTwoPoker pokerA, BigTwoPoker pokerB) {
		int faceA = PokerFaceGrade (pokerA);
		int faceB = PokerFaceGrade (pokerB);
		if (faceA > faceB) {
			return CompareResult.BIGGER;
		}
		if (faceA < faceB) {
			return CompareResult.SMALLER;
		}
		int typeA = PokerTypeGrade (pokerA);
		int typeB = PokerTypeGrade (pokerB);
		if (typeA > typeB) {
			return CompareResult.BIGGER;
		}
		return CompareResult.SMALLER;
	}

	public CompareResult IsPairBigger (List<BigTwoPoker> listA, List<BigTwoPoker> listB) {
		if (listA.Count != 2 || listB.Count != 2) {
			return CompareResult.INVALID;
		}
		int faceA0 = PokerFaceGrade (listA [0]);
		int faceA1 = PokerFaceGrade (listA [1]);
		int faceB0 = PokerFaceGrade (listB [0]);
		int faceB1 = PokerFaceGrade (listB [1]);
		if (faceA0 != faceA1 || faceB0 != faceB1) {
			return CompareResult.INVALID;
		}
		if (faceA0 > faceB0) {
			return CompareResult.BIGGER;
		}
		if (faceA0 < faceB0) {
			return CompareResult.SMALLER;
		}
		int typeA0 = PokerTypeGrade (listA [0]);
		int typeA1 = PokerTypeGrade (listA [1]);
		int typeB0 = PokerTypeGrade (listB [0]);
		int typeB1 = PokerTypeGrade (listB [1]);
		int typeA = typeA0 > typeA1 ? typeA0 : typeA1;
		int typeB = typeB0 > typeB1 ? typeB0 : typeB1;
		if (typeA > typeB) {
			return CompareResult.BIGGER;
		}
		if (typeA < typeB) {
			return CompareResult.SMALLER;
		}
		return CompareResult.INVALID;
	}

	public CompareResult IsThreeBigger (List<BigTwoPoker> listA, List<BigTwoPoker> listB) {
		if (listA.Count != 3 || listB.Count != 3) {
			return CompareResult.INVALID;
		}
		int faceA0 = PokerFaceGrade (listA [0]);
		int faceA1 = PokerFaceGrade (listA [1]);
		int faceA2 = PokerFaceGrade (listA [2]);
		if (faceA0 != faceA1 || faceA0 != faceA2) {
			return CompareResult.INVALID;
		}
		int faceB0 = PokerFaceGrade (listB [0]);
		int faceB1 = PokerFaceGrade (listB [1]);
		int faceB2 = PokerFaceGrade (listB [2]);
		if (faceB0 != faceB1 || faceB0 != faceB2) {
			return CompareResult.INVALID;
		}
		if (faceA0 > faceB0) {
			return CompareResult.BIGGER;
		}
		if (faceA0 < faceB0) {
			return CompareResult.SMALLER;
		}
		int typeA1 = PokerTypeGrade (listA [0]);
		int typeA2 = PokerTypeGrade (listA [1]);
		int typeA3 = PokerTypeGrade (listA [2]);
		int typeB1 = PokerTypeGrade (listB [0]);
		int typeB2 = PokerTypeGrade (listB [1]);
		int typeB3 = PokerTypeGrade (listB [2]);
		int typeA = typeA1 > typeA2 ? typeA1 : typeA2;
		typeA = typeA3 > typeA ? typeA3 : typeA;
		int typeB = typeB1 > typeB2 ? typeB1 : typeB2;
		typeB = typeB3 > typeB ? typeB3 : typeB;
		if (typeA > typeB) {
			return CompareResult.BIGGER;
		}
		if (typeA < typeB) {
			return CompareResult.SMALLER;
		}
		return CompareResult.INVALID;
	}

	private int PokerStraightGrade (List<BigTwoPoker> list) {
		int grade = 0;
		if (list.Count != 5) {
			return grade;
		}
		BigTwoPoker p1 = list [0];
		BigTwoPoker p2 = list [1];
		BigTwoPoker p3 = list [2];
		BigTwoPoker p4 = list [3];
		BigTwoPoker p5 = list [4];
		int f1 = PokerFaceGrade (p1);
		int f2 = PokerFaceGrade (p2);
		int f3 = PokerFaceGrade (p3);
		int f4 = PokerFaceGrade (p4);
		int f5 = PokerFaceGrade (p5);
		if (f5 - f4 == 1 && f4 - f3 == 1 && f3 - f2 == 1 && f2 - f1 == 1) {
			return f1;
		}
		return grade;
	}

	private int PokerFlowerGrade (List<BigTwoPoker> list) {
		int grade = 0;
		if (list.Count != 5) {
			return grade;
		}
		BigTwoPoker p1 = list [0];
		BigTwoPoker p2 = list [1];
		BigTwoPoker p3 = list [2];
		BigTwoPoker p4 = list [3];
		BigTwoPoker p5 = list [4];
		int f5 = PokerFaceGrade (p5);
		int t1 = PokerTypeGrade (p1);
		int t2 = PokerTypeGrade (p2);
		int t3 = PokerTypeGrade (p3);
		int t4 = PokerTypeGrade (p4);
		int t5 = PokerTypeGrade (p5);
		if (t1 == t2 && t2 == t3 && t3 == t4 && t4 == t5) {
			grade += 100 * t1;
			grade += f5;
		}
		return grade;
	}

	private int PokerFullHouseGrade (List<BigTwoPoker> list) {
		int grade = 0;
		if (list.Count != 5) {
			return grade;
		}
		BigTwoPoker p1 = list [0];
		BigTwoPoker p2 = list [1];
		BigTwoPoker p3 = list [2];
		BigTwoPoker p4 = list [3];
		BigTwoPoker p5 = list [4];
		int f1 = PokerFaceGrade (p1);
		int f2 = PokerFaceGrade (p2);
		int f3 = PokerFaceGrade (p3);
		int f4 = PokerFaceGrade (p4);
		int f5 = PokerFaceGrade (p5);
		if (f1 == f2 && f2 == f3 && f4 == f5) {
			grade = f1;
		} else if (f1 == f2 && f3 == f4 && f4 == f5) {
			grade = f3;
		}
		return grade;
	}

	private int PokerKingKongGrade (List<BigTwoPoker> list) {
		int grade = 0;
		if (list.Count != 5) {
			return grade;
		}
		BigTwoPoker p1 = list [0];
		BigTwoPoker p2 = list [1];
		BigTwoPoker p3 = list [2];
		BigTwoPoker p4 = list [3];
		BigTwoPoker p5 = list [4];
		int f1 = PokerFaceGrade (p1);
		int f2 = PokerFaceGrade (p2);
		int f3 = PokerFaceGrade (p3);
		int f4 = PokerFaceGrade (p4);
		int f5 = PokerFaceGrade (p5);
		if (f1 == f2 && f2 == f3 && f3 == f4) {
			grade = f1;
		} else if (f2 == f3 && f3 == f4 && f4 == f5) {
			grade = f2;
		}
		return grade;
	}

	private int PokerStraightFlushGrade (List<BigTwoPoker> list) {
		int grade = 0;
		if (list.Count != 5) {
			return grade;
		}
		BigTwoPoker p1 = list [0];
		BigTwoPoker p2 = list [1];
		BigTwoPoker p3 = list [2];
		BigTwoPoker p4 = list [3];
		BigTwoPoker p5 = list [4];
		int gradeStraight = PokerStraightGrade (list);
		int gradeFlower = PokerFlowerGrade (list);
		if (gradeStraight == 0 || gradeFlower == 0) {
			return grade;
		}
		grade = gradeFlower;
		return grade;
	}

	private CompareResult IsFiveBigger (List<BigTwoPoker> listA, List<BigTwoPoker> listB) {
		if (listA.Count != 5 || listB.Count != 5) {
			return CompareResult.INVALID;
		}
		int straightFlushGradeA = PokerStraightFlushGrade (listA);
		int straightFlushGradeB = PokerStraightFlushGrade (listB);
		if (straightFlushGradeA > 0 || straightFlushGradeB > 0) {
			if (straightFlushGradeA > straightFlushGradeB) {
				return CompareResult.BIGGER;
			} else if (straightFlushGradeB > straightFlushGradeA) {
				return CompareResult.SMALLER;
			}
		}
		int kingkongGradeA = PokerKingKongGrade (listA);
		int kingkongGradeB = PokerKingKongGrade (listB);
		if (kingkongGradeA > 0 || kingkongGradeB > 0) {
			if (kingkongGradeA > kingkongGradeB) {
				return CompareResult.BIGGER;
			} else if (kingkongGradeB > kingkongGradeA) {
				return CompareResult.SMALLER;
			}
		}
		int fullhouseGradeA = PokerFullHouseGrade (listA);
		int fullhouseGradeB = PokerFullHouseGrade (listB);
		if (fullhouseGradeA > 0 || fullhouseGradeB > 0) {
			if (fullhouseGradeA > fullhouseGradeB) {
				return CompareResult.BIGGER;
			} else if (fullhouseGradeB > fullhouseGradeA) {
				return CompareResult.SMALLER;
			}
		}
		int flowerGradeA = PokerFlowerGrade (listA);
		int flowerGradeB = PokerFlowerGrade (listB);
		if (flowerGradeA > 0 || flowerGradeB > 0) {
			if (flowerGradeA > flowerGradeB) {
				return CompareResult.BIGGER;
			} else if (flowerGradeB > flowerGradeA) {
				return CompareResult.SMALLER;
			}
		}
		int straightGradeA = PokerStraightGrade (listA);
		int straightGradeB = PokerStraightGrade (listB);
		if (straightGradeA > 0 || straightGradeB > 0) {
			if (straightGradeA > straightGradeB) {
				return CompareResult.BIGGER;
			} else if (straightGradeB > straightGradeA) {
				return CompareResult.SMALLER;
			}
		}
		return CompareResult.INVALID;
	}

	public void SortPokerList (List<BigTwoPoker> list) {
		List<BigTwoPoker> temp = new List<BigTwoPoker> ();
		for (int i = 0; i < list.Count; i++) {
			temp.Add (list [i]);
		}
		list.Clear ();
		for (int i = 0; i < temp.Count; i++) {
			BigTwoPoker poker = temp [i];
			int index = list.Count;
			for (int j = list.Count - 1; j >= 0; j--) {
				BigTwoPoker p = list [j];
				if (PokerFaceGrade (poker) > PokerFaceGrade (p)) {
					break;
				} else if (PokerFaceGrade (poker) == PokerFaceGrade (p) && PokerTypeGrade (poker) > PokerTypeGrade (p)) {
					break;
				}
				index--;
			}
			list.Insert (index, poker);
		}
		temp.Clear ();
	}

	public CompareResult IsPokerListBigger (List<BigTwoPoker> listA, List<BigTwoPoker> listB) {
		if (listA.Count != listB.Count) {
			return CompareResult.INVALID;
		}
		if (1 == listA.Count) {
			return IsPokerBigger (listA [0], listB [0]);
		}
		if (2 == listA.Count) {
			return IsPairBigger (listA, listB);
		}
		if (3 == listA.Count) {
			return IsThreeBigger (listA, listB);
		}
		if (5 == listA.Count) {
			return IsFiveBigger (listA, listB);
		}
		return CompareResult.INVALID;
	}

}
