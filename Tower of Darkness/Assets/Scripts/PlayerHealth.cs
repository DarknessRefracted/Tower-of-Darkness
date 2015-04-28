using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	Slider healthBar;
	GameObject player;
	public CharController scriptController;
	public MazeSolver solverScript;
	// Use this for initialization
	void Start () {
		GameObject temp = GameObject.Find ("HealthBar");
		player = GameObject.FindWithTag ("Player");
		//solverScript = (MazeSolver)GameObject.FindWithTag ("Tower").GetComponent ("MazeSolver");
		scriptController = player.GetComponent<CharController>();
		if (temp != null) {
			healthBar = temp.GetComponent<Slider>();
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Gargoyle") {
			damagePlayer(50);
			damagePlayer(20);
		}

	}

	void OnTriggerEnter2D(Collider2D other) {

		if(other.gameObject.tag == "Ghost"){
			damagePlayer(15);
			Destroy(other.gameObject);
		}
		
	}



	public void damagePlayer(int damageAmount){
		if(!scriptController.freezeMovement)
		{
		healthBar.value -= damageAmount;
		if (healthBar.value <= 0) {
			Application.LoadLevel("DeathScene");
			//Debug.Log("NINJA DEAD!");
		}
		}
	}
}
