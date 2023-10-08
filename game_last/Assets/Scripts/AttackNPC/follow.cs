using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class follow : MonoBehaviour

{


    public NavMeshAgent enemy;
    public Transform player;
    public GameObject enemyBullet;

    [SerializeField] private float timer = 5;
    private float bulletTime;

    public Transform spawnPoint;

    public float enemySpeed;



    void Start()
    {

    }
    void Update()
    {
        enemy.SetDestination(player.position);
        ShootAtPlayer();
    }
    void ShootAtPlayer()
    {
        bulletTime -= Time.deltaTime;

        if (bulletTime > 0) return;

        bulletTime = timer;
        GameObject bulletObj = Instantiate(enemyBullet, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
        bulletRig.AddForce(bulletRig.transform.forward * enemySpeed);
        Destroy(bulletObj, 5f);
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }







    // Start is called before the first frame update
}
    
