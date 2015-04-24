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

	//Getting the maze generation stuffs
	private GameObject tower;
	private MazeGeneration2 mazeScript;
	private MazeSolver solverScript;

	//The treasure chest most recently touched
	private GameObject recentChest;

	//Variable is set to true when we need to totally stop the subject
	private bool freezeMovement;

	//Animation states
	private int currentAnimationState;
	private enum moves{
		WALKLEFT, WALKRIGHT, 
		JUMPLEFT_UP, JUMPLEFT_DOWN, JUMPRIGHT_UP, JUMPRIGHT_DOWN, 
		CLINGUPLEFT, CLINGUPRIGHT, 
		CLINGLEFT, CLINGRIGHT, 
		STANDING
	};

	void Start(){
		//rigidbody2D = GetComponent<Rigidbody2D>();
		tower = GameObject.FindGameObjectWithTag ("Tower");
		mazeScript = (MazeGeneration2) tower.GetComponent<MazeGeneration2>();
		solverScript = (MazeSolver) tower.GetComponent<MazeSolver> ();

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
		else if(Input.GetKey(KeyCode.M)){
			//Delete the maze
			mazeScript.deleteMaze();

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
					
					if(pickLeft)
						currentAnimationState = (int)moves.JUMPLEFT_DOWN;
					else
						currentAnimationState = (int)moves.JUMPRIGHT_DOWN;
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
		else if(other.gameObject.CompareTag("Catcher"){
			//Kill character
		}
		else{
			onWall = false;

			if(other.gameObject.CompareTag("TreasureChest")){
				//Save the object for deleting later
				recentChest = other.gameObject;

				//Generate the maze
				mazeScript.startMazeGeneration();

				//Suspend movement--movmement is allowed when the maze has been completed by player or computer
				suspendMovement();
			}

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