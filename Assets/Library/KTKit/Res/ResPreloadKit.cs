using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Spine.Unity;
//using DragonBones;

public class ResPreloadKit : MonoBehaviour {

	private enum ResType
	{
		None, 
		Image, SpriteRenderer,
		Spine, DragonBones,
		Bones2D,
	}
	private ResType _resType = ResType.None;

	public ResManager.Asset _asset = null;
	private Texture _backupMainTexture = null;
	private bool _hasReplace = false;
	private bool _hasRemovedCachedRes = false;
	public bool _notRemoveWhenExit = false;

	private void Init () {
		_hasReplace = false;
		if (this.GetComponent<Image> ()) {
			_resType = ResType.Image;
			return;
		}
		if (this.GetComponent<SpriteRenderer> ()) {
			_resType = ResType.SpriteRenderer;
			return;
		}
//		if (this.GetComponent<SkeletonGraphic> () || this.GetComponent<SkeletonAnimation> ()) {
//			_resType = ResType.Spine;
//			return;
//		}
//		if (this.GetComponent<UnityArmatureComponent> ()) {
//			_resType = ResType.DragonBones;
//			return;
//		}
//		if (this.GetComponent<Bones2D.Armature> ()) {
//			_resType = ResType.Bones2D;
//			return;
//		}
	}

	public void DoPreloadRes (CallbackType.CallbackB isPreloadedCallback = null) {
		Init ();
		ResManager.Instance.LoadAsset(_asset, delegate(ResManager.Asset obj) {
			bool isLoaded = true;

			if (_resType == ResType.Image) { 
				SetReplaceImage (obj.sprite);
			} else if (_resType == ResType.SpriteRenderer) {
				SetReplaceSpriteRenderer (obj.sprite);
			} else if (_resType == ResType.Spine) {
				SetReplaceSpine ((Texture)obj.texture, false);
			} else if (_resType == ResType.DragonBones) {
				SetReplaceDragonBones ((Texture)obj.texture, false);
			} else if (_resType == ResType.Bones2D) {
				SetReplaceBones2D ((Texture)obj.texture, false);
			} else {
				isLoaded = false;
				Debug.LogWarning ("ResPreloadKit Load [" + obj.url + "] Fail !");
			}
			if (isLoaded) {
				_hasReplace = true;
				obj.cached = true;
			}
			if (null != isPreloadedCallback) {
				isPreloadedCallback (true);
			}
		}, delegate(ResManager.Asset obj) {
			if (null != isPreloadedCallback) {
				isPreloadedCallback (true);
			}
		});
	}

	private bool SetReplaceImage (Sprite sprite) {
		if (null == sprite) {
			Debug.LogWarning ("ResPreloadKit Image Sprite Null !");
			return false;
		}
		Image target = this.GetComponent<Image> ();
		target.sprite = sprite;
		target.SetNativeSize ();
		return true;
	}

	private bool SetReplaceSpriteRenderer (Sprite sprite) {
		if (null == sprite) {
			Debug.LogWarning ("ResPreloadKit SpriteRenderer Sprite Null !");
			return false;
		}
		SpriteRenderer target = this.GetComponent<SpriteRenderer> ();
		target.sprite = sprite;
//		if (target.GetComponent<RenderTexturePainter> ()) {
//			target.GetComponent<RenderTexturePainter> ().SetSpriteRendererToSourceTexture ();
//		}
		return true;
	}

	private bool SetReplaceSpine (Texture texture, bool isReset) {
		if (null == texture) {
			Debug.LogWarning ("ResPreloadKit Spine texture Null !");
			return false;
		}
		Material target = null;
//		if (null != this.GetComponent<SkeletonGraphic> ()) {
//			target = this.GetComponent<SkeletonGraphic> ().SkeletonDataAsset.atlasAssets [0].materials [0];
//			this.GetComponent<SkeletonGraphic> ().SetAllDirty ();
//		} else if (null != this.GetComponent<SkeletonAnimation> ()) {
//			target = this.GetComponent<SkeletonAnimation> ().SkeletonDataAsset.atlasAssets [0].materials [0];
//		}
		if (null == target) {
			Debug.LogWarning ("ResPreloadKit Spine Materail Null !");
			return false;
		}
		SetMaterialMainTexture (target, texture, isReset);
		return true;
	}

	private bool SetReplaceDragonBones (Texture texture, bool isReset) {
		if (null == texture) {
			Debug.LogWarning ("ResPreloadKit Dragonbones texture Null !");
			return false;
		}
		Material target = null;
//		if (null != this.GetComponent<UnityArmatureComponent> ()) {
//			target = this.GetComponent<UnityArmatureComponent> ().unityData.textureAtlas [0].material;
//		} 
		if (null != target) {
			Debug.LogWarning ("ResPreloadKit Dragonbones Material Null !");
			return false;
		}
		SetMaterialMainTexture (target, texture, isReset);
		return true;
	}

	private bool SetReplaceBones2D (Texture texture, bool isReset) {
		if (null == texture) {
			Debug.LogWarning ("ResPreloadKit Bones2D texture Null !");
			return false;
		}
		Material target = null;
//		if (null != this.GetComponent<Bones2D.Armature> ()) {
//			target = this.GetComponent<Bones2D.Armature> ().textureFrames.materials [0];
//		}
		if (null != target) {
			Debug.LogWarning ("ResPreloadKit Bones2D Material Null !");
			return false;
		}
		SetMaterialMainTexture (target, texture, isReset);
		return true;
	}

	private void SetMaterialMainTexture (Material material, Texture texture, bool isReset) {
		if (!isReset) {
			_backupMainTexture = material.mainTexture;
		}
		material.mainTexture = texture;
		if (isReset) {
			_backupMainTexture = null;
		}
	}

	public void RemoveCachedRes () {
		if (_notRemoveWhenExit) {
			return;
		}
		if (_hasRemovedCachedRes) {
			return;
		}
		_hasRemovedCachedRes = true;
		ResManager.Instance.DisposeAsset (_asset);
	}

	private void ResetBackupTexture () {
		if (!_hasReplace) {
			return;
		}
		if (_resType == ResType.Spine) {
			SetReplaceSpine (_backupMainTexture, true);
		} else if (_resType == ResType.DragonBones) {
			SetReplaceDragonBones (_backupMainTexture, true);
		} else if (_resType == ResType.Bones2D) {
			SetReplaceBones2D (_backupMainTexture, true);
		}
	}

	void OnDestroy () {
		ResetBackupTexture ();
	}

}
