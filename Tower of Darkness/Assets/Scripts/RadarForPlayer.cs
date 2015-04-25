using UnityEngine;
using System.Collections;

public class RadarForPlayer : MonoBehaviour {

	public bool playerInRange = false;
	void Awake()
	{
		playerInRange = false;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("Player")) {
			playerInRange = true;
		}
	}
	
	
	
	void OnTriggerExit2D(Collider2D other) {
		if(other.CompareTag("Player")){
			playerInRange = false;
		}
	}
}
