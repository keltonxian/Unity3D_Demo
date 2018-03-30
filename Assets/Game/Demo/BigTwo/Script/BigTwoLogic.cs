using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BigTwoLogic : MonoBehaviour {

	public BigTwoDeck _deck;
	public BigTwoGamePlayer _playerSeatDown;
	public BigTwoGamePlayer _playerSeatLeft;
	public BigTwoGamePlayer _playerSeatUp;
	public BigTwoGamePlayer _playerSeatRight;
	private List<BigTwoGamePlayer> _listPlayer = new List<BigTwoGamePlayer> ();
	public GameObject _btnStart;
	private int _playerIndex;
	private int _playCount;
	private bool _isInAction = false;

	public void Init () {
		InitDeck ();
		InitCMDQueue ();
		InitPlayer ();
	}

	private void InitDeck () {
		_deck.Init ();
	}

	private void InitCMDQueue () {
		BigTwoCommandQueue.Instance.Clear ();
	}

	private void InitPlayer () {
		_playerSeatDown.Init (BigTwoGamePlayer.PLAYER_SEAT.DOWN);
		_listPlayer.Add (_playerSeatDown);
		_playerSeatLeft.Init (BigTwoGamePlayer.PLAYER_SEAT.LEFT);
		_listPlayer.Add (_playerSeatLeft);
		_playerSeatUp.Init (BigTwoGamePlayer.PLAYER_SEAT.UP);
		_listPlayer.Add (_playerSeatUp);
		_playerSeatRight.Init (BigTwoGamePlayer.PLAYER_SEAT.RIGHT);
		_listPlayer.Add (_playerSeatRight);
	}

	public void ReadyGame (CallbackType.CallbackV callback) {
		_btnStart.SetActive (true);
	}

	public void StartGame () {
		_btnStart.SetActive (false);
		_isInAction = false;
		float time = _deck.DealCard (_listPlayer);
		Sequence sequence = DOTween.Sequence();
		sequence.PrependInterval (time);
		sequence.AppendCallback (()=>{
			//			_btnRound.SetActive (true);
			_playerIndex = -1;
			_playCount = 0;
			NextPlayer ();
		});
	}

	void Update () {
		CheckCMD ();
	}

	private void NextPlayer () {
		_playerIndex++;
		//		if (_playerIndex == 2) {
		//			return;
		//		}
		if (_playerIndex >= _listPlayer.Count) {
			_playerIndex = 0;
		}
		BigTwoGamePlayer player = _listPlayer [_playerIndex];
		if (player.HasPlayAll ()) {
			_btnStart.SetActive (true);
			return;
		}
		string cmd = player.NextPlayCMD ();
		BigTwoCommandQueue.Instance.AddCMD (cmd);

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

	private void CheckCMD () {
		if (_isInAction) {
			return;
		}
		string cmd = BigTwoCommandQueue.Instance.GetLastActionCMD ();
		BigTwoCommandQueue.Instance.RemoveLastActionCMD ();
		if (null == cmd) {
			return;
		}
		_isInAction = true;
		Debug.Log (string.Format ("Cmd[{0}]", cmd));
		List<string> listStr = BigTwoCommandQueue.Instance.SplitCMDToList (cmd);
		string actionStr = listStr [0];
		if (actionStr == BigTwoCommandQueue.CMD_TYPE.PLAY_HAND.ToString ()) {
			PlayAHand (listStr);
		}
	}

	private BigTwoGamePlayer GetPlayerByStr (string str) {
		if (string.IsNullOrEmpty (str)) {
			return null;
		}
		if (str == BigTwoGamePlayer.PLAYER_SEAT.DOWN.ToString ()) {
			return _playerSeatDown;
		}
		if (str == BigTwoGamePlayer.PLAYER_SEAT.LEFT.ToString ()) {
			return _playerSeatLeft;
		}
		if (str == BigTwoGamePlayer.PLAYER_SEAT.UP.ToString ()) {
			return _playerSeatUp;
		}
		if (str == BigTwoGamePlayer.PLAYER_SEAT.RIGHT.ToString ()) {
			return _playerSeatRight;
		}
		return null;
	}

	private void PlayAHand (List<string> listStr) {
		string playerStr = listStr [1];
		BigTwoGamePlayer player = GetPlayerByStr (playerStr);
		if (null == player) {
			return;
		}
		player.PlayHandToSet (listStr, () => {
			NextPlayer ();
			_isInAction = false;
		});
	}

}
