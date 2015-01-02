using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.

	public Transform[] tiles;				//Array of tiles to build the level

	public bool triedTut = false;			// Check if the user is not afk and ready to start the level
	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.

	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	public bool grounded = false;			// Whether or not the player is grounded.
	private Animator anim;					// Reference to the player's animator component.


	public static int currScore = 0; 		//Storing the collected coins
	public static int levelScore = 5;		//Number of coing required for a specific level

	private int myLayerMask = 1;

	void Awake(){
		groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();
	}


	void Update(){
		myLayerMask = 1 << LayerMask.NameToLayer("Ground");

		grounded = Physics2D.Linecast(transform.position, groundCheck.position, myLayerMask);

		if(Input.GetButtonDown("Jump") && grounded)
			jump = true;

	}


	void FixedUpdate (){
		if(Input.GetButtonDown("Jump")){
			triedTut = true; //chech for finishing the tutorial
			//hide the tutorial GUI layer
		}

		if(triedTut){
		rigidbody2D.AddForce(Vector2.right * moveForce);

		float h = Input.GetAxis("Horizontal");
		//
		//Touch myJump = Input.GetTouch(0);

		anim.SetFloat("hSpeed", Mathf.Abs(h));

		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(h * rigidbody2D.velocity.x < maxSpeed)
			// ... add a force to the player.
			rigidbody2D.AddForce(Vector2.right * h * moveForce);

		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);


		if(h > 0 && !facingRight)
			Flip();
		else if(h < 0 && facingRight)
			Flip();
		

		if(jump){
			anim.SetTrigger("Jump");
				if(!audio.isPlaying){
					audio.Play();
				}
			rigidbody2D.AddForce(new Vector2(0, jumpForce));

			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;
		}
		}
	}
	
	
	void Flip (){
		facingRight = !facingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


	//Function of crashing the Sand
	/*function OnTriggerEnter2D( col : Collider2D ){
		if(col.tag == "Sand"){
			Destroy(col.gameObject)
		}
	}
	*/

}