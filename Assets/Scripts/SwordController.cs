using UnityEngine;
using System.Collections;

public class SwordController : MonoBehaviour {

	public float floatUpDistance = 1f;
	public float tumble = 1f;
	public GameObject player;

	private SwordState currentState;
	private bool orbit;

	public float spinningSpeed;

	// Use this for initialization
	void Start () {
		orbit = false;
		currentState = SwordState.Unselected;
	}
	
	// Update is called once per frame
	void Update () {
		if(orbit) {
			Orbit ();
		}
	}

	public void StartOrbit() {
		orbit = true;
		UpdateState(SwordState.Spinning);
	}

	public void StopOrbit() {
		orbit = false;
		UpdateState(SwordState.Unselected);
	}

	void Orbit() {
		transform.RotateAround(player.transform.position
		                       , Vector3.up
		                       , spinningSpeed * Time.deltaTime);
	}
	
	public void StartRotate() {
		rigidbody.angularVelocity = new Vector3(0f, tumble, 0f);
	}
	
	public void StopRotate() {
		rigidbody.freezeRotation = true;
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("Collision enter");

		if(other.tag == "Beam") {
			BeShot();
		}
	}

	public void UpdateState(SwordState state) {
		currentState = state;
	}

	public SwordState GetState() {
		return currentState;
	}

	void BeShot() {
		Debug.Log ("Be shot");

		switch(currentState) {
		case SwordState.Unselected:
			Vector3 newPos = new Vector3(transform.position.x
			                             , transform.position.y + floatUpDistance
			                             , transform.position.z);
			transform.position = newPos;

			UpdateState(SwordState.Selected);
			break;
		case SwordState.Selected:
			Vector3 newPos2 = new Vector3(transform.position.x
			                             , transform.position.y - floatUpDistance
			                             , transform.position.z);
			transform.position = newPos2;

			UpdateState(SwordState.Unselected);
			break;
		case SwordState.Spinning:
			// handle this sword
			transform.position = player.transform.Find ("Sword Handling Point").transform.position;
			transform.Rotate(new Vector3(135f, 0f, 15f));
			// add as child
			transform.parent = player.transform;
			StopOrbit();
			StopRotate();

			// let other swords drop
			GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
			GameController gc = gameController.GetComponent<GameController>();
			gc.ChooseSword(name);

			UpdateState(SwordState.InHand);
			break;
		case SwordState.InHand:
			break;
		default:
			break;
		}
	}
}
