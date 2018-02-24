using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ResPreloadManager))]
public class BaseSceneKit : MonoBehaviour {

	protected ResPreloadManager _preloadManager;

	public virtual void ActionBeforePreload () {
	}

	public virtual void StartPreload () {
		_preloadManager = this.GetComponent<ResPreloadManager> ();
		ActionBeforePreload ();
		_preloadManager.StartPreload (() => {
			PreLoadComplete ();
		});
	}

	protected virtual void PreLoadComplete () {
	}

}
