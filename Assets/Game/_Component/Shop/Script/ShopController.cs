using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopController : ShopHandler {

	public static string _shopFullPrefabName = "ShopFullCanvas";
	public static string _shopMiniPrefabName = "ShopMiniCanvas";

	public RectTransform _fullPack;
	public RectTransform _characterPack;
	public RectTransform _decorationPack;
	public RectTransform _levelPack;
	public RectTransform _noAds;

	private LockManager.IAP_TYPE _miniShopType = LockManager.IAP_TYPE.NONE;
	public LockManager.IAP_TYPE MiniShopType {
		get {
			return _miniShopType;
		}
		set {
			_miniShopType = value;
			ShowMiniShopPack ();
		}
	}

	protected override void Awake () {
//		if (null != _characterPack && LockManager.Instance.IsCharactersUnlocked ()) {
//			LoopEnableImage (_characterPack, false);
////	 		_characterPack.GetComponent<Image>().raycastTarget = false;
////	 		_characterPack.GetComponent<Image>().color = new Color (0.5f, 0.5f, 0.5f, 1f);
//	 	}
//	 	if (null != _levelPack && LockManager.Instance.IsLevelsUnlocked ()) {
//			LoopEnableImage (_levelPack, false);
////	 		_levelPack.GetComponent<Image>().raycastTarget = false;
////	 		_levelPack.GetComponent<Image>().color = new Color (0.5f, 0.5f, 0.5f, 1f);
//	 	}
//	 	if (null != _decorationPack && LockManager.Instance.IsDecorationsUnlocked ()) {
//			LoopEnableImage (_decorationPack, false);
////	 		_decorationPack.GetComponent<Image>().raycastTarget = false;
////	 		_decorationPack.GetComponent<Image>().color = new Color (0.5f, 0.5f, 0.5f, 1f);
//	 	}
//	 	if (null != _noAds && LockManager.Instance.IsNoAdsUnlocked ()) {
//			LoopEnableImage (_noAds, false);
////	 		_noAds.GetComponent<Image>().raycastTarget = false;
////	 		_noAds.GetComponent<Image>().color = new Color (0.5f, 0.5f, 0.5f, 1f);
//	 	}
		base.Awake ();
	}

	protected override void Init () {
		if (null != _fullPack) {
			_fullPack.transform.DOScale (0.06f, 0.8f).SetRelative ().SetLoops (-1, LoopType.Yoyo);
		}
	}

	private void LoopEnableImage (RectTransform item, bool isEnabled) {
		Image image = item.GetComponent<Image> ();
		if (null != image && null != image.sprite) {
			float color = isEnabled ? 1f : 0.5f;
			image.raycastTarget = isEnabled;
			image.color = new Color (color, color, color, 1f);
		}
		if (0 == item.childCount) {
			return;
		}
		for (int i = 0; i < item.childCount; i++) {
			RectTransform subItem = item.GetChild (i).GetComponent<RectTransform> ();
			LoopEnableImage (subItem, isEnabled);
		}
	}

	private void ShowMiniShopPack () {
//		RectTransform pack = null;
//		if (_miniShopType == LockManager.IAP_TYPE.CHARACTERS) {
//			pack = _characterPack;
//		} else if (_miniShopType == LockManager.IAP_TYPE.DECORATIONS) {
//			pack = _decorationPack;
//		} else if (_miniShopType == LockManager.IAP_TYPE.LEVELS) {
//			pack = _levelPack;
//		} else {
//			return;
//		}
//		pack.gameObject.SetActive (true);
	}

	public void OnClickFullPack () {
		UnlockFull ();
	}

	public void OnClickRestore () {
		Restore ();
	}

	public void OnClickGetItForFree () {
		GetItForFree ();
	}

	public void OnClickGoStore () {
		Close (() => {
			ShopController shop = Instantiate (Resources.Load<ShopController> (_shopFullPrefabName));
			shop.IapCallback = IapCallback;
			shop.VideoLockCallback = VideoLockCallback;
			shop.ShopCloseCallback = ShopCloseCallback;
		});
	}

}
