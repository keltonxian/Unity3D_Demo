using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(ButtonManager))]
[RequireComponent(typeof(LockItemHandler))]
public class GameUIKit : MonoBehaviour {

	public enum ButtonMoveInType {
		NONE,
		FROM_BOTTOM_AND_TOP,
	}

	[Header("Touch Block View")]
	public GameObject _blockView;
	[Header("UI Button")]
	public RectTransform[] _arrayLeftTopCornerBtn;
	public RectTransform[] _arrayRightTopCornerBtn;
	public RectTransform[] _arrayLeftBottomCornerBtn;
	public RectTransform[] _arrayRightBottomCornerBtn;
	public ButtonMoveInType _buttonMoveInType = ButtonMoveInType.NONE;
	public float _moveGapTime = 0.2f;
	public bool _isPlayMoveSoundEffect = true;
	protected ButtonManager _buttonManager;
	protected LockItemHandler _lockItemHandler;
//	private bool _hasUnlockScene = false;
	private LockManager.SCENE_TYPE _sceneType;
	private CallbackType.CallbackV _unlockSceneCallback;
	[Header("Button Next")]
	public RectTransform _btnNext;
	private bool _hasShowBtnNext = false;

	public virtual void Init () {
		_buttonManager = this.GetComponent<ButtonManager> ();
		_lockItemHandler = this.GetComponent<LockItemHandler> ();
		if (null != _btnNext) {
			_btnNext.gameObject.SetActive (false);
		}
		CheckHideShop ();
	}

	public void ShowBlockView () {
		if (null == _blockView) {
			return;
		}
		_blockView.SetActive (true);
	}

	public void HideBlockView () {
		if (null == _blockView) {
			return;
		}
		_blockView.SetActive (false);
	}

	public void ShowRateUs () {
		_buttonManager.RateUsOnClick ();
	}

	public void ShowBtnNext () {
		if (_hasShowBtnNext) {
			return;
		}
		_hasShowBtnNext = true;
		_btnNext.DOKill ();
		_btnNext.gameObject.SetActive (true);
		_btnNext.localScale = Vector3.zero;
		Sequence seq = DOTween.Sequence ();
		seq.Append (_btnNext.DOScale (Vector3.one, 0.5f));
		seq.AppendCallback (() => {
			_btnNext.DOScale (new Vector3 (1.2f, 1.2f, 0f), 0.4f).SetLoops (-1, LoopType.Yoyo);
		});
	}

	public void HideBtnNext () {
		if (!_hasShowBtnNext) {
			return;
		}
		_hasShowBtnNext = false;
		_btnNext.DOKill ();
		Sequence seq = DOTween.Sequence ();
		seq.Append (_btnNext.DOScale (Vector3.zero, 0.5f));
		seq.AppendCallback (() => {
			_btnNext.localScale = Vector3.one;
			_btnNext.gameObject.SetActive (false);
		});
	}

	private float SetCornerBtnsShow (RectTransform[] _arrayBtn, bool isShow, Vector2 fromPos, float delay, ButtonMoveInType buttonMoveInType) {
		if (null == _arrayBtn || _arrayBtn.Length == 0) {
			return delay;
		}
		if (!isShow) {
			for (int i = 0; i < _arrayBtn.Length; i++) {
				RectTransform btn = _arrayBtn [i];
				btn.DOKill ();
				btn.gameObject.SetActive (false);
			}
			return delay;
		}
		float time = 0.5f;
		for (int i = 0; i < _arrayBtn.Length; i++) {
			RectTransform btn = _arrayBtn [i];
			btn.gameObject.SetActive (true);
			Vector2 pos = btn.anchoredPosition;
			if (buttonMoveInType == ButtonMoveInType.FROM_BOTTOM_AND_TOP) {
				fromPos.x = pos.x;
			}
			btn.DOKill ();
			btn.DOAnchorPos (fromPos, time).From ().SetEase (Ease.OutBack).SetDelay (delay).OnStart (() => {
				if (_isPlayMoveSoundEffect) {
//					AudioManager.Instance.PlaySoundEffect (GameResPath.SFX_BUTTON_MOVE_IN);
				}
			});
			delay += _moveGapTime;
		}
		return delay;
	}

