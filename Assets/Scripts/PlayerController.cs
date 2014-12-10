using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour {
	public GameObject gameController;

	public Boundary boundary;
	public float speed = 6f;
	public float upDownRange = 60.0f;
	public float mouseSensitivity = 5.0f;
	private float verticalRotation = 0;
	private CharacterController characterController;

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate = 0.25f;
	private float nextFire = 0.0f;

	private const KeyCode summonSwordKey = KeyCode.Q;

	// Use this for initialization
	void Start () {
		Screen.lockCursor = true;
		characterController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		// Rotation
		float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
		transform.Rotate(0, rotLeftRight, 0);

		verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
		Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
		// so that shots can have vertical variations
		shotSpawn.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

		// movement
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		Vector3 movement  = new Vector3(h, 0f, v);
		characterController.SimpleMove(movement * speed);

		// position clamp
		transform.position = new Vector3(
			Mathf.Clamp (transform.position.x, boundary.xMin, boundary.xMax),
			transform.position.y,
			Mathf.Clamp (transform.position.z, boundary.zMin, boundary.zMax)
		);

		// shoot
		if(Input.GetButton ("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			GameObject clone = Instantiate(shot, shotSpawn.position, shotSpawn.rotation) as GameObject;
		}

		// summon swords
		if(Input.GetKeyDown(summonSwordKey)) {
			GameController gc = gameController.GetComponent<GameController>();
			gc.SummonSwords();
		}
	}
}
