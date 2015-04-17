using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {
	public GameObject groundLevel;
	public GameObject floor;
	public GameObject lev_sright, lev_sleft;

	public GameObject treasureChest;
	public GameObject stairsLeft;
	public GameObject stairsRight;

	public bool prev_is_stairleft;
	public float distanceBetweenLevels;//3.25
	public int chestChance;

	public GameObject start;
	//float currentHeight is automatically adjusted by the GetNextLevelPosition
	public float currentHeight;

	public GameObject player;
	private GameObject towerBacking;

	// Use this for initialization
	void Start () {
		GameObject temp;

		player = GameObject.FindGameObjectWithTag ("Player");
		towerBacking = GameObject.FindGameObjectWithTag ("TowerBacking");
		start = GameObject.FindGameObjectWithTag ("Start");

		//Generate the ground level
		Instantiate (groundLevel, start.transform.position, Quaternion.identity);

		//Decide if we're putting stairs on right or left, then set the bool variable
		if(Random.Range(0, 2) == 1 ){
			//Place stairs on the left
			temp = (GameObject) Instantiate(stairsLeft, new Vector3(-4.012F, .235F, -1F), Quaternion.identity);
			temp.transform.localScale = new Vector3(1.70F, 1.3820F, 1);

			//Set bool to true
			prev_is_stairleft = true;
		}
		else{
			//Place stairs on the right
			temp = (GameObject) Instantiate(stairsRight, new Vector3(4.016F, .235F, -1F), new Quaternion(0,180,0,0));
			temp.transform.localScale = new Vector3(1.70F, 1.3825F, 1);

			//Set bool to false
			prev_is_stairleft = false;
		}

		//Set current height. Remember that this height is altered when GetNextLevelPosition is called
		currentHeight = 0.24F;

		MakeNextLevel();
		MakeNextLevel();
		MakeNextLevel();
	}
	
	// Update is called once per frame
	void Update () {
		//Figure out if we need a new level generated. This should occur whenever the player is with one level distance 
			// of the currentHeight
		if(player.transform.position.y > (currentHeight - distanceBetweenLevels)){
			MakeNextLevel();
			//Also, move the towerBacking upwards one level distance, but only after 5 levels
			if((currentHeight/distanceBetweenLevels) > 5){
				towerBacking.transform.position = new Vector3(towerBacking.transform.position.x,
			                                              towerBacking.transform.position.y + (distanceBetweenLevels));
			}
		}
	}

	void MakeNextLevel(){
		GameObject temp;

		//Generate the next level
		//Figure out what kind of level needs to be generated
		if(prev_is_stairleft){
			Instantiate (lev_sright, GetNextLevelPosition(currentHeight), Quaternion.identity);
			prev_is_stairleft = false;

			//Place floor where it is needed
			temp = (GameObject) Instantiate (floor, new Vector3(start.transform.position.x - 1.5F, 
			                                                    currentHeight - (distanceBetweenLevels/2), 
			                                                    -1F), 
			                                 Quaternion.identity);
			temp.transform.localScale = new Vector3 (.4F, 1, 1);

		}
		else{
			Instantiate (lev_sleft, GetNextLevelPosition(currentHeight), Quaternion.identity);
			prev_is_stairleft = true;

			//Place floor where it is needed
			temp = (GameObject) Instantiate (floor, new Vector3(start.transform.position.x + 1.5F, 
			                                                    currentHeight - (distanceBetweenLevels/2), 
			                                                    -1F)
			                                 , Quaternion.identity);
			temp.transform.localScale = new Vector3 (.4F, 1, 1);
		}

		//Random chance of placing a treasure chest
		if(Random.Range(0,100) < chestChance){
			Instantiate(treasureChest, new Vector3(temp.transform.position.x, temp.transform.position.y + .5F)
			            , Quaternion.identity);
		}
	}

	//Function will add the distanceBeteweenLevels to the current_y, and return a Vector3 that has its y as the result,
		// and its x and z as the x and z of the start level.
	//Function will also automatically adjust the currentHeight
	Vector3 GetNextLevelPosition(float current_y){
		Vector3 newPosition = new Vector3(start.transform.position.x, 
		                                  current_y + distanceBetweenLevels, 
		                                  start.transform.position.z);

		currentHeight += distanceBetweenLevels;

		return newPosition;
	}
}
