using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ShopHandler : PopupViewKit {

	private string _videoLockKey;
	public string VideoLockKey {
		get {
			return _videoLockKey;
		}
		set {
			_videoLockKey = value;
		}
	}

	private CallbackType.CallbackB _videoLockCallback;
	public CallbackType.CallbackB VideoLockCallback {
		get {
			return _videoLockCallback;
		}
		set {
			_videoLockCallback = value;
		}
	}

	public delegate void IapCallbackDelegate (LockManager.IAP_TYPE iapType);
	private IapCallbackDelegate _iapCallback;
	public IapCallbackDelegate IapCallback {
		get {
			return _iapCallback;
		}
		set {
			_iapCallback = value;
		}
	}

	private CallbackType.CallbackV _shopCloseCallback;
	public CallbackType.CallbackV ShopCloseCallback {
		get {
			return _shopCloseCallback;
		}
		set {
			_shopCloseCallback = value;
		}
	}

	protected override void Awake () {
		base.Awake ();
	}

	protected override void Start () {
		AddShopListener ();
		base.Start ();
	}

	protected override void Init () {
	}

	protected override void ActionPreClose () {
		base.ActionPreClose ();
		RemoveNetLoading ();
		RemoveShopListener ();
	}

	protected override void ActionInClose () {
		base.ActionInClose ();
	}

	protected override void ActionInOnClickClose () {
		base.ActionInOnClickClose ();
		if (null != _shopCloseCallback) {
			_shopCloseCallback ();
		}
	}

	public virtual void ShowNetLoading () {
		NetLoading.Show ();
	}

	public virtual void RemoveNetLoading () {
		NetLoading.Remove (2f);
	}

	private void AddShopListener () {
//		PluginManager.Instance.OnStartIapEvent += OnProductRequestBegin;
//		PluginManager.Instance.OnEndIapEvent += OnProductRequestEnd;
//
//		PluginManager.Instance.OnPurchaseSuccessEvent += OnPurchaseSuccess;
//
//		PluginManager.Instance.OnRestoreQuerySuccessEvent += OnRestoreQuerySuccess;
	}

	private void RemoveShopListener () {
//		PluginManager.Instance.OnStartIapEvent -= OnProductRequestBegin;
//		PluginManager.Instance.OnEndIapEvent -= OnProductRequestEnd;
//
//		PluginManager.Instance.OnPurchaseSuccessEvent -= OnPurchaseSuccess;
//
//		PluginManager.Instance.OnRestoreQuerySuccessEvent -= OnRestoreQuerySuccess;
	}

	void OnProductRequestBegin () {
		ShowNetLoading ();
	}

	void OnProductRequestEnd () {
		RemoveNetLoading ();
	}

	void OnPurchaseSuccess (string iapKey) {
		UnlockIapItem (iapKey);
		// bool isUnlock = UnlockIapItem (iapKey);
		// if (isUnlock) {
		// 	PluginManager.Instance.internalSDk.popAlertDialog ("Thank you for your purchase.");
		// }
	}

	void OnPurchaseFailed (string iapKey) {
		// PluginManager.Instance.internalSDk.popAlertDialog("Purchase failed.");
		Close ();
	}

	void OnPurchaseCancelled (string iapKey) {
		// PluginManager.Instance.internalSDk.popAlertDialog("Your purchase was canceled. No purchase was made and your account was not charged.");
		Close ();
	}

	void OnRestoreFailed (string iapKey) {
		// PluginManager.Instance.internalSDk.popAlertDialog ("We have detected a problem restoring your purchases. No purchases were restored. Please check your device settings and storage and try again later..");
		Close ();
	}

	void OnRestoreCancelled (string iapKey) {
		Close ();
	}

	void OnNoRestore () {
		// PluginManager.Instance.internalSDk.popAlertDialog ("We could not find any previous purchases. No purchases were restored.Please check your device settings and storage and try again later..");
		Close ();
	}

	void OnProductsNotReady () {
		// PluginManager.Instance.internalSDk.popAlertDialog ("Purchase not ready.");
		Close ();
	}

	void OnRestoreQuerySuccess (string[] iapKeys) {
		if (null == iapKeys || iapKeys.Length == 0) {
			// PluginManager.Instance.internalSDk.popAlertDialog ("No purchase record has been found.");
			return;
		}
		bool hasFull = !string.IsNullOrEmpty (GameDataManager.Instance.KEY_FULL);
		for (int i = 0; i < iapKeys.Length; i++) {
			LockManager.IAP_TYPE iapType = LockManager.IAP_TYPE.NONE;
			string iapKey = iapKeys [i];
			if (iapKey.Equals (GameDataManager.Instance.KEY_FULL)) {
				LockManager.Instance.UnlockFull ();
				hasFull = true;
				//				iapType = UnlockManager.IAP_TYPE.FULL_VERSION;
			} else {
				continue;
			}
			if (null != _iapCallback && iapType != LockManager.IAP_TYPE.NONE) {
				_iapCallback (iapType);
			}
		}
		if (hasFull) {
			if (null != _iapCallback) {
				_iapCallback (LockManager.IAP_TYPE.FULL);
			}
		}
		// PluginManager.Instance.internalSDk.popAlertDialog ("Restore successfully.");
		Close ();
	}

	// void OnAdsRewardHandler (string itemName,int amount,bool isSkipped) {
	// 	OnUnlocked ();
	// }

	void OnUnlocked () {
		LockManager.Instance.UnlockItemByVideo (VideoLockKey);
		Close (() => {
			if (null != _videoLockCallback) {
				_videoLockCallback (true);
			}
		});
	}

	private bool UnlockIapItem (string iapKey) {
		LockManager.IAP_TYPE iapType = LockManager.IAP_TYPE.NONE;
		bool isUnlock = true;
		if (iapKey == GameDataManager.Instance.KEY_FULL) {
			LockManager.Instance.UnlockFull ();
			iapType = LockManager.IAP_TYPE.FULL;
		} else {
			isUnlock = false;
		}
		if (null != _iapCallback && iapType != LockManager.IAP_TYPE.NONE) {
			_iapCallback (iapType);
		}
		Close ();
		return isUnlock;
	}

	private void SimulateNetWaiting (CallbackType.CallbackV callback) {
		Sequence seq = DOTween.Sequence ();
		seq.AppendCallback (() => {
			ShowNetLoading ();
		});
		seq.AppendInterval (0.5f);
		seq.AppendCallback (() => {
			RemoveNetLoading ();
		});
		seq.AppendInterval (1f);
		seq.AppendCallback (() => {
			callback ();
		});
	}

	// private bool CheckNetwork () {
	// 	if (PluginManager.netAvailable) {
	// 		return true;
	// 	}
	// 	PluginManager.Instance.internalSDk.popAlertDialog ("Please connect to internet and try again!");
	// 	return false;
	// }

	protected void UnlockFull () {
		if (Application.isEditor) {
			SimulateNetWaiting (() => {
				LockManager.Instance.UnlockFull ();
				UnlockIapItem (GameDataManager.Instance.KEY_FULL);
			});
			return;
		}
		if (LockManager.Instance.IsFullUnlocked ()) {
//			PluginManager.Instance.internalSDk.popAlertDialog ("You've already purchased all items.");
			UnlockIapItem (GameDataManager.Instance.KEY_FULL);
			return;
		}
//		PluginManager.Instance.PurchaseById (GameManager.Instance.KEY_FULL, SkuType.Managed);
	}

	protected void Restore () {
		if (Application.isEditor) {
			SimulateNetWaiting (() => {
				string[] keys = { GameDataManager.Instance.KEY_FULL };
				OnRestoreQuerySuccess (keys);
			});
			return;
		}
//		PluginManager.Instance.RestoreAllPurchases ();
	}

	protected void GetItForFree () {
		if (string.IsNullOrEmpty (VideoLockKey)) {
			return;
		}
		if (Application.isEditor) {
			SimulateNetWaiting (() => {
				OnUnlocked ();
			});
			return;
		}
//		PluginManager.Instance.ShowRewardAdOrShowCrossIfRewardNotReady (delegate(string name, int count, bool isSkip) {
//			if (!isSkip) {
//				OnUnlocked ();
//			}
//		});
	}

}
