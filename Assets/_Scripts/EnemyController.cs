using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	// PUBLIC INSTANCE VARIABLES
	public Transform boundaryCheck;
	public float enemySpeed;

	// PRIVATE INSTANCE VARIABLES
	private Transform _transform;
	private Rigidbody2D _rigidbody2D;
	private Animation _enemyAnimation;
	private bool _reachBoundary;
	private bool _moveLeft;
	private float _enemyForceX;
	private float _enemyForceY;


	// Use this for initialization
	void Start () {

		//Instantiate Public Variables


		//Instantiate Private Variables
		this._transform = gameObject.GetComponent<Transform>();
		this._rigidbody2D = gameObject.GetComponent<Rigidbody2D> ();
		this._enemyAnimation = gameObject.GetComponent<Animation> ();
		this._moveLeft = false;
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
		Debug.DrawLine (this._transform.position, this.boundaryCheck.position);

		// Check if the Enemy Reached the boundary
		if (this._reachBoundary) {

			/*Check the facing direction of the Enemy and
			 * Flip the opposite side of the facing
			*/
			if (this._moveLeft) {
				this._moveLeft = false;
				this.flipEnemy ();
			} else {
				this._moveLeft = true;
				this.flipEnemy ();
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
		}


		// Check the Enemy Speed, add force only if it's < 10
		if (absValueX <= this.enemySpeed) {
			this._rigidbody2D.AddForce (new Vector2 (_enemyForceX, _enemyForceY));
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
}
