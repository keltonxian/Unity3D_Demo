using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTwoCommandQueue : SingletonKit<BigTwoCommandQueue> {

	public const char SPACE_CHAR_POKER = '_';
	public enum CMD_TYPE {
		PLAY_HAND,
	}
	
	private List<string> _listCMD = new List<string> ();
	private List<string> _listActionCMD = new List<string> ();

	public void Clear () {
		_listCMD.Clear ();
		_listActionCMD.Clear ();
	}

	public void AddCMD (string cmd) {
		if (string.IsNullOrEmpty (cmd)) {
			return;
		}
		_listCMD.Add (cmd);
		_listActionCMD.Add (cmd);
	}

	public string GetLastActionCMD () {
		if (0 == _listActionCMD.Count) {
			return null;
		}
		for (int i = _listActionCMD.Count - 1; i >= 0; i--) {
			string cmd = _listActionCMD [i];
			if (!string.IsNullOrEmpty (cmd)) {
				return cmd;
			}
		}
		return null;
	}

	public void RemoveLastActionCMD () {
		if (0 == _listActionCMD.Count) {
			return;
		}
		for (int i = _listActionCMD.Count - 1; i >= 0; i--) {
			string cmd = _listActionCMD [i];
			if (!string.IsNullOrEmpty (cmd)) {
				_listActionCMD.RemoveAt (i);
				return;
			}
		}
	}

	public string GetLastCMD () {
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

	public List<string> SplitCMDToList (string cmd) {
		List<string> listStr = new List<string> ();
		if (string.IsNullOrEmpty (cmd)) {
			return listStr;
		}
		string[] arrayStr = cmd.Split (' ');
		for (int i = 0; i < arrayStr.Length; i++) {
			string str = arrayStr [i];
			listStr.Add (str);
		}
		return listStr;
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
