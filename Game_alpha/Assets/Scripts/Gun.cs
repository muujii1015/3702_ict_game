using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Gun : MonoBehaviour
{
    //Attributes of the gun
    public float damage = 10f;
    public float range = 100f;
    public float FireRate = 15f;
    public int AmmoCapacity = 10;
    private int CurrentAmmo;
    public float ReloadTime = 2.5f;
    private bool isReloading;

    //GameObjects and other components used in the script
    public Camera playerCam;
    public GameObject impact;
    public GameObject MuzzleFlash;
    public Animator ReloadAnimation;
    public AudioClip reloadSound;
    public GameObject bulletPrefab;
    public Transform muzzle;

    //public Canvas canvas;
    public TextMeshProUGUI AmmoDisplay;

    private AudioSource audioSource;

    private float timeToFire = 0f;

    void Start()
    {
        MuzzleFlash.SetActive(false);
        CurrentAmmo = AmmoCapacity;
        isReloading = false;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(CurrentAmmo ==0 && isReloading == false)
        {
            isReloading = true;
            StartCoroutine(Reload());
            return;
        }

        AmmoDisplay.text = "Ammo: " + CurrentAmmo.ToString();

        if (Input.GetButton("Fire1") && Time.time >= timeToFire && CurrentAmmo >=1)
        {
            timeToFire = Time.time + 1f / FireRate;
            Shoot();
        }
    }

    IEnumerator Reload()
    {
        audioSource.PlayOneShot(reloadSound);
        ReloadAnimation.SetBool("Reloading", true);

        yield return new WaitForSeconds(ReloadTime - .3f);

        ReloadAnimation.SetBool("Reloading", false);

        yield return new WaitForSeconds(.3f);
        CurrentAmmo = AmmoCapacity;
        isReloading = false;
    }

    void Shoot()
    {
        CurrentAmmo--;

        audioSource.Play();
        PlayMuzzleFlash();

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

            if (bulletPrefab != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);

                // Start bullet movement towards hit point
                StartCoroutine(MoveBullet(bullet, hitinfo.point));
                
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

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawRay(playerCam.transform.position, playerCam.transform.forward * range);
    //}

    IEnumerator MoveBullet(GameObject bullet, Vector3 targetPosition)
    {
        float startTime = Time.time;
        float duration = .04f; // Adjust this to control the bullet travel speed

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            bullet.transform.position = Vector3.Lerp(muzzle.position, targetPosition, t);
            yield return null;
        }

        // Ensure the bullet reaches the target position
        bullet.transform.position = targetPosition;
        Destroy(bullet);
    }
}
