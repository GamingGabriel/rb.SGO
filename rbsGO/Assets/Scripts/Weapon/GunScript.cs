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
                //Transform bounceTrans = hit.transform;
                Collider[] enemyHits = Physics.OverlapSphere(bounceHit, 100, LayerMask.GetMask("Enemy"));
                if(enemyHits.Length > 0)
                {

                    Collider closest = enemyHits[0];
                    foreach(Collider c in enemyHits)
                    {
                        Vector3 dir = (c.transform.position - bounceHit).normalized;
                        //RaycastHit los;
                        Debug.DrawRay(bounceHit, dir * 100, Color.red, 2);
                        //print(Physics.Raycast(bounceHit, dir, out hit, float.MaxValue, LayerMask.GetMask("Default")));
                        print("Current Closest: " + closest.name + Vector3.Distance(bounceHit, closest.transform.position));
                        print("Check: " + c.name + Vector3.Distance(bounceHit, c.transform.position));
                        //print(hit.transform.name);
                        if ((Vector3.Distance(bounceHit, closest.transform.position) > Vector3.Distance(bounceHit, c.transform.position))
                            && (Physics.Raycast(bounceHit, dir, out hit, 100)))
                        {
                            closest = c;
                        }
                        print("Winner: " + closest.name);
                    }
                    //Debug.DrawRay(hit.transform.position, hit.transform.forward * 100, Color.blue, 2);
                    print(closest.name + Vector3.Distance(bounceHit, closest.transform.position));
                    //Debug.DrawRay(bounceHit, dir * 100, Color.red, 2);
                    closest.GetComponent<EnemyScript>().TakeDamage(1);
                    BulletTrail(bounceHit, closest.transform.position);
                }
            }
            GameObject impactObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal)); 
            Destroy(impactObj, .5f);
        }
        else
        {
            BulletTrail(spawnPoint.transform.position, cam.transform.forward * 50 );
        }
    }

    public void BulletTrail(Vector3 origin, Vector3 end)
    {
            GameObject trail = Instantiate(bulletTrail, origin, Quaternion.Euler(Vector3.forward));
            BulletTrailScript trailScript = trail.GetComponent<BulletTrailScript>();
            trailScript.endPoint = end;
    }


}
