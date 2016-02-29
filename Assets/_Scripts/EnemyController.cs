using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	// PUBLIC INSTANCE VARIABLES
	public Transform boundaryCheck;
	public Transform enemyWeakSpot;
	public float enemySpeed;
	public float dragSpeed;
	public GameController gameController;

	// PRIVATE INSTANCE VARIABLES
	private Transform _transform;
	private Rigidbody2D _rigidbody2D;
	private Animator _enemyAnimation;
	private PolygonCollider2D _enemyCollider;
	private bool _reachBoundary;
	private bool _hitSpot;
	private bool _moveLeft;
	private float _enemyForceX;
	private float _enemyForceY;
	private AudioSource[] _audioSources;
	private AudioSource _bumpSound;
	private AudioSource _stompSound;


	// Use this for initialization
	void Start () {

		//Instantiate Public Variables

		//Instantiate Private Variables
		this._transform = gameObject.GetComponent<Transform>();
		this._rigidbody2D = gameObject.GetComponent<Rigidbody2D> ();
		this._enemyAnimation = gameObject.GetComponent<Animator> ();
		this._enemyCollider = gameObject.GetComponent<PolygonCollider2D> ();
		this._moveLeft = false;

		// Add Audio Source
		this._audioSources = gameObject.GetComponents<AudioSource>();
		this._stompSound = this._audioSources [0];
		this._bumpSound = this._audioSources [1];
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//Local Variables

		this._enemyForceX = 0f;
		this._enemyForceY = 0f;
		float absValueX = Mathf.Abs (this._rigidbody2D.velocity.x);


		this._reachBoundary = Physics2D.Linecast (
			this._transform.position,
			this.boundaryCheck.position,
		1 << LayerMask.NameToLayer("EnemyBoundary"));

		// Check if the Enemy Reached the boundary
		if (this._reachBoundary) {

			this._rigidbody2D.drag = dragSpeed;
			// Set Animation clip to idle
			this._enemyAnimation.SetInteger ("EnemyState", 0);
			/*Check the facing direction of the Enemy and
			 * Flip the opposite side of the facing
			*/
			if (this._moveLeft) {
				this._moveLeft = false;
				this.flipEnemy ();
				//this._enemyForceX = this._enemyForceX;
			} else {
				this._moveLeft = true;
				this.flipEnemy ();
				//this._enemyForceX = -this._enemyForceX;
			}

		} else {
			/*Check the facing direction of the Enemy and
			 * Apply force based on the facing direction
			*/
			if (this._moveLeft) {
				this._enemyForceX = -this.enemySpeed;
			} else {
				this._enemyForceX = this.enemySpeed;
			}
			// Set Animation Clip to walk
			this._enemyAnimation.SetInteger ("EnemyState", 1);
		}


		// Check the Enemy Speed, add force only if it's < 10
		if (absValueX <= this.enemySpeed) {
			this._rigidbody2D.AddForce (new Vector2 (_enemyForceX, _enemyForceY));
		}

	}

	// Enemy Collision Detection
	void OnCollisionEnter2D(Collision2D other){

		this._hitSpot = Physics2D.Linecast (
			this._transform.position,
			this.enemyWeakSpot.position,
			1 << LayerMask.NameToLayer("WeakSpot"));
		Debug.DrawLine (this._transform.position, this.enemyWeakSpot.position);

		// Check if enemy Weak Spot is hit
		if (this._hitSpot) {
			
			Debug.Log ("Hit");
			// Set Animation Clip to walk
			this._enemyAnimation.SetInteger ("EnemyState", 2);
			this._enemyCollider.isTrigger = true;
			this._stompSound.Play ();
			//Add score to the player
			this.gameController.ScoreValue += 10;

		} else {
			if (other.gameObject.CompareTag ("Mario")) {
				Debug.Log ("Mario Collide");
				this._bumpSound.Play ();
				// Subtract player life
				this.gameController.LivesValue--;
			}
		}
	}

	// Private Methods
	private void flipEnemy(){

		if (this._moveLeft) {
			this._transform.localScale = new Vector2 (-1, 1);
		} else {
			this._transform.localScale = new Vector2 (1, 1);
		}

	}


	private void DestroyEnemy(){
	}
}
