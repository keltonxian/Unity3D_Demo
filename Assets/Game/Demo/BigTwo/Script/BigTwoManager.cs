using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BigTwoManager : GameSceneKit {

	[Space]
	public BigTwoUI _uiController = null;
	public BigTwoLogic _logicController = null;

	protected override void Init () {
		_uiController.Init ();
		_uiController.SetUIBtnShow (false);
		_logicController.Init ();

		_logicController.ReadyGame (() => {
		});
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
