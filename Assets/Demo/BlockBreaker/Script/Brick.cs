using System.Collections;
using UnityEngine;

public class Brick : MonoBehaviour {

	public static int _breakableCount = 0;

	public AudioClip _crack;
	public Sprite[] _arrayHitSprite;
	public GameObject _smoke;

	private int _timesHit;
	private LevelManager _levelManager;
	private bool _isBreakable;

	void Start () {
		_isBreakable = (this.tag == "Breakable");
		if (_isBreakable) {
			_breakableCount++;
		}
		_timesHit = 0;
		_levelManager = GameObject.FindObjectOfType<LevelManager> ();
	}
	
	void Update () {
	}

	void OnCollisionEnter2D (Collision2D col) {
		AudioSource.PlayClipAtPoint (_crack, transform.position, 0.8f);
		if (_isBreakable) {
			HandleHits ();
		}
	}

	void HandleHits () {
		_timesHit++;
		int maxHits = _arrayHitSprite.Length + 1;
		if (_timesHit >= maxHits) {
			_breakableCount--;
			_levelManager.BrickDestroyed ();
			PuffSmoke ();
			Destroy (gameObject);
		} else {
			LoadSprites ();
		}
	}

	void PuffSmoke () {
		GameObject smokePuff = Instantiate (_smoke, transform.position, Quaternion.identity) as GameObject;
		ParticleSystem.MainModule particle = smokePuff.GetComponent<ParticleSystem> ().main;
		particle.startColor = gameObject.GetComponent<SpriteRenderer> ().color;
	}

	void LoadSprites () {
		int spriteIndex = _timesHit - 1;
		if (null != _arrayHitSprite [spriteIndex]) {
			this.GetComponent<SpriteRenderer> ().sprite = _arrayHitSprite [spriteIndex];
		} else {
			Debug.LogError ("Brick sprite mission!");
		}
	}

}
