using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float FireRate = 15f;

    public Camera playerCam;
    public GameObject impact;
    public GameObject MuzzleFlash;

    private float timeToFire = 0f;

    void Start()
    {
        MuzzleFlash.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1f / FireRate;
            Shoot();

            PlayMuzzleFlash();          

        }
    }

    void Shoot()
    {
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        RaycastHit hitinfo;

        if (Physics.Raycast(ray, out hitinfo, range))
        {
            Debug.Log(hitinfo.transform.name);

            Target target = hitinfo.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }



            GameObject impactEffect = Instantiate(impact, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
            Destroy(impactEffect, .15f);
        }
    }

    void PlayMuzzleFlash()
    {
        if (MuzzleFlash != null)
        {
            MuzzleFlash.SetActive(true);

            Invoke("DisableMuzzleFlash", .1f);
        }
    }

    void DisableMuzzleFlash()
    {
        MuzzleFlash.SetActive(false);
    }

    
     void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(playerCam.transform.position, playerCam.transform.forward * range);
    }
}
