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
		else if(Input.GetKey(KeyCode.M) /*WIN CONDITION*/){
			//Delete the lockpicking display
			scrLockPickGen.DeleteDisplay();

			//Delete the chest
			GameObject.Destroy(recentChest);

			//Allow movement again
			allowMovement();
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

				//Suspend movement--movmement is allowed when the player has picked the lock or if user presses
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