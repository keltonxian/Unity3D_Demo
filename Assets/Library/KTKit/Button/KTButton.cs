using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

[DisallowMultipleComponent]
public class KTButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {

	public enum EventType {
		TOUCH,
		CLICK,
	}
	public enum TouchEffectType {
		NONE,
		SCALE,
		JELLY,
	}

	public Transform _target;

	[SerializeField]
	protected UnityEvent _onTouchDown = new UnityEvent ();
	public UnityEvent OnTouchDown {
		get {
			return _onTouchDown;
		}
	}
	[SerializeField]
	protected UnityEvent _onAnimDone = new UnityEvent ();
	public UnityEvent OnAnimDone {
		get {
			return _onAnimDone;
		}
	}
	[SerializeField]
	protected UnityEvent _onTouchUp = new UnityEvent ();
	public UnityEvent OnTouchUp {
		get {
			return _onTouchUp;
		}
	}

	public bool _isTouchEnabled = true;

	public TouchEffectType _touchEffectType = TouchEffectType.JELLY;
	public EventType _eventType = EventType.TOUCH;
	public float _scaleTime = 1f;
	private Vector2 _scaleDefault = Vector2.one;
	public float _scaleChange = 0.13f;
	public Image _stateDefaultImage = null;
	public SpriteRenderer _stateDefaultSpriteRenderer = null;
	private Sprite _stateDefaultSprite = null;
	public Sprite _stateSelectedSprite = null;

	public AudioClip _clickSound = null;
	public string _clickSoundPath = null;
	public AudioClip _clickVO = null;
//	public Particle2DUGUI _particle;

	public float _clickInterval = 1f;
	private float _timeWaitNextClick = 0f;

	private Sequence _animSeq = null;

	void Awake () {
		SetScaleDefault (this.transform.localScale.x, this.transform.localScale.y);
	}

	// Use this for initialization
	void Start () {
		if (!_target) {
			_target = this.transform;
		}

		OnStart ();
	}

	// Update is called once per frame
	void Update () {
		if (_timeWaitNextClick > 0f) {
			_timeWaitNextClick -= Time.deltaTime;
		}
	}

	private bool CheckCanClickByInterval () {
		if (_timeWaitNextClick <= 0f) {
			return true;
		}
		return false;
	}

	public void SetScaleDefault (float x, float y) {
		_scaleDefault = new Vector2 (x, y);
	}

	public bool IsLock () {
//		LockItemKit lockitemKit = this.GetComponent<LockItemKit> ();
//		if (null == lockitemKit || !lockitemKit.IsLock ()) {
			return false;
//		}
//		return true;
	}

	public bool IsTouchEnabled () {
		return _isTouchEnabled;
	}

	protected virtual void OnStart () {
//		transform.GetComponent<Button> ().onClick.AddListener (OnClick);
	}

	public virtual void PlayClickSound () {
//		AudioManager am = AudioManager.Instance;
//		if (null != _clickSound) {
//			am.PlaySoundEffect (_clickSound);
//		} else if (!string.IsNullOrEmpty (_clickSoundPath)) {
//			am.PlaySoundEffect (_clickSoundPath);
//		} else {
//			am.PlaySoundEffect (CommonUtility.SFX_BUTTON_CLICK);
//		}
	}

	public virtual void PlayClickVO () {
		if (null == _clickVO) {
			return;
		}
//		AudioManager.Instance.PlaySoundEffect (_clickVO);
	}

	public virtual void TouchDown (PointerEventData eventData) {
	}

	public virtual void TouchUp (PointerEventData eventData) {
	}

	public virtual void TouchClick (PointerEventData eventData) {
	}

	public void OnPointerDown (PointerEventData eventData) {
		if (_eventType != EventType.TOUCH) {
			return;
		}
		if (!IsTouchEnabled ()) {
			return;
		}
		if (!CheckCanClickByInterval ()) {
			return;
		}
		_timeWaitNextClick = _clickInterval;
		if (null != _stateSelectedSprite) {
			if (null != _stateDefaultImage) {
				_stateDefaultSprite = _stateDefaultImage.sprite;
				_stateDefaultImage.sprite = _stateSelectedSprite;
			} else if (null != _stateDefaultSpriteRenderer) {
				_stateDefaultSprite = _stateDefaultSpriteRenderer.sprite;
				_stateDefaultSpriteRenderer.sprite = _stateSelectedSprite;
			}
		}
		TouchDown (eventData);

		OnTouchDown.Invoke();
		PlayClickSound ();
		PlayClickVO ();
		PlayTouchAnim (() => {
			OnAnimDone.Invoke ();
		});
	}

	public void OnPointerUp (PointerEventData eventData) {
		if (_eventType != EventType.TOUCH) {
			return;
		}
		if (!IsTouchEnabled ()) {
			return;
		}
		if (null != _stateSelectedSprite) {
			if (null != _stateDefaultImage) {
				_stateDefaultImage.sprite = _stateDefaultSprite;
			} else if (null != _stateDefaultSpriteRenderer) {
				_stateDefaultSpriteRenderer.sprite = _stateDefaultSprite;
			}
		}
		TouchUp (eventData);
		OnTouchUp.Invoke ();
	}

	public void OnPointerClick (PointerEventData eventData) {
		if (_eventType != EventType.CLICK) {
			return;
		}
		if (!IsTouchEnabled ()) {
			return;
		}
		if (!CheckCanClickByInterval ()) {
			return;
		}
		_timeWaitNextClick = _clickInterval;
		PlayClickSound ();
		PlayTouchAnim (() => {
			TouchClick (eventData);
			OnAnimDone.Invoke ();
		});
		OnTouchUp.Invoke ();
	}

	public void StopTouchAnim () {
		if (null != _animSeq) {
			_animSeq.Kill ();
			_animSeq = null;
		}
		if (null != _target) {
			_target.DOKill ();
			_target.localScale = new Vector3 (_scaleDefault.x, _scaleDefault.y, 1f);
		}
	}

	public void PlayTouchAnim (CallbackType.CallbackV callback) {
		StopTouchAnim ();
//		if (null != _particle) {
//			_particle.Play ();
//		}
		Sequence seq = DOTween.Sequence ();
		_animSeq = seq;
		if (_touchEffectType == TouchEffectType.SCALE) {
			float time = _scaleTime / 2f;
			seq.Append (_target.DOScale (new Vector3 (_scaleDefault.x + _scaleChange, _scaleDefault.y + _scaleChange, 1f), time));
			seq.Append (_target.DOScale (new Vector3 (_scaleDefault.x, _scaleDefault.y, 1f), time));
		} else if (_touchEffectType == TouchEffectType.JELLY) {
			float time = _scaleTime / 4f;
			if (_scaleDefault.x * _scaleDefault.y > 0) {
				seq.Append (_target.DOScale (new Vector3 (_scaleDefault.x + _scaleChange, _scaleDefault.y - _scaleChange, 1f), time));
				seq.Append (_target.DOScale (new Vector3 (_scaleDefault.x - _scaleChange, _scaleDefault.y + _scaleChange, 1f), time));
			} else {
				seq.Append (_target.DOScale (new Vector3 (_scaleDefault.x - _scaleChange, _scaleDefault.y - _scaleChange, 1f), time));
				seq.Append (_target.DOScale (new Vector3 (_scaleDefault.x + _scaleChange, _scaleDefault.y + _scaleChange, 1f), time));
			}
			seq.Append (_target.DOScale (new Vector3 (_scaleDefault.x, _scaleDefault.y, 1f), time));
		}
		seq.AppendCallback (() => {
			callback ();
		});
		seq.Play ();
	}

}

