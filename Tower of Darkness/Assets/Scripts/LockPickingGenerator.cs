using UnityEngine;
using System.Collections;

public class LockPickingGenerator : MonoBehaviour {
	
	//Interface object prefabs (with sprites)
	public GameObject walledBackground;
	public GameObject wallBlock;
	public GameObject lockPin;
	public GameObject lockPinHole;
	//
	public GameObject objBackground;
	public GameObject[] objLockPins;
	public GameObject[] objLockPinHoles;

	//Difficulty level
	public int difficulty;

	//Durability of the lock pick. This forces the player to be at least somewhat conservative about pick attempts
		//Durability is lowered in the IsPinSolved() function, and in the pin movement section of the FixedUpdate()
			// funtion of the CharController.cs script
	public int durability;

	//LevelGenerator object in order to access height-related values/functions for placing the interface onto the screen
	public LevelGenerator scrLevGen;
	//Camera script
	public SmoothFollow scrCamera;

	public bool currentlyDisplayingInterface;
	public bool lockSolved;

	// Use this for initialization
	void Start () {
		currentlyDisplayingInterface = false;
		lockSolved = false;
		difficulty = 0;
		scrLevGen = (LevelGenerator) GetComponent ("LevelGenerator");
		scrCamera = (SmoothFollow)GameObject.FindWithTag ("MainCamera").GetComponent ("SmoothFollow");
	}


	//Function will display the lock picking interface
	public void GenerateDisplay(){
		//Set the durability of the lock pick
		durability = 500 + 400 * difficulty;
		
		//Display the background and outer bounds of the interface
		objBackground = (GameObject) Instantiate (walledBackground, 
		                                          new Vector3(scrLevGen.start.transform.position.x + 11.0f,
		            scrLevGen.player.transform.position.y - 0.5f,
		            scrLevGen.start.transform.position.z),
		                                          Quaternion.identity);

		//Make the array of lock pin objects
		objLockPins = new GameObject[5 + (int)(difficulty * 3.5)];
		objLockPinHoles = new GameObject[5 + (int)(difficulty * 3.5)];

		//Display the pins and the pin holes
		for(int i=0; i<5 + (int)(difficulty * 3.5); ++i){
			objLockPins[i] = (GameObject) Instantiate(lockPin,
			                                        new Vector3(objBackground.transform.position.x - 4.25f + (i*0.5f),
			            							objBackground.transform.position.y, -2f),
			                                        Quaternion.identity);

			objLockPinHoles[i] = (GameObject) Instantiate(lockPinHole,
			                                        new Vector3(objBackground.transform.position.x - 4.25f + (i*0.5f),
			            							objBackground.transform.position.y + Random.Range(.125f, 1f), -2f),
			                                        Quaternion.identity);
		}
		
		//Shift the camera's focus to the lock display
		scrCamera.CutTo (objBackground);
	}


	public void DeleteDisplay(){
		//First, shift the camera's focus back to the player
		scrCamera.FindPlayer ();

		//Next, destroy all of the pins and pin holes
		for(int i=0; i<5 + (int)(difficulty * 3.5); ++i){
			GameObject.Destroy(objLockPins[i]);
			GameObject.Destroy(objLockPinHoles[i]);
		}

		//Next, destroy the background of the interface and mark that there is no interface right now
		GameObject.Destroy (objBackground);
		currentlyDisplayingInterface = false;

		//Finally, increment the difficulty level by one
		difficulty++;
	}


	//Function will check if the pin at index is in the hole
		//Every time that we check this and it is returning false, chink away at some of the durability of the pick
	public bool IsPinSolved(int index){
		float y1 = objLockPins[index].transform.position.y, y2 = objLockPinHoles[index].transform.position.y;
		float proximityNecessary = 0.07f;


		if(y1 > y2){
			if(y1 - y2 < proximityNecessary)
				return true;

			//If false, then lower durability
			durability -= 10;
			return false;
		}
		else{
			if(y2-y1 < proximityNecessary)
				return true;

			//If false, then lower durability
			durability -= 10;
			return false;
		}
	}
}
