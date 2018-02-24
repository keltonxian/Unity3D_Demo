using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public void LoadLevel (string name) {
		Brick._breakableCount = 0;
		SceneManager.LoadScene (name);
	}

	public void QuitRequest () {
		Application.Quit ();
	}

	public void BrickDestroyed () {
		if (Brick._breakableCount <= 0) {
			LoadLevel ("BlockBreakerStart");
		}
	}

}
