using UnityEngine;
using System.Collections;

public class SimpleFSM : MonoBehaviour
{
    public enum FSMState
    {
        None,
        Patrol,
        Dead,
    }

    // Current state that the NPC is reaching
    public FSMState curState;

    public float moveSpeed = 8.0f; // Speed of the tank
    public float rotSpeed = 2.0f; // Tank Rotation Speed

    protected Transform playerTransform;// Player Transform
    protected Vector3 destPos; // Next destination position of the NPC Tank
    protected GameObject[] pointList; // List of points for patrolling

    // Whether the NPC is destroyed or not
    protected bool bDead;
    public int health = 100;

    // Effects for death
    public GameObject explosion;
    public GameObject smokeTrail;


    /*
     * Initialize the Finite state machine for the NPC tank
     */
    void Start()
    {
        curState = FSMState.Patrol;

        bDead = false;

        // Get the list of patrol points
        pointList = GameObject.FindGameObjectsWithTag("PatrolPoint");
        FindNextPoint();  // Set a random destination point first

        // Get the target (Player)
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;
        if (!playerTransform)
            print("Player doesn't exist.. Please add one with Tag named 'Player'");
    }


    // Update each frame
    void Update()
    {

        switch (curState)
        {
            case FSMState.Patrol: UpdatePatrolState(); break;
            case FSMState.Dead: UpdateDeadState(); break;
        }

        // Go to dead state if no health left
        if (health <= 0)
            curState = FSMState.Dead;
    }


    /*
     * Patrol state
     */
    protected void UpdatePatrolState()
    {
        // Find another random patrol point if the current point is reached
        if (Vector3.Distance(transform.position, destPos) <= 2.0f)
        {
            FindNextPoint();
        }

        // Rotate to the target point
        Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        GetComponent<Rigidbody>().MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed));

        // Go Forward
        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.forward * Time.deltaTime * moveSpeed);
    }



    /*
     * Dead state
     */
    protected void UpdateDeadState()
    {
        // Show the dead animation with some physics effects
        if (!bDead)
        {
            bDead = true;
            Explode();
        }
    }


    // Find the next semi-random patrol point
    protected void FindNextPoint()
    {
        int rndIndex = Random.Range(0, pointList.Length);
        destPos = pointList[rndIndex].transform.position;
    }


    // Check the collision with the bullet
    void OnCollisionEnter(Collision collision)
    {
        // Reduce health
        if (collision.gameObject.tag == "Bullet")
            health -= collision.gameObject.GetComponent<Bullet>().damage;
    }


    protected void Explode()
    {
        float rndX = Random.Range(8.0f, 12.0f);
        float rndZ = Random.Range(8.0f, 12.0f);
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
        Invoke("CreateFinalExplosionEffect", 1.4f);
        Destroy(gameObject, 1.5f);
    }


    protected void CreateFinalExplosionEffect()
    {
        if (explosion)
            Instantiate(explosion, transform.position, transform.rotation);
    }

    void ApplyDamage(int dmg)
    {
        health -= dmg;
    }

}
