using UnityEngine;
using System.Collections;

public class CharController : MonoBehaviour {
	private bool grounded = true;
	private bool candoublejump = true;
	public float jumpForce= 250f;
	public Rigidbody2D rigidbody2d;
	public float jumpCooldown = 0.4f;
	public float speed = 5f;
	private float move;
	void Start(){
		//rigidbody2D = GetComponent<Rigidbody2D>();
	}

	void Update(){
				if (!grounded) {
						jumpCooldown -= Time.deltaTime;
				} 
		}
	void FixedUpdate(){
		move = Input.GetAxis ("Horizontal");
		rigidbody2D.velocity = new Vector2 (move * speed, rigidbody2D.velocity.y);

		if (Input.GetKey(KeyCode.W)) {//Input.GetAxis ("Vertical") > 0 other choice 
						if (grounded) {
								rigidbody2d.velocity = new Vector2 (rigidbody2D.velocity.x, 0);
								rigidbody2d.AddForce (new Vector2 (0, jumpForce));
								grounded = false;
								candoublejump = true;
								jumpCooldown = 0.4f;
						} else if( jumpCooldown < 0) {
								if (candoublejump) {
										candoublejump = false;
										rigidbody2d.velocity = new Vector2 (rigidbody2D.velocity.x, 0);
										rigidbody2D.AddForce (new Vector2 (0, jumpForce));
								}
						}
				}
		}
	void OnTriggerEnter2D(Collider2D other) {
		grounded = true;
	}

	void OnCollisionEnter2D(Collision2D other) {
		grounded = true;
	}
}