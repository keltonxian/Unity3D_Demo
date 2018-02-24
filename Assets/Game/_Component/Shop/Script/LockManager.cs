using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockManager : SingletonKit<LockManager> {

	public enum IAP_TYPE {
		NONE,
		FULL,
	}

	public enum SCENE_TYPE {
		NONE = 0,
		LEVEL_1 = 1,
		LEVEL_2,
		LEVEL_3,
		LEVEL_4,
		LEVEL_5,
		LEVEL_6,
		LEVEL_7,
		LEVEL_8,
	}

	protected override void Awake () {
		base.Awake ();
		this.gameObject.name = "LockManager";
	}

	public string LockSceneKey (SCENE_TYPE sceneType) {
		return string.Format ("SCENE_TYPE_SCENE_{0}", (int)sceneType);
	}

	#region video lock
	public bool IsItemUnlockedByVideo (string itemKey) {
		//当前的分钟tick
		int nowMin = (int)(System.DateTime.Now.Ticks * 0.0000001f / 60f);
		//unlock时保存的时间
		int savedMin = PlayerPrefs.GetInt (itemKey, nowMin - 48 * 60);
		if (nowMin - savedMin < 24 * 60) {
			return true;
		}
		return false;
	}

	public void UnlockItemByVideo (string itemKey) {
		//当前的分钟tick
		int nowMin = (int)(System.DateTime.Now.Ticks * 0.0000001f / 60f);
		PlayerPrefs.SetInt (itemKey, nowMin);
		PlayerPrefs.Save ();
	}
	#endregion

	#region iap lock
	public bool IsAllUnlock () {
		bool isUnlock = false;
		if (LockManager.Instance.IsFullUnlocked ()) {
			isUnlock = true;
		}
		return isUnlock;
	}

	public bool IsFullUnlocked () {
		if (GameDataManager.NoAds && GameDataManager.NoLock) {
			return true;
		}
		if (string.IsNullOrEmpty (GameDataManager.Instance.KEY_FULL)) {
			return true;
		}
		if (PlayerPrefs.GetInt (GameDataManager.Instance.KEY_FULL, 0) == 1) {
			return true;
		}
		return false;
	}

	public void UnlockFull () {
		PlayerPrefs.SetInt (GameDataManager.Instance.KEY_FULL, 1);
		PlayerPrefs.Save ();
	}
	#endregion

}
