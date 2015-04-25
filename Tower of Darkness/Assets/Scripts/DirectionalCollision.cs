using UnityEngine;
using System.Collections;

//http://pastebin.com/nvJCSuZb
// But in unity you cant mix 2D and 3D so if you want to mix it than rays are your best bet

[RequireComponent(typeof(Rigidbody2D))]
public class DirectionalCollision : MonoBehaviour {
	
	public bool right=false, left=false, up=true, down=false;
	
	void OnCollisionEnter2D(Collision2D coll){
		//for every collision coll saves 2 points of contact test begining of collision and end of collision
		if(coll.contacts.Length==2){
			//chack are two points on x axis the same 
			if( coll.contacts[0].point.x == coll.contacts[1].point.x ){  
				// chack where they are in regards to game object origin
				if( coll.contacts[0].point.x > transform.position.x ){ 
					right = true;
				}else{
					left = true;
				}
			}else if(coll.contacts[0].point.y == coll.contacts[1].point.y){
				if(coll.contacts[0].point.y > transform.position.y){
					up = true;
				}else{
					down = true;
				}
			}
		}else{
			Debug.LogError("This script is defined only for 2D collisions");
		}
	}
	
	void OnCollisionExit2D(Collision2D coll){
		if(coll.contacts.Length==2){
			if( coll.contacts[0].point.x == coll.contacts[1].point.x ){
				if( coll.contacts[0].point.x > transform.position.x ){
					right = false;
				}else{
					left = false;
				}
			}else if(coll.contacts[0].point.y == coll.contacts[1].point.y){
				if(coll.contacts[0].point.y > transform.position.y){
					up = false;
				}else{
					down = false;
				}
			}
		}
	}
}
