using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour {

	public static float playerScore;
	GUIStyle style = new GUIStyle();
	Vector3 previousPosition;

	void Start() {
		playerScore = 0;
	}

	void Update () 
	{
		if(transform.position.y > previousPosition.y)
			playerScore += Time.deltaTime;
		previousPosition = transform.position;
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
		GUI.Label (new Rect (20, 20, 100, 30), "Score: " + (int)(playerScore * 100),style);
	}

	public static int getScore(){
		return (int)(playerScore * 100);
	}
}