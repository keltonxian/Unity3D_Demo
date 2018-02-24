using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PopupViewKit : BaseSceneKit {

	public GameObject _popup;

	public GameObject _particleShow;
	public AudioClip _soundShow;

	public GameObject _particlePopupDone;
	public AudioClip _soundPopupDone;

	private CallbackType.CallbackB _closeCallback;
	public CallbackType.CallbackB CloseCallback {
		get {
			return _closeCallback;
		}
		set {
			_closeCallback = value;
		}
	}

	private bool _isDone = false;
	public bool IsDone {
		get {
			return _isDone;
		}
		set {
			_isDone = value;
		}
	}

	protected virtual void Awake () {
		_popup.SetActive (false);
		gameObject.GetComponent<Canvas> ().worldCamera = Camera.main;
	}

	protected virtual void Start () {
		StartPreload ();
	}

	protected override void PreLoadComplete () {
		_popup.SetActive (true);
//		AudioManager.Instance.PlaySoundEffect (GameResPath.SFX_POPUP_OPEN);
		PlayParticle (_particleShow);
		PlaySound (_soundShow);
		_popup.transform.DOScale (new Vector3 (0f, 0f, 1f), 0.5f).From ().SetEase (Ease.OutBack).OnComplete (() => {
			Init ();
		});
	}

	protected virtual void Init () {
	}

	protected void Close (CallbackType.CallbackV _closeCallbackV = null) {
		ActionPreClose ();
		this.enabled = false;
//		AudioManager.Instance.PlaySoundEffect (GameResPath.SFX_POPUP_CLOSE);
		Sequence sequence = DOTween.Sequence();
		sequence.AppendCallback (() => {
			if (IsDone) {
				PlayParticle (_particlePopupDone);
				PlaySound (_soundPopupDone);
			}
		});
		sequence.Append (_popup.transform.DOScale (new Vector3 (0f, 0f, 1f), 0.5f).SetEase (Ease.InBack));
		sequence.OnComplete (() => {
			ActionInClose ();
			if (null != _closeCallbackV) {
				_closeCallbackV ();
			}
			if (null != CloseCallback) {
				CloseCallback (IsDone);
			}
			_preloadManager.RemovePreloadCached ();
			Destroy (gameObject);
		});
	}

	protected virtual void ActionPreClose () {
	}

	protected virtual void ActionInClose () {
	}

	protected void PlayParticle (GameObject particle) {
		if (null == particle) {
			return;
		}
		if (particle.GetComponent<ParticleSystem> ()) {
			particle.GetComponent<ParticleSystem> ().Play ();
			return;
		}
//		if (particle.GetComponent<Particle2DUGUI> ()) {
//			particle.GetComponent<Particle2DUGUI> ().Play ();
//			return;
//		}
	}

	protected void PlaySound (AudioClip sound) {
		if (null == sound) {
			return;
		}
//		AudioManager.Instance.PlaySoundEffect (sound);
	}

	public void OnClickClose () {
		Close (() => {
			ActionInOnClickClose ();
		});
	}

	protected virtual void ActionInOnClickClose () {
	}

}
