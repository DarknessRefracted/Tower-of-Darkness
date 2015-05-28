using UnityEngine;
using System.Collections;

public class CharController : MonoBehaviour {
	private bool grounded = true;
	private bool onWall = false;
	private bool candoublejump = true;
	public float jumpForce= 250f;
	public Rigidbody2D rigidbody2d;
	public float jumpCooldown = 0.4f;
	public float speed = 5f;
	private float move;
	private Animator animationController;
	private bool pickLeft;

	//Score script
	public ScoreScript scrScore;

	//Getting the lock picking stuffs
	private GameObject tower;

	//The treasure chest most recently touched
	private GameObject recentChest;

	//Variable is set to true when we need to totally stop the subject
	public bool freezeMovement;
	public DirectionalCollision scriptCollision;

	//Animation states
	private int currentAnimationState;

	//Script for Lock Picking Interface
	public LockPickingGenerator scrLockPickGen;

	//Script for health
	public PlayerHealth scrHealth;


	//Integer to hold what pin the user is currently controlling
	int lockPinIndex;
	Rigidbody2D rb2d;

	private enum moves{
		WALKLEFT, WALKRIGHT, 
		JUMPLEFT_UP, JUMPLEFT_DOWN, JUMPRIGHT_UP, JUMPRIGHT_DOWN, 
		CLINGUPLEFT, CLINGUPRIGHT, 
		CLINGLEFT, CLINGRIGHT, 
		STANDING
	};

	void Start(){
		scriptCollision = GetComponent<DirectionalCollision>();
		//rigidbody2D = GetComponent<Rigidbody2D>();
		tower = GameObject.FindGameObjectWithTag ("Tower");
		scrLockPickGen = tower.GetComponent<LockPickingGenerator> ();
		scrHealth = (PlayerHealth)GetComponent<PlayerHealth> ();
		scrScore = (ScoreScript)GetComponent<ScoreScript> ();
		lockPinIndex = 0;

		freezeMovement = false;

		animationController = this.GetComponent<Animator>();
		currentAnimationState = 10;
		pickLeft = true;
	}

	void Update(){
		if (!grounded) {
			jumpCooldown -= Time.deltaTime;
		}
	}

	void FixedUpdate(){
		if(!freezeMovement){
			handleMovement();
		}
		else{
			//Controlling the pins on the lock
			if( !(Input.GetKey(KeyCode.M) || scrLockPickGen.lockSolved) ){
				//Check if user wants to move onto the next pin (not sure if to punish them for mistakes or prevent them from moving on).
					//Right now, it just requires the user to have it right in order to move on
				if(Input.GetKey(KeyCode.E)){
					if(scrLockPickGen.IsPinSolved(lockPinIndex)){
						//If we've solved the last pin, then exit the lock and award points
						if(++lockPinIndex == 5 + (int)(scrLockPickGen.difficulty * 1.5)){
							scrLockPickGen.lockSolved = true;
							return;
						}
					}
				}

				//If user wants to move pin upwards
				if((scrLockPickGen.objLockPins[lockPinIndex].transform.position.y <=
				   scrLockPickGen.objBackground.transform.position.y + 1.1f)
					&& (Input.GetKey(KeyCode.W))){
					scrLockPickGen.objLockPins[lockPinIndex].transform.position = 
						new Vector3(scrLockPickGen.objLockPins[lockPinIndex].transform.position.x, 
						            scrLockPickGen.objLockPins[lockPinIndex].transform.position.y + 0.0625f,
						            scrLockPickGen.objLockPins[lockPinIndex].transform.position.z);
				}
				//Current pin will always be moving downwards (while not in its initial position)
				else if(scrLockPickGen.objLockPins[lockPinIndex].transform.position.y > 
				        scrLockPickGen.objBackground.transform.position.y){
					scrLockPickGen.objLockPins[lockPinIndex].transform.position = 
						new Vector3(scrLockPickGen.objLockPins[lockPinIndex].transform.position.x, 
						            scrLockPickGen.objLockPins[lockPinIndex].transform.position.y - 0.03125f,
						            scrLockPickGen.objLockPins[lockPinIndex].transform.position.z);
				}
			}

			//Player has successfully completed the lock picking, or has given up. Award no points for the latter
			else if(Input.GetKey(KeyCode.M) || scrLockPickGen.lockSolved){
				//Delete the lockpicking display
				scrLockPickGen.DeleteDisplay();

				//Delete the chest
				GameObject.Destroy(recentChest);

				//Allow movement again
				allowMovement();

				//If the lock was solved, then award points
				if(scrLockPickGen.lockSolved)
					scrScore.IncreaseScore(scrLockPickGen.difficulty + 5);
				
				//Reset pin index and lockSolved
				lockPinIndex = 0;
				scrLockPickGen.lockSolved = false;
			}
		}
	}
	
