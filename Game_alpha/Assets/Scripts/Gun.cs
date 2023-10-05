using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Transform muzzle;
    public GameObject impact;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Vector3 screenCenterInWorldSpace = Camera.main.ScreenToWorldPoint(screenCenter);

            // Calculate the direction from muzzle to screen center
            Vector3 direction = (screenCenterInWorldSpace - muzzle.position).normalized;

            // Create a ray from the muzzle to the center of the screen
            Ray ray = new Ray(muzzle.position, direction);

            RaycastHit hitinfo;

            if (Physics.Raycast(ray, out hitinfo, range))
            {
                Debug.Log(hitinfo.transform.name);

                Target target = hitinfo.transform.GetComponent<Target>();
                if(target != null)
                {
                    target.TakeDamage(damage);       
                }

                GameObject impactEffect = Instantiate(impact, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
                Debug.Log("Impact Effect Instantiated at: " + impactEffect.transform.position);
                Destroy(impactEffect, .15f);            }

        }
    }

    //To visualize Gizmos in the Scene view, uncomment the OnDrawGizmos method
     void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(muzzle.position, muzzle.forward * range);
    }
}