	public void SetUIBtnShow (bool isShow) {
		Vector2 fromLeftTopPos = Vector2.zero;
		Vector2 fromRightTopPos = Vector2.zero;
		Vector2 fromLeftBottomPos = Vector2.zero;
		Vector2 fromRightBottomPos = Vector2.zero;
		if (_buttonMoveInType == ButtonMoveInType.FROM_BOTTOM_AND_TOP) {
			fromLeftTopPos = new Vector2 (0f, 100f);
			fromRightTopPos = new Vector2 (0f, 100f);
			fromLeftBottomPos = new Vector2 (0f, -100f);
			fromRightBottomPos = new Vector2 (0f, -100f);
		} else {
			fromLeftTopPos = new Vector2 (-100f, 100f);
			fromRightTopPos = new Vector2 (100f, 100f);
			fromLeftBottomPos = new Vector2 (-100f, -100f);
			fromRightBottomPos = new Vector2 (100f, -100f);
		}
		float delay = 0f;
		delay = SetCornerBtnsShow (_arrayLeftTopCornerBtn, isShow, fromLeftTopPos, delay, _buttonMoveInType);
		delay = SetCornerBtnsShow (_arrayRightTopCornerBtn, isShow, fromRightTopPos, delay, _buttonMoveInType);
		delay = SetCornerBtnsShow (_arrayLeftBottomCornerBtn, isShow, fromLeftBottomPos, delay, _buttonMoveInType);
		delay = SetCornerBtnsShow (_arrayRightBottomCornerBtn, isShow, fromRightBottomPos, delay, _buttonMoveInType);
	}

	public virtual void CheckHideShop (GameObject[] arrayLockItem = null) {
//		if (LockManager.Instance.IsAllUnlock()) {
//			if (null != _buttonManager._btnShop) {
//				_buttonManager._btnShop.gameObject.SetActive (false);
//			}
//		}
	}

//	protected void ShowMiniShop (LockManager.IAP_TYPE shopType, string videoLockKey, ShopController.IapCallbackDelegate iapCallback=null, CallbackType.CallbackB videoLockCallback=null, CallbackType.CallbackV closeCallback = null) {
//		ShopController shop = (ShopController)Instantiate (Resources.Load<ShopController> (ShopController._shopMiniPrefabName));
//		shop.MiniShopType = shopType;
//		shop.VideoLockKey = videoLockKey;
//		shop.IapCallback = iapCallback;
//		shop.VideoLockCallback = videoLockCallback;
//		shop.ShopCloseCallback = closeCallback;
//	}
//
//	protected void ShowBaseShop (ShopController.IapCallbackDelegate iapCallback=null, CallbackType.CallbackB videoLockCallback=null) {
//		ShopController shop = (ShopController)Instantiate (Resources.Load<ShopController> (ShopController._shopFullPrefabName));
//		shop.IapCallback = iapCallback;
//		shop.VideoLockCallback = videoLockCallback;
//	}

	public void ShowShop () {
//		ShowBaseShop ((LockManager.IAP_TYPE iapType) => {
//			_lockItemHandler.IapUnlock (iapType, (GameObject[] arrayLockItem)=>{
//				CheckHideShop (arrayLockItem);
//				ShowIapUnlockAction (arrayLockItem);
//			});
//		});
	}

	protected virtual void ShowIapUnlockAction (GameObject[] arrayLockItem) {
	}

//	private bool IsSceneLock (LockManager.SCENE_TYPE sceneType) {
//		if (sceneType == LockManager.SCENE_TYPE.NONE) {
//			return false;
//		}
//		if (sceneType == LockManager.SCENE_TYPE.LEVEL_1) {
//			return false;
//		}
//		if (LockManager.Instance.IsLevelsItemUnlocked (LockManager.Instance.LockSceneKey (_sceneType))) {
//			return false;
//		}
//		return true;
//	}

//	public void CheckSceneLock (LockManager.SCENE_TYPE sceneType, CallbackType.CallbackV unlockCallback) {
//		_sceneType = sceneType;
//		_unlockSceneCallback = unlockCallback;
//		RectTransform lockParent = this.GetComponent<RectTransform> ();
//		if (IsSceneLock (_sceneType) && null != lockParent) {
//			_lockItemHandler.ActiveSceneLock (lockParent, () => {
//				UnlockScene ();
//			});
//			RectTransform lockItem = _lockItemHandler.GetSceneLock ().Find ("ItemLockImage").GetComponent<RectTransform> ();
//			lockItem.GetComponent<KTButton> ().enabled = false;
//			lockItem.localScale = Vector3.zero;
//			Sequence seq = DOTween.Sequence ();
//			seq.Append (lockItem.DOScale (Vector3.one, 0.6f).SetEase (Ease.OutBack));
//			seq.AppendInterval (0.7f);
//			seq.AppendCallback (() => {
//				UnlockScene ();
//				lockItem.GetComponent<KTButton> ().enabled = true;
//			});
//			return;
//		}
//		UnlockSceneDone ();
//	}

	private void UnlockActions () {
//		_lockItemHandler.VideoUnlockScene ((GameObject lockItem) => {
//			if (!_hasUnlockScene) {
//				lockItem.SetActive (false);
//				UnlockSceneDone ();
//			}
//		});
	}

