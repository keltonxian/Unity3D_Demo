using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class LockItemKit : MonoBehaviour {

	public LockManager.IAP_TYPE _iapType = LockManager.IAP_TYPE.NONE;
	public string _lockKey = null;
	public Sprite _lockSprite = null;
	public Vector2 _lockOffset = Vector2.zero;
	public float _lockScale = 1f;
	public Vector2 _lockAreaSize = Vector2.zero;
	private GameObject _itemLock;
//	private CallbackType.CallbackB _iapUnlockCallback;
	private CallbackType.CallbackB _videoUnlockCallback;
	private LockItemHandler _lockItemHandler;
	public bool _isAutoAddLock = true;

	private bool _isLockForTool = false;
	public bool IsLockForTool {
		get {
			return _isLockForTool;
		}
		set {
			_isLockForTool = value;
		}
	}

	void Awake () {
		_lockItemHandler = GameObject.Find ("UICanvas").GetComponent<LockItemHandler> ();
	}

	void Start () {
		if (!_isAutoAddLock) {
			return;
		}
		AddLock ((bool isUnlock) => {
			UnlockVideoLock (isUnlock);
		}, (bool isUnlock) => {
			UnlockVideoLock (isUnlock);
		});
		if (IsLock ()) {
			SetToolTouchEnabled (false);
		}
	}

	public void AddLock (CallbackType.CallbackB iapCallback = null, CallbackType.CallbackB videoCallback = null) {
		if (string.IsNullOrEmpty (_lockKey) || null == _lockSprite) {
			return;
		}
//		if (_iapType == LockManager.IAP_TYPE.LEVELS && LockManager.Instance.IsLevelsItemUnlocked (_lockKey)) {
//			return;
//		}
//		if (_iapType == LockManager.IAP_TYPE.CHARACTERS && LockManager.Instance.IsCharactersItemUnlocked (_lockKey)) {
//			return;
//		}
//		if (_iapType == LockManager.IAP_TYPE.DECORATIONS && LockManager.Instance.IsDecorationsItemUnlocked (_lockKey)) {
//			return;
//		}
//		_iapUnlockCallback = iapCallback;
		_videoUnlockCallback = videoCallback;
		if (Vector2.zero == _lockAreaSize) {
			_lockAreaSize = this.GetComponent<RectTransform> ().sizeDelta;
		}
		GameObject itemLock = new GameObject ("ItemLock");
		itemLock.AddComponent<Image> ();
		itemLock.AddComponent<KTButton> ();
		RectTransform itemLockRT = itemLock.GetComponent<RectTransform> ();
		itemLockRT.SetParent (this.GetComponent<RectTransform> ());
		itemLock.transform.localPosition = Vector3.zero;
		itemLockRT.sizeDelta = _lockAreaSize;
		itemLockRT.localScale = Vector3.one;
		itemLockRT.localPosition = Vector3.zero;
		itemLockRT.anchoredPosition = Vector3.zero;
		itemLock.GetComponent<Image> ().color = new Color (0f, 0f, 0f, 0f);
		itemLock.GetComponent<KTButton> ().OnAnimDone.AddListener (Unlock);
		GameObject lockImage = new GameObject ("ItemLockImage");
		lockImage.AddComponent<Image> ();
		RectTransform lockImageRT = lockImage.GetComponent<RectTransform> ();
		lockImageRT.SetParent (itemLock.GetComponent<RectTransform> ());
		lockImage.transform.localPosition = Vector3.zero;
		lockImageRT.localScale = new Vector3 (_lockScale, _lockScale, 1f);
		lockImageRT.anchoredPosition = _lockOffset;
		lockImage.GetComponent<Image> ().sprite = _lockSprite;
		lockImage.GetComponent<Image> ().SetNativeSize ();

		float scale = 0.6f;
		lockImageRT.localScale *= scale;
		Vector2 lockImageSize = lockImageRT.sizeDelta * scale;
		lockImageRT.anchorMin = new Vector2 (0f, 1f);
		lockImageRT.anchorMax = new Vector2 (0f, 1f);
		lockImageRT.anchoredPosition = new Vector2 (lockImageSize.x / 2f, -lockImageSize.y / 2f);

		SetItemLock (itemLock);
	}

	private void SetItemLock (GameObject itemLock = null) {
		_itemLock = itemLock;
	}

	public void RemoveLock (bool hasParticle = true, bool hasMusic = true, CallbackType.CallbackV callback = null) {
		if (null == _itemLock) {
			return;
		}
		GameObject itemLock = _itemLock;
		SetItemLock ();

		_lockItemHandler.VideoUnlock (itemLock, (GameObject lockItem) => {
		}, hasParticle, hasMusic);
	}

	public bool IsLock () {
		return (_itemLock != null);
	}

	private void UnlockByInterstitialAds () {
		LockManager.Instance.UnlockItemByVideo (_lockKey);
		RemoveLock ();
		if (null != _videoUnlockCallback) {
			_videoUnlockCallback (true);
		}
	}

	private void GetItForFree () {
		if (string.IsNullOrEmpty (_lockKey)) {
			return;
		}
		if (Application.isEditor) {
			UnlockByInterstitialAds ();
			return;
		}
//		PluginManager.Instance.ShowInterstitialRewardAd (delegate(bool flag) {
//			if (flag) {
//				UnlockByInterstitialAds ();
//			}
//		});
	}

	public void Unlock () {
		if (!IsLock ()) {
			return;
		}

		GetItForFree ();
	}

	private void SetToolTouchEnabled (bool isEnabled) {
//		if (null != this.GetComponent<UGUIDrag> ()) {
//			UGUIDrag target = this.GetComponent<UGUIDrag> ();
//			target.enabled = isEnabled;
//		}
		if (null != this.GetComponent<KTButton> ()) {
			KTButton target = this.GetComponent<KTButton> ();
			target._isTouchEnabled = isEnabled;
		}
	}

	public void SetToolLock (bool isLock) {
		IsLockForTool = isLock;
		SetToolTouchEnabled (!(isLock || IsLock ()));
	}

	public void UnlockVideoLock (bool isUnlock) {
		if (IsLockForTool) {
			isUnlock = false;
		}
		SetToolTouchEnabled (isUnlock);
	}

}
