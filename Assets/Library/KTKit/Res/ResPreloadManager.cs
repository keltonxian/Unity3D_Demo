using System.Collections;
using UnityEngine;

public class ResPreloadManager : MonoBehaviour {

	public ResPreloadKit[] _arrayResPreloadKit;
	private int _loadIndex;
	private int _loadTotal;

	public void StartPreload (CallbackType.CallbackV callback = null) {
		if (null != _arrayResPreloadKit && _arrayResPreloadKit.Length > 0) {
			_loadTotal = _arrayResPreloadKit.Length;
			foreach (ResPreloadKit kit in _arrayResPreloadKit) {
				kit.DoPreloadRes ((bool isLoaded) => {
					_loadIndex++;
					if (_loadIndex == _loadTotal) {
						if (null != callback) {
							callback ();
						}
					}
				});
			}
		} else {
			if (null != callback) {
				callback ();
			}
		}
	}

	public void RemovePreloadCached () {
		if (null != _arrayResPreloadKit && _arrayResPreloadKit.Length > 0) {
			_loadTotal = _arrayResPreloadKit.Length;
			foreach (ResPreloadKit kit in _arrayResPreloadKit) {
				if (null != kit) {
					kit.RemoveCachedRes ();
				}
			}
		}
	}

}
