using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ResPreloadManager))]
public class Loading: MonoBehaviour {

	protected ResPreloadManager _preloadManager;

	public static Loading _current = null;
	public static string _prevSceneName = null;

	public Canvas _canvas = null;
	public Image _block = null;
	public GameObject _content = null;
	public Image _progress = null;
	public Image[] _dots = null;
	public int _dotIndex = 0;
	private float _timeCount = 0f;
	public float _delay = 0f;
	public float _tweenTime = 0.5f;
	public string _nextSceneName = null;
	public event System.Action<Loading> OnLoadStart = null;
	public event System.Action<Loading> OnLoading = null;
	public event System.Action<Loading> OnLoadEnd = null;
	public event System.Action<Loading> OnRemoved = null;
	public bool _isMuteOnLoading = true;

	private AsyncOperation _asyncOperation = null;
	private float _progressValue = 0f;
//	private float _bgMusicVolume = 0f;
//	private float _volume = 0f;
	private Image _canvasBlock = null;
	private bool _isLoadedNextScene = false;
	private bool _isLoadingProgressEnded = false;

	public bool _isShowBlockAnim = true;

	public bool IsLoadingProgressEnded {
		get { return _isLoadingProgressEnded; }
	}

	void Awake() {
		DontDestroyOnLoad (gameObject);
		_preloadManager = this.GetComponent<ResPreloadManager> ();
		Vector3 pos = transform.position;
		pos.z = Camera.main.transform.position.z + Camera.main.nearClipPlane - 100;
		transform.position = pos;
		_current = this;
		InputUtil._isOnUI = true;
	}

	// Use this for initialization
	IEnumerator Start () {
		_canvas.worldCamera = Camera.main;
		transform.position = new Vector3 (
			0f, 0f, 
			Camera.main.nearClipPlane + 1f + Camera.main.transform.position.z
		);

		if (!string.IsNullOrEmpty(_nextSceneName)) {
			if (_progress) {
				_progress.fillAmount = 0f;
			}
		}
		_content.SetActive (false);
		_block.color = new Color (1f, 1f, 1f, 0f);

		AddCanvasBlock ();

//		AudioManager.Instance.StopAll (false);
//		if (true == _isMuteOnLoading) {
//			_bgMusicVolume = AudioManager.Instance.bgMusicVolume;
//			if (_bgMusicVolume > 0) {
//				DOTween.To (() => AudioManager.Instance.bgMusicVolume, x => AudioManager.Instance.bgMusicVolume = x, 0f, _tweenTime).OnComplete (() => {
//					_volume = AudioManager.Instance.volume;
//					AudioManager.Instance.volume = 0;
//				});
//			} else {
//				_volume = AudioManager.Instance.volume;
//				AudioManager.Instance.volume = 0;
//			}
//		}

		float toAlpha = 1f;
		if (false == _isShowBlockAnim) {
			toAlpha = 0f;
		}
		yield return _block.DOFade (toAlpha, _tweenTime).WaitForCompletion ();
		_content.SetActive (true);

		ResManager.Instance.DisposeAll(true);

		_preloadManager.StartPreload (() => {
			StartCoroutine (Init ());
		});
	}

	private IEnumerator Init () {
		yield return _block.DOFade (0f, _tweenTime).WaitForCompletion ();
		yield return new WaitForSeconds (_delay);

		if (null != OnLoadStart) {
			OnLoadStart (this);
		}

		if (!string.IsNullOrEmpty (_nextSceneName)) {
			StartCoroutine (LoadNextScene());
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (null != _dots && _dots.Length > 0) {
			_timeCount += Time.deltaTime;
			if (_timeCount > 0.5f) {
				_timeCount = 0f;
				for (int i = 0; i < _dots.Length; i++) {
					Image dot = _dots [i];
					bool isEnabled = false;
					if (i <= _dotIndex) {
						isEnabled = true;
					}
					dot.enabled = isEnabled;
				}
				_dotIndex++;
				if (_dotIndex >= _dots.Length) {
					_dotIndex = 0;
				}
			}
		}
		if (_progress && null != _asyncOperation) {
			_progressValue = _asyncOperation.progress;
			if (_isLoadedNextScene) {
				_progressValue = 1f;
			}
			
			if (_progress.fillAmount < _progressValue) {
				_progress.fillAmount += 0.02f;
				if (_progress.fillAmount >= 1f && _isLoadedNextScene) {
					finish ();
				}
			}
		}
	}

	void AddCanvasBlock() {
		GameObject gameObject = new GameObject("___block");
		_canvasBlock = gameObject.AddComponent<Image>();
		_canvasBlock.color = new Color (1f, 1f, 1f, 0f);
		_canvasBlock.rectTransform.sizeDelta = new Vector2 (768f, 1024);
		gameObject.transform.SetParent (_block.rectTransform.parent);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
	}

	void finish() {
		_asyncOperation = null;
		_isLoadingProgressEnded = true;
		if (null != OnLoadEnd) {
			OnLoadEnd (this);
		}
	}

	void CheckAdShow() {
		if (string.IsNullOrEmpty (_nextSceneName)) {
			Remove ();
		} else {
			StartCoroutine (LoadNextScene ());
		}
	}

	public void Remove() {
		StartCoroutine (RemoveAnim ());
	}

	IEnumerator RemoveAnim() {
		float toAlpha = 1f;
		if (false == _isShowBlockAnim) {
			toAlpha = 0f;
		}
		yield return _block.DOFade (toAlpha, _tweenTime).WaitForCompletion ();
		_content.SetActive (false);
		yield return _block.DOFade (0f, _tweenTime).WaitForCompletion ();

		if (true == _isMuteOnLoading) {
//			if (AudioManager.Instance.bgMusicVolume != _bgMusicVolume) {
//				DOTween.To (() => AudioManager.Instance.bgMusicVolume, x => AudioManager.Instance.bgMusicVolume = x, _bgMusicVolume, _tweenTime);
//			}
//			AudioManager.Instance.volume = _volume;
		}

		Loading._current = null;
		if (null != OnRemoved) {
			OnRemoved (this);
		}

		_preloadManager.RemovePreloadCached ();

		Destroy (gameObject);
		InputUtil._isOnUI = false;
		if (_canvasBlock) {
			Destroy (_canvasBlock.gameObject);
		}
	}

	IEnumerator LoadNextScene() {
		_prevSceneName = SceneManager.GetActiveScene ().name;
		_isLoadedNextScene = false;

		if (null != OnLoading) {
			OnLoading (this);
		}
		_asyncOperation = SceneManager.LoadSceneAsync (_nextSceneName, LoadSceneMode.Single);
		while (!_asyncOperation.isDone) {
			yield return 0;
		}

		_canvas.worldCamera = Camera.main;
		transform.position = new Vector3 (
			0f, 0f, 
			Camera.main.nearClipPlane + 1f + Camera.main.transform.position.z
		);
		AddCanvasBlock ();

		Resources.UnloadUnusedAssets ();

		yield return new WaitForEndOfFrame ();
		_isLoadedNextScene = true;

		if (null == _progress) {
			finish ();
		}
	}
}

