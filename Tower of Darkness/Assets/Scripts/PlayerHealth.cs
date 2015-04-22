using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	Slider healthBar;

	// Use this for initialization
	void Start () {
		GameObject temp = GameObject.Find ("HealthBar");
		if (temp != null) {
			healthBar = temp.GetComponent<Slider>();
		}
	}

	void Update(){

	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Treasure") {
			damagePlayer(20);
			Debug.Log ("Player damaged by: " + col.gameObject.tag + "! Health: " + healthBar.value);
		}
	}

	void damagePlayer(int damageAmount){
		healthBar.value -= damageAmount;
		if (healthBar.value <= 0) {
			Debug.Log("NINJA DEAD!");
		}
	}
}