	public void UnlockScene () {
//		ShowMiniShop (
//			LockManager.IAP_TYPE.LEVELS,
//			LockManager.Instance.LockSceneKey (_sceneType),
//			(LockManager.IAP_TYPE iapType) => {
//				_lockItemHandler.IapUnlock (iapType, (GameObject[] arrayLockItem) => {
//					CheckHideShop (arrayLockItem);
//					// if (null == arrayLockItem) {
//					// 	return;
//					// }
//					if (iapType == LockManager.IAP_TYPE.LEVELS || iapType == LockManager.IAP_TYPE.FULL) {
//						UnlockActions ();
//					}
//				});
//			},
//			(bool isUnlocked) => {
//				if (false == isUnlocked) { return; };
//				UnlockActions ();
//			},
//			() => {
//				Loading loading = (Loading)Instantiate (Resources.Load<Loading> ("LoadingCustom"));
//				loading._nextSceneName = "SceneMap";
//			}
//		);
	}

	private void UnlockSceneDone () {
//		_hasUnlockScene = true;
//		if (null != _unlockSceneCallback) {
//			_unlockSceneCallback ();
//		}
	}

	public void ShowTargetTip (SpriteRenderer tip, Vector3 fromPos, Vector3 toPos, LoopType loopType = LoopType.Yoyo) {
		if (true == tip.enabled) {
			return;
		}
		tip.transform.DOKill ();
		tip.enabled = true;
		float distance = Mathf.Sqrt (Mathf.Pow ((toPos.x - fromPos.x), 2f) + Mathf.Pow ((toPos.y - fromPos.y), 2f));
		float time = distance / 1f;
		tip.transform.localPosition = fromPos;
		tip.transform.DOLocalMove (toPos, time).SetLoops (-1, loopType);
	}

	public void HideTargetTip (SpriteRenderer tip) {
		if (false == tip.enabled) {
			return;
		}
		tip.transform.DOKill ();
		tip.enabled = false;
	}

	public void ShowTargetTipRT (Image tip, Vector2 fromPos, Vector2 toPos, LoopType loopType = LoopType.Yoyo) {
		if (true == tip.enabled) {
			return;
		}
		tip.GetComponent<RectTransform> ().DOKill ();
		tip.enabled = true;
		float distance = Mathf.Sqrt (Mathf.Pow ((toPos.x - fromPos.x), 2f) + Mathf.Pow ((toPos.y - fromPos.y), 2f));
		float time = distance / 100f;
		tip.GetComponent<RectTransform> ().anchoredPosition = fromPos;
		tip.GetComponent<RectTransform> ().DOAnchorPos (toPos, time).SetLoops (-1, loopType);
	}

	public void HideTargetTipRT (Image tip) {
		if (false == tip.enabled) {
			return;
		}
		tip.GetComponent<RectTransform> ().DOKill ();
		tip.enabled = false;
	}

	public void ShowSimulateBanner () {
		ShowAdBannerRect ();
	}

	public void HideSimulateBanner () {
		RemoveAdBannerRect ();
	}

	private void ShowAdBannerRect () {
		if (!Application.isEditor) {
			return;
		}
//		if (GameManager.NoAds) {
//			return;
//		}
		GameObject canvas = GameObject.Find ("UICanvas");
		if (null == canvas) {
			return;
		}
		GameObject rect = new GameObject ("AdBannerRect");
		Image image = rect.AddComponent<Image> ();
		image.color = new Color (255, 0, 0, 200);
		rect.GetComponent<RectTransform> ().SetParent (canvas.GetComponent<RectTransform> ());
		rect.GetComponent<RectTransform> ().sizeDelta = new Vector2 (768f, 90f);
		rect.GetComponent<RectTransform> ().localPosition = new Vector3 (0f, 45f, 1f);
		rect.GetComponent<RectTransform> ().localScale = new Vector3 (1f, 1f, 1f);
		rect.GetComponent<RectTransform> ().anchorMax = new Vector2 (0.5f, 0f);
		rect.GetComponent<RectTransform> ().anchorMin = new Vector2 (0.5f, 0f);
	}

	private void RemoveAdBannerRect () {
		if (!Application.isEditor) {
			return;
		}
		Canvas canvas = null;
		GameObject topCanvas = GameObject.Find ("UICanvas");
		if (null != topCanvas) {
			canvas = topCanvas.GetComponent<Canvas> ();
		}
		if (null == canvas) {
			return;
		}
		Transform rect = canvas.GetComponent<RectTransform> ().Find ("AdBannerRect");
		if (null == rect) {
			return;
		}
		GameObject.Destroy (rect.gameObject);
	}

}
