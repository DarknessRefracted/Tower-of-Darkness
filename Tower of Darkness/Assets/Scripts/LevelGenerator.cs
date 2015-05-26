using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {
	public GameObject groundLevel;
	public GameObject floor;
	public GameObject lev_sright, lev_sleft;
	public GameObject lev_sright_hole, lev_sleft_hole;
	public int default_hole_chance;
	private int hole_chance;

	public GameObject treasureChest;
	public GameObject stairsLeft;
	public GameObject stairsRight;

	public bool prev_is_stairleft;
	public float distanceBetweenLevels;//3.25

	//Igor's added variables
	public bool spawnGhosts = true;
	public GameObject ghost;
	public GameObject gargoyle;
	public int chestChance;
	public int levelCounter = 1;
	public int ghostChance = 1;
	public Vector3 tempVectorPos;

	public GameObject start;
	//float currentHeight is automatically adjusted by the GetNextLevelPosition
	public float currentHeight;

	public GameObject player;
	private GameObject towerBacking;
	bool changeOnce = false;
	bool changeTwice = false;
	float changeTime = 0.0f;

	// Use this for initialization
	void Start () {
		GameObject temp;
		Camera.main.clearFlags = CameraClearFlags.SolidColor;

		player = GameObject.FindGameObjectWithTag ("Player");
		towerBacking = GameObject.FindGameObjectWithTag ("TowerBacking");
		start = GameObject.FindGameObjectWithTag ("Start");
		hole_chance = default_hole_chance;

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
		if (!changeOnce) {

			Camera.main.backgroundColor = Color.Lerp(Color.blue, Color.red, changeTime);
			if(Camera.main.backgroundColor == Color.red){
				changeOnce = true;
				changeTime = 0f;
			}
		}
		if (!changeTwice && changeOnce) {
			if(Camera.main.backgroundColor == Color.black){
				changeTwice = true;
				
			}
			Camera.main.backgroundColor = Color.Lerp(Color.red, Color.black, changeTime);
		}

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
		levelCounter++;
		changeTime += 0.02f;
		GameObject temp;

		//Generate the next level
		//Figure out what kind of level needs to be generated
		if(prev_is_stairleft){

			//Decide whether or not to remove the opposite wall
			if(Random.Range(0, 10) < hole_chance){
				tempVectorPos = GetNextLevelPosition(currentHeight);
				Instantiate (lev_sright_hole, tempVectorPos, Quaternion.identity);
				//Reset the hole chance
				hole_chance = default_hole_chance;
				if(Random.Range(0, 10) > 7)
				{
					Instantiate (gargoyle, tempVectorPos, Quaternion.identity);
				}
			}
			else{
				tempVectorPos = GetNextLevelPosition(currentHeight);
				Instantiate (lev_sright, tempVectorPos, Quaternion.identity);
				hole_chance++;
				if(Random.Range (0,5) < ghostChance)
				{
					if(spawnGhosts)
					{
						Instantiate(ghost, new Vector3(tempVectorPos.x + Random.Range (0,20),tempVectorPos.y + Random.Range (0,20), 0), Quaternion.identity);
						ghostChance = 1;
					}
				} 
				else
				{
					ghostChance++;
				}
			}

			prev_is_stairleft = false;

			//Place floor where it is needed
			temp = (GameObject) Instantiate (floor, new Vector3(start.transform.position.x - 1.5F, 
			                                                    currentHeight - (distanceBetweenLevels/2), 
			                                                    -1F), 
			                                 Quaternion.identity);
			temp.transform.localScale = new Vector3 (.4F, 1, 1);

		}
		else{

			//Decide whether or not to remove the opposite wall
			if(Random.Range(0, 10) < hole_chance){
				tempVectorPos = GetNextLevelPosition(currentHeight);
				Instantiate (lev_sleft_hole, tempVectorPos, Quaternion.identity);
				//Reset the hole chance
				hole_chance = default_hole_chance;
				if(Random.Range(0, 10) > 7)
				{
					Instantiate (gargoyle, tempVectorPos, Quaternion.identity);
				}
			}
			else{
				Instantiate (lev_sleft, GetNextLevelPosition(currentHeight), Quaternion.identity);
				hole_chance++;
			}

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
				treasureChest.gameObject.tag = "TreasureChest";
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
