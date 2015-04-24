using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {
	
	public void ChangeToScene (string sceneToChangeTo) {
		Application.LoadLevel (sceneToChangeTo);
	}

	public void ExitGame(){
		Application.Quit();
	}
}
