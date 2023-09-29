using UnityEngine;
using System.Collections;

public class PlayerTank : MonoBehaviour {

	public float moveSpeed = 20.0f;  // units per second
	public float rotateSpeed = 3.0f;

	public int health = 100;
	private int maxHealth;
	
	private Transform _transform;
	private Rigidbody _rigidbody;

	public GameObject turret;
	public GameObject bullet;
	public GameObject bulletSpawnPoint;

	public float turretRotSpeed = 3.0f;

	//Bullet shooting rate
	public float shootRate = 1.5f;
	protected float elapsedTime;

	public int ammo = 5;
	private const int maxAmmo = 20;

	private int score = 0;

	private float move;     // Store the user's last forward/backward commands
	private float rotate;   // Store the user's last rotation commands
	private float thrust = 6.5f;  // units per second
	private float maxVelocity = 20.0f;
	private float torque = 1.4f;
	private float maxAngularVelocity = 2.0f;

	public Camera mainCamera;
	public Camera secondCamera;
	public GameObject smokeTrail;
	public GameObject explosion;


	// Use this for initialization
	void Start () {
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody>();

		maxHealth = health;

		elapsedTime = shootRate; // allow first press to fire
		rotateSpeed = rotateSpeed * 180 / Mathf.PI; // convert from rad to deg for rot function

		secondCamera.enabled = false;

		
	}

	private void Update()
	{
		move = Input.GetAxis("Vertical");
		rotate = Input.GetAxis("Horizontal");

		if(health <= 0)
        {
			Explode();
		}
	}

	// Update is called once per frame
	void FixedUpdate () {

		//// check for input
		//float rot = _transform.localEulerAngles.y + rotateSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
		//Vector3 fwd = _transform.forward * moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");

		//// Tank Chassis is rigidbody, use MoveRotation and MovePosition
		//_rigidbody.MoveRotation(Quaternion.AngleAxis(rot, Vector3.up));
		//_rigidbody.MovePosition(_rigidbody.position + fwd);

		if (Mathf.Abs(_rigidbody.velocity.z) < maxVelocity)
		{
			_rigidbody.AddForce(transform.forward * move * thrust, ForceMode.VelocityChange);
		}
		if (Mathf.Abs(_rigidbody.angularVelocity.y) < maxAngularVelocity)
		{
			_rigidbody.AddTorque(transform.up * rotate * torque, ForceMode.VelocityChange);
		}

		if (turret) {
			Plane playerPlane = new Plane(Vector3.up, _transform.position + new Vector3(0, 0, 0));
			
			// Generate a ray from the cursor position
			Ray RayCast = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			// Determine the point where the cursor ray intersects the plane.
			float HitDist = 0;
			
			// If the ray is parallel to the plane, Raycast will return false.
			if (playerPlane.Raycast(RayCast, out HitDist))
			{
				// Get the point along the ray that hits the calculated distance.
				Vector3 RayHitPoint = RayCast.GetPoint(HitDist);
				
				Quaternion targetRotation = Quaternion.LookRotation(RayHitPoint - _transform.position);
				turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, targetRotation, Time.deltaTime * turretRotSpeed);
			}
		}

		if(Input.GetButton("Fire1"))
		{
			if (elapsedTime >= shootRate)
			{
				//Reset the time
				elapsedTime = 0.0f;

				if (ammo > 0) {
					//Also Instantiate over the PhotonNetwork
					if ((bulletSpawnPoint) & (bullet)) {
						Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
						ammo--;
					}
				}
			}
		}

		// Update the time
		elapsedTime += Time.deltaTime;
	}

	// Apply Damage if hit by bullet
	public void ApplyDamage(int damage ) {
		health -= damage;
	}

	// Increment ammo
	public void ApplyAmmoPickup() {
		ammo = ammo + 5;
		if (ammo > maxAmmo) ammo = maxAmmo;
	}

	// Increment ammo
	public void UpdateScore(int addScore) {
		score = score + addScore;
		// update the GUI score here

	}

	protected void Explode()
	{
		float rndX = Random.Range(8.0f, 12.0f);
		float rndZ = Random.Range(8.0f, 12.0f);

		GetComponent<Rigidbody>().angularDrag = 0.05f;
		GetComponent<Rigidbody>().drag = 0.05f;

		for (int i = 0; i < 3; i++)
		{
			GetComponent<Rigidbody>().AddExplosionForce(10.0f, transform.position - new Vector3(rndX, 2.0f, rndZ), 45.0f, 40.0f);
			GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(rndX, 10.0f, rndZ));
		}

		if (smokeTrail)
		{
			GameObject clone = Instantiate(smokeTrail, transform.position, transform.rotation) as GameObject;
			clone.transform.parent = transform;
		}
		Invoke("CreateFinalExplosion", 1.4f);

		//Vector3 GlobalMainCamPos = transform.TransformPoint(mainCamera.transform.position);
		//secondCamera.transform.position = GlobalMainCamPos;
		secondCamera.enabled = true;
		Destroy(gameObject, 1.5f);
	}

	protected void CreateFinalExplosion()
	{
		if (explosion)
			Instantiate(explosion, transform.position, transform.rotation);
	}
}
