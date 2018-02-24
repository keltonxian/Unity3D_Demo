using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
//using Spine.Unity;

public class ButtonManager : MonoBehaviour {

//	public RectTransform _btnMenu = null;
	public RectTransform _btnMute = null;
	public RectTransform _btnFacebook = null;
	public RectTransform _btnPrivacyPolicy = null;
	public RectTransform _btnHome = null;
	public RectTransform _btnShop = null;
	public RectTransform _btnRateUs = null;
	public RectTransform _btnMap = null;

	void Start () {
		AddToggleListener (_btnMute, MuteOnClick, (bool isDone) => {
//			if (isDone && AudioManager.Instance.volume == 0) {
//				_btnMute.transform.GetComponent<Toggle> ().isOn = true;
//			}
		});

		AddButtonListener (_btnFacebook, FacebookOnClick);
		AddButtonListener (_btnPrivacyPolicy, PrivacyPolicyOnClick);
		AddButtonListener (_btnMap, MapOnClick);
		AddButtonListener (_btnHome, HomeOnClick);
		AddButtonListener (_btnShop, ShopOnClick);
		AddButtonListener (_btnRateUs, RateUsOnClick);
	}

	private void AddButtonListener (RectTransform btn, UnityAction clickAction, CallbackType.CallbackB callback = null) {
		bool isDone = false;
		if (null != btn && null != btn.transform.GetComponent<KTButton> ()) {
			btn.transform.GetComponent<KTButton> ().OnAnimDone.AddListener (clickAction);
			isDone = true;
		}
		if (null != callback) {
			callback (isDone);
		}
	}

	private void AddToggleListener (RectTransform btn, UnityAction<bool> clickAction, CallbackType.CallbackB callback = null) {
		bool isDone = false;
		if (null != btn && null != btn.transform.GetComponent<KTButton> ()) {
			btn.transform.GetComponent<Toggle> ().onValueChanged.AddListener (clickAction);
		}
		if (null != callback) {
			callback (isDone);
		}
	}

	public void MuteOnClick (bool isMute) {
//		AudioManager.Instance.volume = isMute ? 0f : 1f;
	}

	public void FacebookOnClick () {
//		Application.OpenURL(GameManager.Instance.URL_FACEBOOK);
	}

	public void PrivacyPolicyOnClick () {
//		Application.OpenURL(GameManager.Instance.URL_PRIVACYPOLICY);
	}

	public void RateUsOnClick () {
//		Instantiate (Resources.Load<RateUsController> ("RateUsCanvas"));
	}

	public void ShopOnClick () {
		this.GetComponent<GameUIKit> ().ShowShop ();
	}

	public void MapOnClick () {
//		Instantiate (Resources.Load<TipToMapController> ("TipToMapCanvas")).GetComponent<TipToMapController> ();
	}

	public void HomeOnClick () {
//		Loading loading = (Loading)Instantiate (Resources.Load<Loading> ("LoadingCustom"));
//		loading._nextSceneName = "SceneHome";
	}

}
