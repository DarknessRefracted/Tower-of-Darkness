using UnityEngine;
using System.Collections;

public class LockPickingGenerator : MonoBehaviour {
	
	//Interface object prefabs (with sprites)
	public GameObject walledBackground;
	public GameObject wallBlock;
	public GameObject lockPin;
	//
	public GameObject objBackground;
	public GameObject[] objLockPins;

	//Difficulty level
	public int difficulty;

	//LevelGenerator object in order to access height-related values/functions for placing the interface onto the screen
	public LevelGenerator scrLevGen;
	//Camera script
	public SmoothFollow scrCamera;

	public bool currentlyDisplayingInterface;

	// Use this for initialization
	void Start () {
		currentlyDisplayingInterface = false;
		difficulty = 0;
		scrLevGen = (LevelGenerator) GetComponent ("LevelGenerator");
		scrCamera = (SmoothFollow)GameObject.FindWithTag ("MainCamera").GetComponent ("SmoothFollow");
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	//Function will display the lock picking interface
	public void GenerateDisplay(){
		//Display the interface
		//Display the background and outer bounds of the interface
		objBackground = (GameObject) Instantiate (walledBackground, 
		                                          new Vector3(scrLevGen.start.transform.position.x + 11.0f,
		            scrLevGen.player.transform.position.y - 0.5f,
		            scrLevGen.start.transform.position.z),
		                                          Quaternion.identity);

		//Make the array of lock pin objects
		objLockPins = new GameObject[5 + (int)(difficulty * 1.5)];

		//Display the pins
		for(int i=0; i<5 + (int)(difficulty * 1.5); ++i){
			objLockPins[i] = (GameObject) Instantiate(lockPin,
			                                          new Vector3(objBackground.transform.position.x - 4.5f + (i*0.5f),
			            objBackground.transform.position.y,
			            -2f),
			                                          Quaternion.identity);
		}
		
		//Shift the camera's focus to the maze
		scrCamera.CutTo (objBackground);
	}

	public void DeleteDisplay(){
		//First, shift the camera's focus back to the player
		scrCamera.FindPlayer ();

		//Finally, destroy the background of the interface and mark that there is no interface right now
		GameObject.Destroy (objBackground);
		currentlyDisplayingInterface = false;
	}
}
