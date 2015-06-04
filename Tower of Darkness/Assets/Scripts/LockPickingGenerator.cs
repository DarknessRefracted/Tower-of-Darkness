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
	//
	public GameObject damageMaxBlock;
	public GameObject damageLevelBlock;
	//
	public GameObject[] objDamageMax;
	public GameObject[] objDamageLevel;
	//Which damage level are we on?
	public int indicatorIndex;
	//The elapsed time block moves each time the damage level block moves (up or down). When it reaches the
		// end, then the player fails the lock picking.
	//public GameObject objTimeEnd;
	//public GameObject objTimeElapsed;

	//Difficulty level
	public int difficulty;

	//Durability of the lock pick. This forces the player to be at least somewhat conservative about pick attempts
		//Durability is lowered in the IsPinSolved() function, and in the pin movement section of the FixedUpdate()
			// funtion of the CharController.cs script
	//public int durability;
	public float levelInitialY;

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
		objDamageMax = new GameObject[2];
		objDamageLevel = new GameObject[2];
		indicatorIndex = 0;
	}


	//Function will display the lock picking interface
	public void GenerateDisplay(){
		//sum... will hold the total amount of movement necessary to solve the lock
		float sumOfNeccessaryMovement = 0, temp;
		//Set the durability of the lock pick
		//durability = 500 + 400 * difficulty;
		
		//Display the background and outer bounds of the interface
		objBackground = (GameObject) Instantiate (walledBackground, 
		                                        new Vector3(scrLevGen.start.transform.position.x + 11.0f,
		            							scrLevGen.player.transform.position.y - 0.5f,
		            							scrLevGen.start.transform.position.z),
		                                        Quaternion.identity);

		//Make the array of lock pin objects
		objLockPins = new GameObject[5 + (int)(difficulty * 1.75)];
		objLockPinHoles = new GameObject[5 + (int)(difficulty * 1.75)];

		//Display the pins and the pin holes
		for(int i=0, end = 5 + (int)(difficulty * 1.75); i<end; ++i){
			objLockPins[i] = (GameObject) Instantiate(lockPin,
			                                        new Vector3(objBackground.transform.position.x - 4.25f + (i*0.5f),
			            							objBackground.transform.position.y, -2f),
			                                        Quaternion.identity);

			temp = Random.Range(.125f, 1.06f);
			sumOfNeccessaryMovement += (temp);

			objLockPinHoles[i] = (GameObject) Instantiate(lockPinHole,
			                                        new Vector3(objBackground.transform.position.x - 4.25f + (i*0.5f),
			            							objBackground.transform.position.y + temp, -2f),
			                                        Quaternion.identity);
		}

		sumOfNeccessaryMovement -= difficulty;
		sumOfNeccessaryMovement /= 2;

		//Make the damage level indicator
			//If this indicator reaches max, then the lock picking is aborted
		levelInitialY = objBackground.transform.position.y - 0.5f;
		objDamageLevel[0] = (GameObject) Instantiate(damageLevelBlock,
		                                    new Vector3(objBackground.transform.position.x,
		            						levelInitialY, -2f),
		                                    Quaternion.identity);
		objDamageMax[0] = (GameObject) Instantiate(damageMaxBlock,
		                                    new Vector3(objBackground.transform.position.x,
		            						levelInitialY + sumOfNeccessaryMovement, -2f),
		                                    Quaternion.identity);
		objDamageLevel[1] = (GameObject) Instantiate(damageLevelBlock,
		                                    new Vector3(objBackground.transform.position.x + 0.5f,
		            						levelInitialY, -2f),
		                                    Quaternion.identity);
		objDamageMax[1] = (GameObject) Instantiate(damageMaxBlock,
		                                    new Vector3(objBackground.transform.position.x + 0.5f,
		            						levelInitialY + sumOfNeccessaryMovement, -2f),
		                                    Quaternion.identity);

		
		//Shift the camera's focus to the lock display
		scrCamera.CutTo (objBackground);
	}


	public void DeleteDisplay(){
		//First, shift the camera's focus back to the player
		scrCamera.FindPlayer ();

		//Next, destroy all of the pins and pin holes
		for(int i=0, end =5 + (int)(difficulty * 1.75); i<end; ++i){
			GameObject.Destroy(objLockPins[i]);
			GameObject.Destroy(objLockPinHoles[i]);
		}

		//Next, destroy the damage indicators
		GameObject.Destroy (objDamageLevel[0]);
		GameObject.Destroy (objDamageLevel[1]);
		GameObject.Destroy (objDamageMax[0]);
		GameObject.Destroy (objDamageMax[1]);

		//Finally, destroy the background of the interface and mark that there is no interface right now
		GameObject.Destroy (objBackground);
		currentlyDisplayingInterface = false;
	}


	//Function will check if the pin at index is in the hole
		//Every time that we check this and it is returning false, chink away at some of the durability of the pick
	public bool IsPinSolved(int index){
		float y1 = objLockPins[index].transform.position.y, y2 = objLockPinHoles[index].transform.position.y;
		float proximityNecessary = 0.07f;


		if(y1 > y2){
			if(y1 - y2 < proximityNecessary)
				return true;

			//If false, then raise the damage level
			objDamageLevel[indicatorIndex].transform.position = 
				new Vector3(objDamageLevel[indicatorIndex].transform.position.x, 
				            objDamageLevel[indicatorIndex].transform.position.y + 0.01f,
				            objDamageLevel[indicatorIndex].transform.position.z);
			return false;
		}
		else{
			if(y2-y1 < proximityNecessary)
				return true;

			//If false, then raise the damage level
			objDamageLevel[indicatorIndex].transform.position = 
				new Vector3(objDamageLevel[indicatorIndex].transform.position.x, 
				            objDamageLevel[indicatorIndex].transform.position.y + 0.01f,
				            objDamageLevel[indicatorIndex].transform.position.z);
			return false;
		}
	}
}
