using System.Collections;
using UnityEngine;

public class GameDataManager : SingletonKit<GameDataManager> {

	public static bool DebugMute = false;
	public static bool AllLevelPass = false;
	public static bool NoLock = true;
	public static bool NoAds = false;

	public static string ScreenName (string name) {
		return string.Format ("tabtale_{0}", name);
	}

	public enum LEVEL_TYPE {
		LEVEL_1 = 0,
		LEVEL_2 = 1,
		LEVEL_3 = 2,
		LEVEL_4 = 3,
		LEVEL_5 = 4,
		LEVEL_6 = 5,
		LEVEL_7 = 6,
		SHOP = 20,
		NONE = 100,
	}

	[Header("iOS iap key")]
	public string KEY_FULL_IOS = "com.kwork.demo.full";
	[Header("android iap key")]
	public string KEY_FULL_ANDROID = "com.kwork.demo.full";

	public string KEY_FULL {
		get { return (Application.platform == RuntimePlatform.IPhonePlayer ? KEY_FULL_IOS : KEY_FULL_ANDROID); }
	}

	[Header("facebook url")]
	public string URL_FACEBOOK = "https://www.facebook.com/pages/Beauty-Inc/1490275731254673";
	[Header("privacy policy url")]
	public string URL_PRIVACYPOLICY = "http://www.beautyapps.net/privacy-policy/";

	protected override void Awake () {
		base.Awake ();
		this.gameObject.name = "GameManager";
		Init ();
	}

	private LockManager.SCENE_TYPE _lastSceneType = LockManager.SCENE_TYPE.NONE;
	public LockManager.SCENE_TYPE LastSceneType {
		get { return _lastSceneType; }
		set { _lastSceneType = value; }
	}

	private void Init () {
		_lastSceneType = LockManager.SCENE_TYPE.NONE;
	}

	public void Reset () {
		_lastSceneType = LockManager.SCENE_TYPE.NONE;
	}

//	private List<int> _listSidebarSoundEffect = new List<int> ();
//	private int _playSidebarSoundEffectCount = 0;
//	public void PlayRandomSidebarSoundEffect (int specifiedIndex = -1) {
//		if (specifiedIndex != -1) {
//			_listSidebarSoundEffect.Clear ();
//			_playSidebarSoundEffectCount = 0;
//		}
//		_playSidebarSoundEffectCount++;
//		if (_playSidebarSoundEffectCount > 1) {
//			int playGap = 2;
//			if (_playSidebarSoundEffectCount > playGap) {
//				_playSidebarSoundEffectCount = 0;
//			}
//			return;
//		}
//		if (_listSidebarSoundEffect.Count == 0) {
//			int total = 6;
//			for (int i = 0; i < total; i++) {
//				_listSidebarSoundEffect.Add (i);
//			}
//		}
//		int listIndex = ((specifiedIndex != -1) ? specifiedIndex : Random.Range (0, _listSidebarSoundEffect.Count));
//		int index = _listSidebarSoundEffect [listIndex];
//		_listSidebarSoundEffect.Remove (index);
////		AudioManager.Instance.PlaySoundEffect (string.Format ("SidebarSoundEffect/{0}", index));
//	}

}
