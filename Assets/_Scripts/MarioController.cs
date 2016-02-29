using UnityEngine;
using System.Collections;

// VELOCITY RANGE UTILITY Class +++++++++++++++++++++++
[System.Serializable]
public class VelocityRange {
	// PUBLIC INSTANCE VARIABLES ++++
	public float minimum;
	public float maximum;

	// CONSTRUCTOR ++++++++++++++++++++++++++++++++++++
	public VelocityRange(float minimum, float maximum) {
		this.minimum = minimum;
		this.maximum = maximum;
	}
}

public class MarioController : MonoBehaviour {

	// PUBLIC INSTANCE VARIABLE
	public Transform playerCamera;
	public Transform groundCheck;
	public VelocityRange velocityRange;
	public float marioMoveForce;
	public float marioJumpForce;
	public float cameraPositionX;
	public float cameraPositionY;
	public GameController gameController;

	// PRIVATE INSTANCE VARIABLE
	private Transform _transform;
	private Animator _marioAnimator;
	private Rigidbody2D _rigidbody2D;
	private float _horizontalMove;
	private float _verticalMove;
	private bool _moveLeft;
	private bool _isGrounded;
	private AudioSource[] _audioSources;
	private AudioSource _jumpSound;
	private AudioSource _coinSound;



	// Use this for initialization
	void Start () {

		// Instantiate public variable
		this.velocityRange = new VelocityRange(300f, 30000f);

		// Instantiate private variable
		this._transform = gameObject.GetComponent<Transform>();
		this._rigidbody2D = gameObject.GetComponent<Rigidbody2D> ();
		this._marioAnimator = gameObject.GetComponent<Animator> ();
		this._horizontalMove = 0f;
		this._verticalMove = 0f;
		this._marioAnimator.SetInteger ("AnimState", 0);
		this._moveLeft = true;

		// Add Audio Source
		this._audioSources = gameObject.GetComponents<AudioSource>();
		this._jumpSound = this._audioSources [0];
		this._coinSound = this._audioSources [1];

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		// Local Variable
		float forceX = 0f;
		float forceY = 0f;
		Vector3 marioCurrentPosition = new Vector3(this._transform.position.x + this.cameraPositionX, this._transform.position.y + this.cameraPositionY, -10);
		this.playerCamera.position = marioCurrentPosition;

		this._isGrounded = Physics2D.Linecast (
			this._transform.position,
			this.groundCheck.position,
			1 << LayerMask.NameToLayer("Ground"));
		Debug.DrawLine (this._transform.position, this.groundCheck.position);

		float absValueX = Mathf.Abs (this._rigidbody2D.velocity.x);
		float absValueY = Mathf.Abs (this._rigidbody2D.velocity.y);


		// Check if player is grounded
		if (this._isGrounded) {
			
			this._horizontalMove = Input.GetAxis ("Horizontal");
			this._verticalMove = Input.GetAxis ("Vertical");

			if (this._horizontalMove != 0) {
			
				// Check if the Right arrow is pressed
				if (this._horizontalMove > 0) {

					// movement force
					if (absValueX < this.velocityRange.maximum) {
						forceX = this.marioMoveForce;
					}
					this._moveLeft = true;
					FlipMario ();
				}
				// Check if the Left arrow is pressed
				if (this._horizontalMove < 0) {

					// movement force
					if (absValueX < this.velocityRange.maximum) {
						forceX = -this.marioMoveForce;
					}
					this._moveLeft = false;
					FlipMario ();
				}

				// Set Animation clip to walk
				this._marioAnimator.SetInteger ("AnimState", 1);
			} else {
				// Set Animation clip to idle
				this._marioAnimator.SetInteger ("AnimState", 0);
			}

			if (this._verticalMove > 0) {
				// jump force
				if (absValueY < this.velocityRange.maximum) {
					this._jumpSound.Play ();
					forceY = this.marioJumpForce;
				}
			}
		} else {
			// Set Animation clip to Jump
			this._marioAnimator.SetInteger ("AnimState", 2);
		}

		this._rigidbody2D.AddForce (new Vector2 (forceX, forceY));


	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.CompareTag ("Coin")) {
			this._coinSound.Play ();
			Destroy (other.gameObject);
			// Add coin count
			this.gameController.CoinValue +=2;
		}
	}

	// Private Methods
	private void FlipMario(){

		if (this._moveLeft) {
			this._transform.localScale = new Vector2 (1, 1);
		} else {
			this._transform.localScale = new Vector2 (-1, 1);
		}

	}
		
}
