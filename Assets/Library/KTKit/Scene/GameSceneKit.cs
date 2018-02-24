using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
//using Spine.Unity;

public abstract class GameSceneKit : BaseSceneKit {

	[Space]
	public AudioClip _bgMusic = null;
	public AudioClip _onEnterEffect = null;
//	public LockManager.SCENE_TYPE _sceneType;

	protected abstract void Init ();
	protected abstract void OnLoadingRemoved ();

	public override void ActionBeforePreload () {
		base.ActionBeforePreload ();
	}

	protected virtual void Start() {
		StartPreload ();
	}

	protected override void PreLoadComplete() {
		base.PreLoadComplete ();
		Init ();
		RemoveLoading ();
	}

	protected void RemoveLoading() {
//		GameManager.Instance.LastSceneType = _sceneType;
//		if (LaunchLogic._current) {
//			LaunchLogic._current.OnRemoved += delegate(LaunchLogic loading) {
//				OnLoadingRemoved();
//				PlayBackgroundMusic ();
//			};
//			if (LaunchLogic._current.IsLoadingProgressEnded) {
//				LaunchLogic._current.Remove ();
//			} else {
//				LaunchLogic._current.OnLoadEnd += delegate(LaunchLogic loading) {
//					LaunchLogic._current.Remove ();
//				};
//			}
//		} else if (Loading._current) {
//			Loading._current.OnRemoved += delegate(Loading loading) {
//				OnLoadingRemoved();
//				PlayBackgroundMusic ();
//			};
//			if (Loading._current.IsLoadingProgressEnded) {
//				Loading._current.Remove ();
//			} else {
//				Loading._current.OnLoadEnd += delegate(Loading loading) {
//					Loading._current.Remove ();
//				};
//			}
//		} else {
			OnLoadingRemoved ();
//			PlayBackgroundMusic ();
//		}
	}

	public void PlayBackgroundMusic () {
//		if (_bgMusic && false == GameManager.DebugMute) {
//			AudioManager.Instance.PlayBgMusic (_bgMusic);
//		}
	}

	public IEnumerator PlayOnEnterEffectByDelay (float delay) {
		yield return new WaitForSeconds (delay);
		if (_onEnterEffect) {
//			AudioManager.Instance.PlaySoundEffect (_onEnterEffect);
		}
	}

	public void EnterModule (string sceneName, bool isShowAd) {
//		if (true == isShowAd) {
//			PluginManager.Instance.OnAdsShowHandler += AdsShowHandler;
//			PluginManager.Instance.OnAdsRemoveHandler += AdsRemoveHandler;
//		}
//		ModuleManager.Instance.EnterModule (sceneName, isShowAd);
	}

//	protected virtual void AdsShowHandler (AdType obj) {
//		if (obj == AdType.AdTypeInterstitialAds) {
//			PluginManager.Instance.OnAdsShowHandler -= AdsShowHandler;
//			Time.timeScale = 0f;
//			return;
//		}
//	}
//
//	protected virtual void AdsRemoveHandler (AdType obj) {
//		if (obj == AdType.AdTypeInterstitialAds) {
//			PluginManager.Instance.OnAdsRemoveHandler -= AdsRemoveHandler;
//			Time.timeScale = 1f;
//			return;
//		}
//	}

	public void GoNextScene () {
//		PluginManager.Instance.OnAdsShowHandler -= AdsShowHandler;
//		PluginManager.Instance.OnAdsRemoveHandler -= AdsRemoveHandler;

		ToNextScene ();
	}

	protected virtual void ToNextScene () {
	}

	private IEnumerator DelayIntroStory () {
		yield return new WaitForEndOfFrame ();
		StartIntroStory ();
	}

	protected virtual void StartIntroStory () {
	}

	protected void ShowIntroStory () {
		StartCoroutine (DelayIntroStory ());
	}

}
