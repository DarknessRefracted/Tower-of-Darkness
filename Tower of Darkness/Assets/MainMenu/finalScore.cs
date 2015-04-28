using UnityEngine;
using System.Collections;

public class finalScore : MonoBehaviour {
	
	GUIStyle finalScoreStyle = new GUIStyle();
	int score = ScoreScript.getScore();

	void OnGUI(){
		finalScoreStyle.fontSize = 60;
		finalScoreStyle.normal.textColor = Color.red;
		finalScoreStyle.font = (Font)Resources.Load("Something Strange");

		GUI.Label (new Rect (Screen.width/2-180, Screen.height/2-80, 100, 50), "Final Score: " + score,finalScoreStyle);
	}
}
