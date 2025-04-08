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
            //Intsntiate Bullet Trail Object; maybe make function? idk
            /*GameObject trail = Instantiate(bulletTrail, spawnPoint.transform.position, Quaternion.Euler(spawnPoint.transform.forward));
            BulletTrailScript trailScript = trail.GetComponent<BulletTrailScript>(); 
            trailScript.endPoint = hit.point;*/

            BulletTrail(spawnPoint.transform.position, hit.point);

            if(hit.transform.CompareTag("Bounce"))
            {
                Debug.DrawRay(hit.transform.position, hit.transform.forward * 100, Color.blue, 2);
                Debug.Log("Bounce Hit!");
                
                Vector3 bounceHit = hit.point;
                Transform bounceTrans = hit.transform;
                Collider[] enemyHits = Physics.OverlapSphere(bounceHit, 100, LayerMask.GetMask("Enemy"));
                if(enemyHits.Length > 0)
                {

                    Collider closest = enemyHits[0];
                    foreach(Collider c in enemyHits)
                    {
                        if (Vector3.Distance(bounceHit, closest.transform.position) > Vector3.Distance(bounceHit, c.transform.position))
                        {
                            closest = c;
                        }
                    }
                    //Debug.DrawRay(hit.transform.position, hit.transform.forward * 100, Color.blue, 2);
                    closest.GetComponent<EnemyScript>().TakeDamage(1);
                    BulletTrail(bounceHit, closest.transform.position);
                }
            }
            


            //get trailScript from spawned trail
            //do the thing

            /*GameObject impactObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal)); 
            Destroy(impactObj, .5f);*/
        }
    }

    public void BulletTrail(Vector3 origin, Vector3 end)
    {
            GameObject trail = Instantiate(bulletTrail, origin, Quaternion.Euler(Vector3.forward));
            BulletTrailScript trailScript = trail.GetComponent<BulletTrailScript>();
            trailScript.endPoint = end;
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
