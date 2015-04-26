using UnityEngine;
using System.Collections;

public class MazeSolver : MonoBehaviour {
	public bool playerFinished;
	public bool cpuFinished;

	// Use this for initialization
	void Start () {
		//Start the computer solver. If this slow-going solver beats the player, the player loses
		//ComputerSolveMaze ();
	}
	
	// Update is called once per frame
	void Update () {
	}


	//Function will execute an algorithm to slowly solve the maze
	void ComputerSolveMaze(){

		cpuFinished = true;
	}
}
