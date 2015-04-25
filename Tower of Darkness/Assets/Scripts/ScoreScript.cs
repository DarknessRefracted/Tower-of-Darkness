using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour {

	float playerScore = 0;
	GUIStyle style = new GUIStyle();

	void Update () 
	{
		playerScore += Time.deltaTime;
	}
	
	public void IncreaseScore(int amount)
	{
		playerScore += amount;
	}
	
	void OnDisable()
	{
		PlayerPrefs.SetInt("Score",(int)(playerScore * 100));
	}
	
	void OnGUI()
	{
		style.fontSize = 30;
		style.font = (Font)Resources.Load("Something Strange");
		GUI.Label (new Rect (10, 10, 100, 30), "Score: " + (int)(playerScore * 100),style);
	}
}