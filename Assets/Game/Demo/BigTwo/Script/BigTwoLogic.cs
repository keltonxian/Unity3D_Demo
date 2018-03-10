using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BigTwoLogic : MonoBehaviour {

	public BigTwoDeck _deck;
	public BigTwoGamePlayer[] _arrayPlayer;
	public GameObject _btnStart;
	private int _playerIndex;
	private int _playCount;

	public void Init () {
	}

	public void ReadyGame (CallbackType.CallbackV callback) {
		_deck.InitDeck (callback);
		_btnStart.SetActive (true);
	}

	public void StartGame () {
		_btnStart.SetActive (false);
		float time = _deck.DealCard ();
		Sequence sequence = DOTween.Sequence();
		sequence.PrependInterval (time);
		sequence.AppendCallback (()=>{
			//			_btnRound.SetActive (true);
			_playerIndex = -1;
			_playCount = 0;
			NextPlayer ();
		});
	}

	private void NextPlayer () {
		_playerIndex++;
		//		if (_playerIndex == 2) {
		//			return;
		//		}
		if (_playerIndex >= _arrayPlayer.Length) {
			_playerIndex = 0;
		}
		BigTwoGamePlayer player = _arrayPlayer [_playerIndex];
		if (player.HasPlayAll ()) {
			_btnStart.SetActive (true);
			return;
		}
		string cmd = player.NextPlayCMD ();
		Debug.Log ("Cmd: "+cmd);

		// player.PlayAHand (prevPlayerHand, (List<Poker> hand, bool isPass)=>{
		// 	Sequence sequence = DOTween.Sequence();
		// 	sequence.PrependInterval (1f);
		// 	sequence.AppendCallback (()=>{
		// 		if (isPass) {
		// 			_playCount++;
		// 		} else {
		// 			_playCount = 0;
		// 		}
		// 		if (_playCount == _arrayPlayer.Length - 1) {
		// 			_playCount = 0;
		// 			hand = null;
		// 		}
		// 		NextPlayer (hand);
		// 	});
		// });

		//		if (_playerIndex > 0) {
		//			_btnPlay.SetActive (false);
		//			return;
		//		}
		//		_btnPlay.SetActive (true);
	}

}
