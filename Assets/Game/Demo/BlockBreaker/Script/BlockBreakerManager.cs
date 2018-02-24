using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BlockBreakerManager : GameSceneKit {

	[Space]
	public BlockBreakerUI _uiController = null;
	public BlockBreakerLogic _logicController = null;

	protected override void Init () {
		_uiController.Init ();
		_uiController.SetUIBtnShow (false);
		_logicController.Init ();
	}

	protected override void OnLoadingRemoved () {
	}

	protected override void StartIntroStory () {
	}

	private void StartGame () {
	}

	protected override void ToNextScene () {
	}

}
