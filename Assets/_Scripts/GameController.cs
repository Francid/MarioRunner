using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	// Public Variables
//	public int enemyCount;
//	public float enemyWait;
//	public float startEnemyWait;
//	public float waveEnemyWait;
//	public GameObject enemyDistration;
//	public DragonController dragon;
//	public EnemyController enemy;
	public MarioController marioController;
	public Text livesLabel;
	public Text scoreLabel;
	public Text gameOverLabel;
	public Text coinLabel;
	public Button restartButton;


	// PRIVATE INSTANCE VARIABLE
	private Transform _enemyDTransform;
	private int _scoreValue;
	private int _livesValue;
	private int _coinValue;
	private AudioSource[] _audioSources;
	//private AudioSource _backgroundSource;
	private AudioSource _gameOverSource;

	public int ScoreValue{
		get{
			return this._scoreValue;
		}
		set{ 
			this._scoreValue = value;
			this.scoreLabel.text = "Score: " + this._scoreValue;
		}
	}

	public int CoinValue{
		get{
			return this._coinValue;
		}
		set{ 
			this._coinValue = value;
			this.coinLabel.text = "Coins: " + this._coinValue;
		}
	}
	public int LivesValue{
		get{
			return this._livesValue;
		}
		set{
			this._livesValue = value;
			if (this._livesValue <= 0) {
				this._EndGame ();
			}
			this.livesLabel.text = "Lives: " + this._livesValue;
		}
	}

	// Use this for initialization
	void Start () {
		this._audioSources = gameObject.GetComponents<AudioSource> ();
		this._Intialize ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	private void _Intialize(){
		this.ScoreValue = 0;
		this.LivesValue = 3;
		this.CoinValue = 0;
		this.gameOverLabel.gameObject.SetActive (false);
		this.restartButton.gameObject.SetActive (false);
		//this._backgroundSource = this._audioSources [0];
		this._gameOverSource = this._audioSources [0];
		//this._backgroundSource.Play ();

//		this._enemyDTransform = this.enemyDistration.GetComponent<Transform>();
//		StartCoroutine (EnemyWaves ());
	}

	private void _EndGame(){
		//this.dragon.gameObject.SetActive (false);
		//this.enemy.gameObject.SetActive (false);
		this.marioController.gameObject.SetActive(false);
		this.livesLabel.gameObject.SetActive (false);
		this.scoreLabel.gameObject.SetActive (false);
		this.coinLabel.gameObject.SetActive (false);
		this.gameOverLabel.gameObject.SetActive (true);
		this.restartButton.gameObject.SetActive (true);
		//this._backgroundSource.Stop ();
		this._gameOverSource.Play ();
	}

//	// Instantiate New Enemy at a Setting time
//	IEnumerator EnemyWaves(){
//		yield return new WaitForSeconds (this.startEnemyWait);
//
//		while (true) {
//			for (int i = 0; i < this.enemyCount; i++) {
//				this._enemyDTransform.position = new Vector2 (212f, Random.Range (-125, 125));
//				Instantiate (this.enemyDistration);
//				yield return new WaitForSeconds (this.enemyWait);
//			}
//			yield return new WaitForSeconds (this.waveEnemyWait);
//		}
//	}
//
	// Restart the Main Scene
	public void RestartButtonclick(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
}
