using System.Drawing;
using UnityEngine;

public class GunScript : MonoBehaviour
{

    [SerializeField]
    Camera cam;
    
    [SerializeField]
    ParticleSystem muzzleFlash;

    [SerializeField]
    GameObject impactEffect;

    [SerializeField]
    GameObject bulletTrail;

    [SerializeField]
    Transform spawnPoint;
    
    [SerializeField]
    float trailDuration;

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

            //Intsntiate Bullet Trail Object
            GameObject trail = Instantiate(bulletTrail, spawnPoint.transform.position, Quaternion.Euler(spawnPoint.transform.forward));
            BulletTrailScript trailScript = trail.GetComponent<BulletTrailScript>();
            trailScript.endPoint = hit.point;

            //get trailScript from spawned trail
            //do the thing

            //SpawnTrail(hit.point, trailDuration);
            GameObject impactObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal)); 
            Destroy(impactObj, .5f);
        }
    }

    /*void SpawnTrail(Vector3 endPoint, float duration)
    {
        
        GameObject trail = Instantiate(bulletTrail, spawnPoint.transform.position, Quaternion.Euler(spawnPoint.transform.forward));
        Vector3 startPosition = spawnPoint.transform.position;
        
        //float distance = Vector3.Distance(trail.transform.position, endPoint);
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            Debug.Log(elapsedTime);
            float currentPosition = elapsedTime / duration; 
            trail.transform.position = Vector3.Lerp(startPosition, endPoint, currentPosition);
            //Debug.Log(trail.transform.position);

            //Debug.Log(currentPosition);
        }

        

    }
    */
}