	void handleMovement(){
		move = Input.GetAxis ("Horizontal");
		rigidbody2D.velocity = new Vector2 (move * speed, rigidbody2D.velocity.y);

		if (Input.GetKey(KeyCode.W)) {//Input.GetAxis ("Vertical") > 0 other choice 
			if (grounded) {
				
				rigidbody2d.velocity = new Vector2 (rigidbody2D.velocity.x, 0);
				rigidbody2d.AddForce (new Vector2 (0, jumpForce));
				grounded = false;
				jumpCooldown = 0.4f;
				
				if(!onWall){
					if(pickLeft){
						currentAnimationState = (int)moves.JUMPLEFT_UP;
					}
					else
						currentAnimationState = (int)moves.JUMPRIGHT_UP;
				}
				
			} else if( jumpCooldown < 0) {
				if (candoublejump) {
					candoublejump = false;
					rigidbody2d.velocity = new Vector2 (rigidbody2D.velocity.x, 0);
					rigidbody2D.AddForce (new Vector2 (0, jumpForce));
				}
			}
		}
		else if(grounded && !onWall){
			if(Input.GetKey(KeyCode.A)){
				currentAnimationState = (int)moves.WALKLEFT;
				pickLeft = true;
			}
			else if (Input.GetKey(KeyCode.D)){
				currentAnimationState = (int)moves.WALKRIGHT;
				pickLeft = false;
			}
			else{
				currentAnimationState = (int)moves.STANDING;
			}
		}
			
		animationController.SetInteger("Action", currentAnimationState);
	}


	void OnCollisionEnter2D(Collision2D other) {
		//grounded = true;
		//candoublejump = true;

		if (other.gameObject.CompareTag("RVerticalWall")){
			currentAnimationState = (int)moves.JUMPLEFT_UP;
			onWall = true;
		}
		else if(other.gameObject.CompareTag("LVerticalWall")){
			currentAnimationState = (int)moves.JUMPRIGHT_UP;
			onWall = true;
		}
		else if(other.gameObject.CompareTag("Catcher")){
			//Kill character
			scrHealth.damagePlayer(100);
		}
		else{
			//onWall = false;

			if(other.gameObject.CompareTag("TreasureChest")){
				//Save the object for deleting later
				recentChest = other.gameObject;

				//Generate the display for lockpicking
				scrLockPickGen.GenerateDisplay();

				//Suspend movement--movmement is allowed when the player has picked the lock or if user has pressed
					// the 'M' key
				suspendMovement();
			}

		}
	}
	void OnCollisionExit2D(Collision2D other) {
		grounded = true;
		candoublejump = true;
		
		if (other.gameObject.CompareTag("RVerticalWall")){
			currentAnimationState = (int)moves.CLINGRIGHT;
			onWall = false;
		}
		else if(other.gameObject.CompareTag("LVerticalWall")){
			currentAnimationState = (int)moves.CLINGLEFT;
			onWall = false;
		}
		else if(other.gameObject.CompareTag("Catcher")){
			//Kill character

		}
	}
	void OnCollisionStay2D(Collision2D other) {
		grounded = true;
		candoublejump = true;
		if (other.gameObject.CompareTag("RVerticalWall")){
			currentAnimationState = (int)moves.CLINGRIGHT;
			onWall = true;
		}
		else if(other.gameObject.CompareTag("LVerticalWall")){
			currentAnimationState = (int)moves.CLINGLEFT;
			onWall = true;
		}
	}


	//Function prevents the subject from moving (used after generating a maze
	void suspendMovement(){
		freezeMovement = true;
		rigidbody2d.velocity = new Vector2 (0, 0);
	}
	//Function restores movement to the subject
	void allowMovement(){
		freezeMovement = false;
	}
}