using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Door : MonoBehaviour {

	public GameObject door;
	public GameObject moveToObj;
	public GameObject keyTexture;
	
	private Vector3 initialPosition;
	private Vector3 moveToPosition;
	private float totalDist;

	public float moveSpeed = 4.0f;
	
	private bool playerInTrigger = false;
	public int tanksInTrigger = 0;

	private bool doorClosed = true;
	private bool doorOpen = false;
	private bool doorOpening = false;
	private bool doorClosing = false;


	void Start() {

		initialPosition = door.transform.position;
		moveToPosition = moveToObj.transform.position;

		totalDist = Vector3.Distance(initialPosition, moveToPosition);
	}


	void OnTriggerEnter (Collider col) {
		if (col.gameObject.tag == "Player")
		{
			if (doorClosed) {
				if  (keyTexture) {
					keyTexture.SetActive(true);
				}
			}
			playerInTrigger = true;
			tanksInTrigger++;
		}
		else if (col.gameObject.tag == "EnemyTank")
		{
			if (doorClosing) {
				doorOpening = true;
			}
			tanksInTrigger++;
		}
	}


	void OnTriggerExit (Collider col) {
		if (col.gameObject.tag == "Player")
		{
			if (keyTexture) {
				keyTexture.SetActive(false);
			}
			playerInTrigger = false;
			tanksInTrigger--;
		}
		
		else if (col.gameObject.tag == "EnemyTank")
		{
			tanksInTrigger--;
		}
	}


	void Update() {
		if (playerInTrigger) {
			if ((doorClosed) & (Input.GetKeyDown("e"))) {
				keyTexture.SetActive(false);
				doorClosed = false;
				doorOpening = true;
			}
			else if (doorClosing) {
				doorOpening = true;
			}
		}

		if ((doorOpening) & (!doorOpen)) {
			float distCovered = (Vector3.Distance(door.transform.position, initialPosition) + moveSpeed *Time.deltaTime) / totalDist;
			door.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp (initialPosition, moveToPosition, distCovered));
			
			if (distCovered >= 1.0f) {
				doorOpen = true;
				doorOpening = false;
			}
		}

		if ((tanksInTrigger < 1) & (!doorClosed))
		{
			doorOpen = false;
			doorOpening = false;
			doorClosing = true;

			if ((doorClosing) & (!doorClosed)) {
				float distCovered = (Vector3.Distance(door.transform.position, moveToPosition) + moveSpeed *Time.deltaTime) / totalDist;
				door.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp (moveToPosition, initialPosition, distCovered));
			
				if (distCovered >= 1.0f) {
					doorClosed = true;
					doorClosing = false;
				}
			}
		}
	}
}
