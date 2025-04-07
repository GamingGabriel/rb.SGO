using UnityEngine;

public class GunScript : MonoBehaviour
{

    [SerializeField]
    Camera cam;
    
    [SerializeField]
    ParticleSystem muzzleFlash;

    [SerializeField]
    GameObject impactEffect;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        muzzleFlash.Play();
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, float.MaxValue))
        {
            Debug.Log(hit.transform.name);

            GameObject impactObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal)); 
            Destroy(impactObj, .5f);
        }
    }
}
