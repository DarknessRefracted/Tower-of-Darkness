using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour {

	public static float playerScore;
	GUIStyle style = new GUIStyle();
	Vector3 previousPosition;
	LockPickingGenerator scrLockPickGen;

	void Start() {
		playerScore = 0;
		scrLockPickGen = GameObject.FindGameObjectWithTag ("Tower").GetComponent<LockPickingGenerator> ();
	}

	void Update () 
	{
		if(transform.position.y > previousPosition.y)
			playerScore += Time.deltaTime;
		previousPosition = transform.position;
	}
	
	public void IncreaseScore(int increaseBy)
	{
		playerScore += increaseBy;
	}
	
	void OnDisable()
	{
		PlayerPrefs.SetInt("Score",(int)(playerScore * 100));
	}
	
	void OnGUI()
	{
		style.fontSize = 30;
		style.normal.textColor = Color.white;
		style.font = (Font)Resources.Load("Something Strange");
		GUI.Label (new Rect (20, 20, 100, 30), "Score: " + (int)(playerScore * 100),style);
		style.fontSize = 20;
		GUI.Label (new Rect (20, 100, 100, 30), "Durability: " + (int)(scrLockPickGen.durability),style);
	}

	public static int getScore(){
		return (int)(playerScore * 100);
	}

	public void setScore(int amount){
		playerScore = amount;
	}
}